using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.IdentityModel.Tokens;

namespace campaign_hub.Services
{
    public class EncyclopediaService
    {
        public enum Source
        {
            Campaign,
            Character,
            Guild,
            Color
        };
        public List<KeyValuePair<int, Source>> entries { get; set; } = new();
        public List<CampaignModel> campaigns { get; set; } = new();
        public List<CharacterModel> characters { get; set; } = new();
        public List<GuildModel> guilds { get; set; } = new();
        public List<FilterService.Colors> colors { get; set; } = Enum.GetValues<FilterService.Colors>().ToList();
        public async Task LoadEncyclopedia(ICampaignData _db)
        {
            campaigns = await _db.getCampaigns();
            await Task.CompletedTask;
        }
        public async Task LoadEncyclopedia(ICharacterData _db)
        {
            characters = await _db.getCharacters();
            await Task.CompletedTask;
        }
        public async Task LoadEncyclopedia(IGuildData _db)
        {
            guilds = await _db.getGuilds();
            await Task.CompletedTask;
        }
        public void SortEncyclopedia(FilterService filter)
        {
            entries.Clear();
            switch (filter.CategoryFilter)
            {
                case FilterService.Categories.Character:
                    SortCharacters(filter);
                    break;
                case FilterService.Categories.Guild:
                    SortGuilds(filter);
                    break;
                case FilterService.Categories.Location:
                    // Implement SortLocation 
                    break;
                case FilterService.Categories.Event:
                    // Implement SortEvent 
                    break;
                case FilterService.Categories.Other:
                    // Implement SortOther 
                    break;
            }
            // Sorting based on GroupFilter
            switch (filter.GroupFilter)
            {
                case FilterService.Groups.Name:
                    entries = entries.OrderBy(GetName).ToList();
                    break;
                case FilterService.Groups.Color:
                    GroupByColor(filter);
                    break;
                case FilterService.Groups.Guild:
                    if (filter.CategoryFilter != FilterService.Categories.Guild)
                        GroupByGuild(filter);
                    break;
                case FilterService.Groups.Class:
                    // Implement GroupByClass 
                    break;
                case FilterService.Groups.Plane:
                    // Implement GroupByPlane 
                    break;
                case FilterService.Groups.Campaign:
                    GroupByCampaign(filter);
                    break;
            }
            return;
        }

        private void GroupByCampaign(FilterService filter)
        {
            List<KeyValuePair<int, Source>> temp = new();
            foreach (var campaign in campaigns)
            {
                var selected_entries = entries.Where(e => GetCampaign(e) == campaign.Id).ToList();
                if (!selected_entries.IsNullOrEmpty())
                {
                    KeyValuePair<int, Source> this_selection = new KeyValuePair<int, Source>(campaigns.IndexOf(campaign), Source.Campaign);
                    temp.Add(this_selection);
                    temp.AddRange(selected_entries);
                }
            }
            entries = temp;
        }

        private void GroupByGuild(FilterService filter)
        {
            List<KeyValuePair<int, Source>> temp = new();
            foreach (var guild in guilds)
            {
                var selected_entries = entries.Where(e => GetTags(e).Contains(ParseTags(guild.Tags)[0])).ToList();
                if (!selected_entries.IsNullOrEmpty())
                {
                    KeyValuePair<int, Source> this_selection = new KeyValuePair<int, Source>(guilds.IndexOf(guild), Source.Guild);
                    temp.Add(this_selection);
                    temp.AddRange(selected_entries);
                }
            }
            entries = temp;
        }

        private void GroupByColor(FilterService filter)
        {
            List<KeyValuePair<int, Source>> temp = new();
            if (filter.multicolored) GroupMultiColor(filter);
            else GroupMonoColor(filter);
        }

        private void GroupMultiColor(FilterService filter)
        {
            List<KeyValuePair<int, Source>> temp = new();
            foreach (var entry in entries)
            {
                bool matches_all = true;
                var entry_tags = GetTags(entry);
                if (entry_tags.IsNullOrEmpty()) continue;
                foreach (var color in filter.ColorFilter)
                {
                    if (!entry_tags.Contains(color.ToString()))
                    {
                        matches_all = false;
                        break;
                    }
                }
                if (matches_all)
                    temp.Add(entry);
            }
            entries = temp;
        }

        private void GroupMonoColor(FilterService filter)
        {
            List<KeyValuePair<int, Source>> temp = new();
            foreach (var color in colors)
            {
                if (filter.ColorFilter.Contains(color))
                {
                    var selected_entries = entries.Where(e => GetTags(e).Contains(color.ToString())).ToList();
                    if (!selected_entries.IsNullOrEmpty())
                    {
                        KeyValuePair<int, Source> this_selection = new KeyValuePair<int, Source>(colors.IndexOf(color), Source.Color);
                        temp.Add(this_selection);
                        temp.AddRange(selected_entries);
                    }
                }
            }
            entries = temp;
        }

        private void SortCharacters(FilterService filter)
        {
            foreach (int i in indexes(characters.Count(), filter.ascending))
            {
                if (filter.CampaignFilter == "0000" || characters[i].campaign_id.ToString() == filter.CampaignFilter)
                    entries.Add(new KeyValuePair<int, Source>(i, Source.Character));
            }
        }

        private void SortGuilds(FilterService filter)
        {
            foreach (int i in indexes(guilds.Count(), filter.ascending))
            {
                if (filter.CampaignFilter == "0000" || guilds[i].Campaign_Id.ToString() == filter.CampaignFilter)
                    entries.Add(new KeyValuePair<int, Source>(i, Source.Guild));
            }
        }

        private string? GetName(KeyValuePair<int, Source> entry)
        {
            return entry.Value switch
            {
                EncyclopediaService.Source.Character => characters[entry.Key].first_name,
                EncyclopediaService.Source.Guild => guilds[entry.Key].Name,
                _ => "" // base case
            };
        }
        private string? GetTags(KeyValuePair<int, Source> entry)
        {
            return entry.Value switch
            {
                EncyclopediaService.Source.Character => characters[entry.Key].tags,
                EncyclopediaService.Source.Guild => guilds[entry.Key].Tags,
                //EncyclopediaService.Source.Campaign => campaigns[entry.Key].tags,
                _ => "" // base case
            };
        }

        private int GetCampaign(KeyValuePair<int, Source> entry)
        {
            return entry.Value switch
            {
                EncyclopediaService.Source.Character => characters[entry.Key].campaign_id.Value,
                EncyclopediaService.Source.Guild => guilds[entry.Key].Campaign_Id.Value,
                _ => 0000 // base case
            };
        }

        private List<string> ParseTags(string tags)
        {
            return tags.TrimEnd().Split(',').ToList();
        }


        //public Array indexes(int list_count, bool ascending)
        //{
        //    if (ascending) return Enumerable.Range(0, list_count).ToArray();
        //    else return Enumerable.Range(0, list_count).Reverse().ToArray();
        //}

        // I think this is faster because it doesn't create/reverse an array
        public IEnumerable<int> indexes(int list_count, bool ascending)
        {
            if (ascending)
                for (int i = 0; i < list_count; i++)
                    yield return i;
            else
                for (int i = list_count - 1; i >= 0; i--)
                    yield return i;
        }

        public string FullName(int key)
        {
            return characters[key].first_name + " " + characters[key].last_name;
        }
    }
}

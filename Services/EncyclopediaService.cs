using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Routing.Template;
using System.ComponentModel;
using System.Drawing;

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
                    // Implement GroupByCampaign 
                    break;
            }
            return;
        }

        private void GroupByGuild(FilterService filter)
        {
            List<KeyValuePair<int, Source>> temp = new();
            foreach (var guild in guilds)
            {
                KeyValuePair<int, Source> this_selection = new KeyValuePair<int, Source>(guilds.IndexOf(guild), Source.Guild);
                temp.Add(this_selection);
                var selected_entries = entries.Where(e => GetTags(e).Contains(ParseTags(guild.Tags)[0])).ToList();
                temp.AddRange(selected_entries);
            }
            entries = temp;
        }

        private void GroupByColor(FilterService filter)
        {
            List<KeyValuePair<int, Source>> temp = new();
            foreach (var color in colors)
            {
                KeyValuePair<int, Source> this_selection = new KeyValuePair<int, Source>(colors.IndexOf(color), Source.Color);
                temp.Add(this_selection);
                var selected_entries = entries.Where(e => GetTags(e).Contains(color.ToString())).ToList();
                temp.AddRange(selected_entries);
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

        private List<string> ParseTags(string tags)
        {
            return tags.TrimEnd().Split(',').ToList();
        }

        //private string? ExtractColors(string tags)
        //{
        //    var parsed = ParseTags(tags);
        //    return parsed.ToList<string>();
        //}


        //public Array indexes(int list_count, bool ascending)
        //{
        //    if (ascending) return Enumerable.Range(0, list_count).ToArray();
        //    else return Enumerable.Range(0, list_count).Reverse().ToArray();
        //}

        // I think this is faster because it doesn't create/reverse an array
        public IEnumerable<int> indexes(int list_count, bool ascending)
        {
            if (ascending)
            {
                for (int i = 0; i < list_count; i++)
                    yield return i;
            }
            else
            {
                for (int i = list_count - 1; i >= 0; i--)
                    yield return i;
            }
        }

    }
}

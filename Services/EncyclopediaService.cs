using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Routing.Template;

namespace campaign_hub.Services
{
    public class EncyclopediaService
    {
        public enum Source
        {
            Campaign,
            Character,
            Guild
        };
        public List<KeyValuePair<int, Source>> entries { get; set; } = new();
        public List<CampaignModel> campaigns { get; set; } = new();
        public List<CharacterModel> characters { get; set; } = new();
        public List<GuildModel> guilds { get; set; } = new();
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
                    foreach (int i in indexes(characters.Count(), filter.ascending))
                    {
                        if (filter.CampaignFilter == "0000" || characters[i].campaign_id.ToString() == filter.CampaignFilter)
                            entries.Add(new KeyValuePair<int, Source>(i, Source.Character));
                    }
                    break;
                case FilterService.Categories.Guild:
                    foreach (int i in indexes(guilds.Count(), filter.ascending))
                    {
                        if (filter.CampaignFilter == "0000" || guilds[i].Campaign_Id.ToString() == filter.CampaignFilter)
                            entries.Add(new KeyValuePair<int, Source>(i, Source.Guild));
                    }
                    break;
                    // Additional cases for Location, Event, Other can be added here
            }
            // Sorting based on GroupFilter
            entries = filter.GroupFilter switch
            {
                FilterService.Groups.Name => entries.OrderBy(e => e.Value).ToList(),
                _ => entries
            };
            return;
        }

        //public Array indexes(int list_count, bool ascending)
        //{
        //    if (ascending) return Enumerable.Range(0, list_count).ToArray();
        //    else return Enumerable.Range(0, list_count).Reverse().ToArray();
        //}

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

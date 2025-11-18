using campaign_hub.Components.Pages;
using DataAccessLibrary.Models;
using DataAccessLibrary;
using Microsoft.AspNetCore.Components;

namespace campaign_hub.Services
{
    public class FilterService
    {
        public event Action? FiltersChanged;
        public bool FiltersEnabled { get; set; } = false;
        public Categories CategoryFilter { get; set; } = Categories.Character;
        public string CampaignFilter { get; set; } = "0000";
        public Groups GroupFilter { get; set; } = Groups.Name;

        public enum Categories
        {
            Character,
            Location,
            Guild,
            Event,
            Other,
        };
        public enum Groups
        {
            Name,
            Color,
            Guild,
            Class,
            Plane
        };
        public enum Classes
        {
            Barbarian,
            Bard,
            Cleric,
            Druid,
            Fighter,
            Monk,
            Paladin,
            Ranger,
            Rogue,
            Sorcerer,
            Warlock,
            Wizard,
            Artificer,
            Illrigger
        }
        public enum Colors
        {
            white,
            blue,
            black,
            red,
            green
        }
        public void ResetAllFilters()
        {
            CategoryFilter = Categories.Character;
            CampaignFilter = "0000";
            GroupFilter = Groups.Name;
            FiltersChanged?.Invoke();
        }
        public void SetCategoryFilter(ChangeEventArgs e)
        {
            try
            {
                Categories result;
                Enum.TryParse<Categories>(e.Value.ToString(), out result);
                CategoryFilter = result;
                FiltersChanged?.Invoke();
            }
            catch
            {
                //error
            }
        }
        public void SetCampaignFilter(ChangeEventArgs e)
        {
            try
            {
                CampaignFilter = e.Value.ToString();
                FiltersChanged?.Invoke();
            }
            catch
            {
                //error
            }
        }
        public void SetGroupFilter(ChangeEventArgs e)
        {
            try
            {
                Groups result;
                Enum.TryParse<Groups>(e.Value.ToString(), out result);
                GroupFilter = result;
                FiltersChanged?.Invoke();
            }
            catch
            {
                //error
            }
        }

        public bool Toggle()
        {
            return FiltersEnabled = !FiltersEnabled;
        }
    }
}

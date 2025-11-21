using campaign_hub.Components.Pages;
using DataAccessLibrary.Models;
using DataAccessLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;

namespace campaign_hub.Services
{
    public class FilterService
    {
        public event Action? FiltersChanged;
        public bool FiltersEnabled { get; set; } = false;
        public Categories CategoryFilter { get; set; } = Categories.Character;
        public string CampaignFilter { get; set; } = "0000";
        public Groups GroupFilter { get; set; } = Groups.Name;
        public List<Colors> ColorFilter { get; set; } =
            new List<Colors> { Colors.white, Colors.blue, Colors.black, Colors.green, Colors.red };
        public bool multicolored = false;
        public bool ascending { get; set; } = true;

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
            Plane,
            Campaign
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
            multicolored = false;
            ColorFilter = new List<Colors> { Colors.white, Colors.blue, Colors.black, Colors.green, Colors.red };
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

        public string? GetColorFilter()
        {
            if (ColorFilter.IsNullOrEmpty()) return null;
            string result = "";
            foreach (Colors color in ColorFilter)
                result = result + "," + color.ToString();
            return result.Substring(1);
        }

        public void Toggle_Color(string color)
        {
            try
            {
                Colors result;
                Enum.TryParse<Colors>(color, out result);
                if (ColorFilter.Contains(result)) ColorFilter.Remove(result);
                else ColorFilter.Add(result);
                FiltersChanged?.Invoke();
            }
            catch
            {
                //error
            }
        }

        public void Toggle_Multi()
        {
            multicolored = !multicolored;
            FiltersChanged?.Invoke();
        }

        public bool Toggle_Asc_Desc()
        {
            ascending = !ascending;
            FiltersChanged?.Invoke();
            return ascending;
        }

        public bool Toggle()
        {
            return FiltersEnabled = !FiltersEnabled;
        }
    }
}

using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace campaign_hub.Services
{
    public class NewCharacterService
    {
        public CharacterModel model { get; set; } = new();
        public bool menu_open = false;
        public bool photo_menu_open = false;
        public bool info_menu_open = false;
        public bool stats_menu_open = false;

        public void toggleMenu()
        {
            if (menu_open) menu_open = false;
            else menu_open = true;
            return;
        }

        public bool toggleExtra(string button)
        {
            switch (button)
            {
                case "photo":
                    info_menu_open = stats_menu_open = false;
                    return photo_menu_open = !photo_menu_open;
                case "info":
                    photo_menu_open = stats_menu_open = false;
                    return info_menu_open = !info_menu_open;
                case "stats":
                    info_menu_open = photo_menu_open = false;
                    return stats_menu_open = !stats_menu_open;
            }
            return false;
        }

        public Task setImg(string src)
        {
            model.img = src;
            return Task.CompletedTask;
        }

        public Task toggleTag(string tag)
        {
            if (model.tags == null || !model.tags.Contains(tag))
                addTag(tag);
            else
            {
                List<string> tags = ParseTags(model.tags);
                model.tags = null;
                foreach (string t in tags)
                    if (t != tag)
                        addTag(t);
            }
            return Task.CompletedTask;
        }

        private Task addTag(string tag)
        {
            if (model.tags == null)
                model.tags = tag;
            else if (!model.tags.Contains(tag))
                model.tags = model.tags + "," + tag;
            return Task.CompletedTask;
        }

        private List<string> ParseTags(string tags)
        {
            return tags.TrimEnd().Split(',').ToList();
        }

    }
}

using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;

namespace campaign_hub.Services.NewCharacterService
{
    public class NewCharacterService
    {
        public CharacterModel model { get; set; } = new();
        public List<int> stats { get; set; } = new() { 10, 10, 10, 10, 10, 10 };
        public bool menu_open = false;
        public bool photo_menu_open = false;
        public bool info_menu_open = false;
        public bool stats_menu_open = false;
        public event Action? OnChange;

        public void reset(int? user_id)
        {
            model = new CharacterModel();
            menu_open = false;
            photo_menu_open = false;
            info_menu_open = false;
            stats_menu_open = false;
            model.user_id = user_id.Value;
            OnChange?.Invoke();
        }

        public async Task SubmitCharacter(ICharacterData _db)
        {
            try
            {
                await _db.UpsertCharacter(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating character: {ex.Message}");
                await Task.CompletedTask;
            }

        }

        public bool toggleMenu()
        {
            menu_open = !menu_open;
            OnChange?.Invoke();
            return menu_open;
        }

        public void openMenu()
        {
            menu_open = true;
            OnChange?.Invoke();
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

        public bool canSubmit()
        {
            if (model.user_id == null || model.campaign_id == null || model.first_name == null)
            {
                return false;
            }
            return true;
        }

    }
}

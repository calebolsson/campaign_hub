using campaign_hub.Components.Pages;
using DataAccessLibrary.Models;
using DataAccessLibrary;

namespace campaign_hub.Services.AccountService
{
    public class AccountService
    {
        public event Action? AccountChanged;
        public AccountModel? account { get; set; } = null;
        public CharacterModel? character { get; set; } = null;

        public bool IsLoggedIn()
        {
            return account != null && character != null;
        }

        public async Task<AccountModel?> SetAccount(IAccountData _account_db, ICharacterData _char_db, int? user_id)
        {
            if (user_id != null)
            {
                account = await _account_db.getAccount(user_id.Value);
                if (account != null && account.Char_Id != null)
                    character = await _char_db.getCharacter(account.Char_Id.Value);
            }
            AccountChanged?.Invoke();
            return account;
        }
    }
}

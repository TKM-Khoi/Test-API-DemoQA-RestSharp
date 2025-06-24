using Service.Const;
using Service.Models.DTOs;

using static Core.Utils.JsonFileUtils;

namespace Test.DataProvider
{
    public class AccountDataProvider
    {
        private static Dictionary<string, AccountDto> _accountData;

        public static AccountDto LoadAccountDataFile(string key)
        {
            AccountDto emptyAccount = new() { Username = "", Password = "", UserId = "-1" };
            if (String.IsNullOrWhiteSpace(key))
            {
                return emptyAccount;
            }
            if (_accountData == null || _accountData.Count == 0)
            {
                _accountData = JsonFileUltils.ReadJsonAndParse<Dictionary<string, AccountDto>>(
                    FilePathConst.ACCOUNT_DATA);
            }
            if (!_accountData.ContainsKey(key))
            {
                return emptyAccount;
            }
            return _accountData[key];
        }
    }
}
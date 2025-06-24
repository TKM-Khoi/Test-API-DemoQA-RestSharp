
namespace Core.ShareData
{
    public class DataStorage
    {
        //Have to seperate by feature because ParallelScope.Fixtures => TestCases of different features runs at the same time
        //=>TestCase may Tear down at the same time=> Access <Set of users need to delete books> at the same time 
        //=>Remove set's child while others are accessing => Collection was modified; enumeration operation may not execute.
         //=>If ParallelScope.All =>Need to revise test data and seperate sets into testcase
        public static string CREATED_BOOKS_USERS_KEY(string featureName) => $"createdBooksUser-{featureName}";
        private static AsyncLocal<Dictionary<string, object>> _data = new AsyncLocal<Dictionary<string, object>>();
        public static void InitData()
        {
            _data.Value = new Dictionary<string, object>();
        }
        public static void SetData(string key, object value)
        {
            if (_data.Value.ContainsKey(key))
            {
                _data.Value[key] = value;
            }
            else
            {
                _data.Value.Add(key, value);
            } 
        }
        public static object GetData(string key)
        {
            if (!_data.Value.ContainsKey(key))
            {
                return null;
            }
            return _data.Value.GetValueOrDefault(key);
        }
        public static Dictionary<string, object> GetAllData() => _data.Value;
        public static void ClearData()
        {
            if (_data.Value is not null)
            {
                _data.Value.Clear();
            }
        }
    }
}
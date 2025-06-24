using ProjectTest.DataModels;

using Service.Const;

using static Core.Utils.JsonFileUtils;

namespace Test.DataProvider
{
    public class ReplaceBookDataProvider
    {
        private static Dictionary<string, ReplaceBookData> _replaceData;

        public static Dictionary<string, ReplaceBookData> LoadReplaceBookDataFile()
        {
            if (_replaceData == null || _replaceData.Count == 0)
            {
                _replaceData = JsonFileUltils.ReadJsonAndParse<Dictionary<string, ReplaceBookData>>(
                    FilePathConst.REPLACE_BOOK_DATA);
            }
            return _replaceData;
        }
        
        public static IEnumerable<ReplaceBookData> GetData(string key)
        {
            yield return LoadReplaceBookDataFile()[key];
        }
        public static IEnumerable<ReplaceBookData> ReplaceValidBook()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_SUCCESS];
        }
         public static IEnumerable<ReplaceBookData> ReplaceNotOwnOldBook()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_NOTOWN_OLD_BOOK];
        }
        public static IEnumerable<ReplaceBookData> ReplaceOldBookNotFound()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_NOTFOUND_OLD_BOOK];
        }
         public static IEnumerable<ReplaceBookData> ReplaceDuplicateNewBook()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_DUPLICATE_NEW_BOOK];
        }
        public static IEnumerable<ReplaceBookData> ReplaceNewBookNotFound()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_NOTFOUND_NEW_BOOK];
        }
        public static IEnumerable<ReplaceBookData> ReplaceBookNotFoundUser()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_NOTFOUND_USER];
        }
        public static IEnumerable<ReplaceBookData> ReplaceBookDifferentUser()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_DIFFERENT_USER];
        }
        public static IEnumerable<ReplaceBookData> NoAuthen()
        {
            yield return LoadReplaceBookDataFile()[TestDataKeyConst.REPLACE_BOOK_NO_AUTHEN];
        }
    }
}
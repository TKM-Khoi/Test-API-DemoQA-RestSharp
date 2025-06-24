using ProjectTest.DataModels;

using Service.Const;

using static Core.Utils.JsonFileUtils;

namespace Test.DataProvider;

public class DeleteBookDataProvider
{
    private static Dictionary<string, DeleteBookData> _deleteData;

    public static Dictionary<string, DeleteBookData> LoadDeleteBookDataFile()
    {
        if (_deleteData == null || _deleteData.Count == 0)
        {
            _deleteData = JsonFileUltils.ReadJsonAndParse<Dictionary<string, DeleteBookData>>(
                FilePathConst.DELETE_BOOK_DATA);
        }
        return _deleteData;
    }
    
    public static IEnumerable<DeleteBookData> GetData(string key)
    {
        yield return LoadDeleteBookDataFile()[key];
    }
    public static IEnumerable<DeleteBookData> DeleteValidBook()
    {
        return GetData(TestDataKeyConst.DEL_BOOK_SUCCESS);
    }
    public static IEnumerable<DeleteBookData> DeleteNotOwnBook()
    {
        return GetData(TestDataKeyConst.DEL_BOOK_NOTOWN);
    }
    public static IEnumerable<DeleteBookData> DeleteBookNotFound()
    {
        return GetData(TestDataKeyConst.DEL_BOOK_NOTFOUND_BOOK);
    }
    public static IEnumerable<DeleteBookData> DeleteBookWrongUser()
    {
        return GetData(TestDataKeyConst.DEL_BOOK_DIFFERENT_USER);
    }
    public static IEnumerable<DeleteBookData> DeleteBookNotFoundUser()
    {
        return GetData(TestDataKeyConst.DEL_BOOK_NOTFOUND_USER);
    }
    public static IEnumerable<DeleteBookData> NoAuthen()
    {
        return GetData(TestDataKeyConst.DEL_BOOK_NO_AUTHEN);
    }
}
using ProjectTest.DataModels;

using Service.Const;

using static Core.Utils.JsonFileUtils;

namespace Test.DataProvider;

public class AddBookDataProvider
{
    private static Dictionary<string, AddBookData> _addData;

    public static Dictionary<string, AddBookData> LoadDeleteBookDataFile()
    {
        if (_addData == null || _addData.Count == 0)
        {
            _addData = JsonFileUltils.ReadJsonAndParse<Dictionary<string, AddBookData>>(
                FilePathConst.ADD_BOOK_DATA);
        }
        return _addData;
    }
    public static IEnumerable<AddBookData> GetData(string key)
    {
        yield return LoadDeleteBookDataFile()[key];
    }
    public static IEnumerable<AddBookData> AddValidBook()
    {
        yield return LoadDeleteBookDataFile()[TestDataKeyConst.ADD_BOOK_SUCCESS];
    }
    public static IEnumerable<AddBookData> AddDuplicateBook()
    {
        yield return LoadDeleteBookDataFile()[TestDataKeyConst.ADD_BOOK_DUPLICATE];
    }
    public static IEnumerable<AddBookData> AddBookNotFoundBook()
    {
        yield return LoadDeleteBookDataFile()[TestDataKeyConst.ADD_BOOK_NOTFOUND_BOOK];
    }
    public static IEnumerable<AddBookData> AddBookWrongUser()
    {
        yield return LoadDeleteBookDataFile()[TestDataKeyConst.ADD_BOOK_DIFFERENT_USER];
    }
    public static IEnumerable<AddBookData> UserNotFound()
    {
        yield return LoadDeleteBookDataFile()[TestDataKeyConst.ADD_BOOK_NOTFOUND_USER];
    }
    public static IEnumerable<AddBookData> NoAuthen()
    {
        yield return LoadDeleteBookDataFile()[TestDataKeyConst.ADD_BOOK_NO_AUTHEN];
    }
}

using Service.Const;
using Service.Models.DTOs;

using static Core.Utils.JsonFileUtils;

namespace Test.DataProvider
{
    public class BookDataProvider
    {
        private static Dictionary<string, BookDto> _bookData;

        public static BookDto LoadBookDataFile(string key)
        {
            BookDto emptyBook = new BookDto { Isbn= "-1"};
            if (String.IsNullOrWhiteSpace(key))
            {
                return emptyBook;
            }
            if (_bookData == null || _bookData.Count == 0)
            {
                _bookData = JsonFileUltils.ReadJsonAndParse<Dictionary<string, BookDto>>(
                    FilePathConst.BOOK_DATA);
            }
            if (!_bookData.ContainsKey(key))
            {
                return emptyBook;
            }
            return _bookData[key];
        }
    }
}
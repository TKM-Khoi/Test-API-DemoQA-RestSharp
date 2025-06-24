namespace Service.Const
{
    public class TestDataKeyConst
    {
        public static string GET_ACC_DETAIL_SUCCESS = "GetAccDetail_Success";
        public static string GET_ACC_DETAIL_NOTFOUND_USER = "GetAccDetail_NotFoundAccount";
        public static string GET_ACC_DETAIL_DIFFERENT_USER = "GetAccDetail_DifferentAccount";
        public static string GET_ACC_DETAIL_NO_AUTHEN = "GetAccDetail_NoAuthen";

        public static string ADD_BOOK_SUCCESS = "AddBook_Success";
        public static string ADD_BOOK_DUPLICATE = "AddBook_Duplicate";
        public static string ADD_BOOK_NOTFOUND_BOOK = "AddBook_NotFoundBook";
        public static string ADD_BOOK_NOTFOUND_USER = "AddBook_NotFoundUser";
        public static string ADD_BOOK_DIFFERENT_USER = "AddBook_DifferentUser";
        public static string ADD_BOOK_NO_AUTHEN = "AddBook_NoAuthentication";

        public static string DEL_BOOK_SUCCESS = "DeleteBook_Success";
        public static string DEL_BOOK_NOTOWN = "DeleteBook_NotOwnBook";
        public static string DEL_BOOK_NOTFOUND_BOOK = "DeleteBook_NotFoundBook";
        public static string DEL_BOOK_NOTFOUND_USER = "DeleteBook_NotFoundUser";
        public static string DEL_BOOK_DIFFERENT_USER = "DeleteBook_DifferentUser";
        public static string DEL_BOOK_NO_AUTHEN = "DeleteBook_NoAuthentication";

        public static string REPLACE_BOOK_SUCCESS = "ReplaceBook_Success";
        public static string REPLACE_BOOK_NOTOWN_OLD_BOOK = "ReplaceBook_NotOwn_Old";
        public static string REPLACE_BOOK_NOTFOUND_OLD_BOOK = "ReplaceBook_NotFound_Old";
        public static string REPLACE_BOOK_DUPLICATE_NEW_BOOK = "ReplaceBook_Duplicate_New";
        public static string REPLACE_BOOK_NOTFOUND_NEW_BOOK = "ReplaceBook_NotFound_New";
        public static string REPLACE_BOOK_NOTFOUND_USER = "ReplaceBook_NotFoundUser";
        public static string REPLACE_BOOK_DIFFERENT_USER = "ReplaceBook_DifferentUser";
        public static string REPLACE_BOOK_NO_AUTHEN = "ReplaceBook_NoAuthentication";

        public static string ACC_FOR_GET_DETAIL = "getDetail_account";
        public static string ACC_FOR_ADD_BOOK = "addBook_account";
        public static string ACC_FOR_DELETE_BOOK = "deleteBook_account";
        public static string ACC_FOR_REPLACE_BOOK = "replaceBook_account";

    }
}
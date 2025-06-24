namespace Service.Const
{
    public class ErrorConst
    {
        public static int ADD_DUP_BOOK_CODE = 1210;
        public static string ADD_DUP_BOOK_MSG = "ISBN already present in the User's Collection!";
        
        public static int NOT_FOUND_BOOK_CODE = 1205;
        public static string NOT_FOUND_BOOK_MSG = "ISBN supplied is not available in Books Collection!";

        public const int NOT_OWN_BOOK_CODE = 1206;
        public const string NOT_OWN_BOOK_MSG = "ISBN supplied is not available in User's Collection!";

                

        public static int NOT_FOUND_OR_WRONG_USER_ID_CODE = 1207;
        public static string NOT_FOUND_USER_ID_MSG = "User not found!";
        public static string WRONG_USER_ID_MSG = "User Id not correct!";
        
        public static int NOT_AUTHORIZED_CODE = 1200;
        public static string NOT_AUTHORIZED_MSG = "User not authorized!";
        
        
        
    }
}
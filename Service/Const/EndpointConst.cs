namespace Service.Const
{
    public class EndpointConst
    {
        public static string GEN_JWT_TOKEN_API  = "Account/v1/GenerateToken";
        public static string GET_USER_DETAL_API(string USERID)=> $"Account/v1/User/{USERID} ";
        public static string ADD_BOOK_API = "BookStore/v1/Books";
        public static string DELETE_BOOK_API = "BookStore/v1/Book";
        public static string DELETE_ALL_BOOKS_API = "BookStore/v1/Books";
        public static string REPLACE_BOOK_API(string USERID)=> $"BookStore/v1/Books/{USERID} ";
    }
}
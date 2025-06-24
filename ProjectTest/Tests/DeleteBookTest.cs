using System.Net;

using FluentAssertions;
using FluentAssertions.Execution;

using ProjectTest.DataModels;

using Service.Const;
using Service.Extensions;
using Service.Models.DTOs;
using Service.Models.Resquests;
using Service.Services;

using Test.Core.Extensions;
using Test.DataProvider;

namespace Test.Tests
{
    public class DeleteBookTest : BaseTest
    {
        [Test]
        [Category("delete"), Category("success")]
        [TestCaseSource(typeof(DeleteBookDataProvider), nameof(DeleteBookDataProvider.DeleteValidBook))]
        public async Task DeleteBookSuccess(DeleteBookData data)
        {
            //setup book to delete
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, book.Isbn);
            await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);

            //Begin Test
            var deleteBookResponse = await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, book.Isbn, logined.Username, logined.Password);

            //To delete the prep after test case if api fail and Book is not deleted
            if (deleteBookResponse.StatusCode != HttpStatusCode.NoContent)
            {
                BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);
            }

            //Verify
            var getDetailResponse = await AccountService.GetDetailUserWithUnameAndPasswordAsync(owner.UserId, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                deleteBookResponse.VerifyStatusCodeNoContent();
                deleteBookResponse.Content.Should().BeNullOrWhiteSpace();
                getDetailResponse.Data?.Books.Should().AllSatisfy(x=>x.Isbn.Should().NotBe(book.Isbn));
            }
        }

        /// <summary>
        /// User does not own that book
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("delete"), Category("fail")]
        [TestCaseSource(typeof(DeleteBookDataProvider), nameof(DeleteBookDataProvider.DeleteNotOwnBook))]
        public async Task DeleteBookUserDoesNotOwn(DeleteBookData data)
        {
            //Setup: Make sure user does not own book
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, book.Isbn, logined.Username, logined.Password);

            var deleteBookResponse = await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, book.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                deleteBookResponse.VerifyStatusCodeBadRequest();
                deleteBookResponse.VerifyErrorPayload(ErrorConst.NOT_OWN_BOOK_CODE, ErrorConst.NOT_OWN_BOOK_MSG);
            }
        }
        /// <summary>
        /// Book ISBN not in the system
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("delete"), Category("fail")]
        [TestCaseSource(typeof(DeleteBookDataProvider), nameof(DeleteBookDataProvider.DeleteBookNotFound))]
        public async Task DeleteBookIsbnNotFound(DeleteBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            var deleteBookResponse = await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, book.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                deleteBookResponse.VerifyStatusCodeBadRequest();
                deleteBookResponse.VerifyErrorPayload(ErrorConst.NOT_OWN_BOOK_CODE, ErrorConst.NOT_OWN_BOOK_MSG);
            }
        }
        /// <summary>
        /// UserId not in the system
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("delete"), Category("fail")]
        [TestCaseSource(typeof(DeleteBookDataProvider), nameof(DeleteBookDataProvider.DeleteBookNotFoundUser))]
        public async Task DeleteBookUserIdNotFound(DeleteBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            var deleteBookResponse = await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, book.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                deleteBookResponse.VerifyStatusCodeUnauthorized();
                deleteBookResponse.VerifyErrorPayload(ErrorConst.NOT_FOUND_OR_WRONG_USER_ID_CODE, ErrorConst.WRONG_USER_ID_MSG);
            }
        }
        /// <summary>
        /// UserId does not match with logged in user
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("delete"), Category("fail")]
        [TestCaseSource(typeof(DeleteBookDataProvider), nameof(DeleteBookDataProvider.DeleteBookWrongUser))]
        public async Task DeleteBookDifferentUserId(DeleteBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            var deleteBookResponse = await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, book.Isbn, logined.Username, logined.Password);

            using (new AssertionScope())
            {
                deleteBookResponse.VerifyStatusCodeUnauthorized();
                deleteBookResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
            }
        }
         /// <summary>
        /// User is not logged in
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("delete"), Category("fail")]
        [TestCaseSource(typeof(DeleteBookDataProvider), nameof(DeleteBookDataProvider.NoAuthen))]
        public async Task DeleteBookNoAuthen(DeleteBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            var deleteBookResponse = await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, book.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                deleteBookResponse.VerifyStatusCodeUnauthorized();
                deleteBookResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
            }
        }
    }
}
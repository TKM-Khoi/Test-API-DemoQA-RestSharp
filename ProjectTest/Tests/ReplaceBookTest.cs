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
    public class ReplaceBookTest : BaseTest
    {
        [Test]
        [Category("replace"), Category("success")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.ReplaceValidBook))]
        public async Task ReplaceValidBookSuccess(ReplaceBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            BookService.DeleteAllBooksWithUnameAndPass(owner.UserId, logined.Username, logined.Password);
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, oldBook.Isbn);

            await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);
            BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);

            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            //Verify
            var getDetailResponse = await AccountService.GetDetailUserWithUnameAndPasswordAsync(owner.UserId, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeOk();
                await replaceBookResponse.VerifySchema(FilePathConst.REPLACE_BOOK_SUCCESS_SCHEMA);
                replaceBookResponse.Data?.UserId.Should().Be(owner.UserId);
                replaceBookResponse.Data?.Username.Should().Be(owner.Username);
                replaceBookResponse.Data?.Books.Should().ContainEquivalentOf(new BookDto { Isbn = newBook.Isbn },
                    opts => opts.Including(x => x.Isbn));
                replaceBookResponse.Data?.Books.Should().NotContainEquivalentOf(new BookDto { Isbn = oldBook.Isbn },
                    opts => opts.Including(x => x.Isbn));
                getDetailResponse.Data?.Books.Should().AllSatisfy(x => x.Isbn.Should().NotBe(oldBook.Isbn));
                getDetailResponse.Data?.Books.Select(x=>x.Isbn).Contains(newBook.Isbn);
            }
        }

        /// <summary>
        /// User does not own that old book
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("replace"), Category("fail")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.ReplaceNotOwnOldBook))]
        public async Task ReplaceFromNotOwnOldBookFail(ReplaceBookData data)
        {
            //Make sure user does not own book
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            await BookService.DeleteBookWithUnameAndPassAsync(owner.UserId, oldBook.Isbn, logined.Username, logined.Password);

            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeBadRequest();
                replaceBookResponse.VerifyErrorPayload(ErrorConst.NOT_OWN_BOOK_CODE, ErrorConst.NOT_OWN_BOOK_MSG);
            }
        }
        /// <summary>
        /// Book ISBN not in the system
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("replace"), Category("fail")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.ReplaceOldBookNotFound))]
        public async Task ReplaceFromNotFoundOldBookFail(ReplaceBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeBadRequest();
                replaceBookResponse.VerifyErrorPayload(ErrorConst.NOT_OWN_BOOK_CODE, ErrorConst.NOT_OWN_BOOK_MSG);
            }
        }
        /// <summary>
        /// NewBook ISBN already own
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("replace"), Category("fail")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.ReplaceDuplicateNewBook))]
        public async Task ReplaceDuplicateNewBookFail(ReplaceBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            AddBookRequest addRequest = new AddBookRequest(owner.UserId, newBook.Isbn);
            //Make sure user already own new book
            await BookService.AddBookWithUnameAndPasswordAsync(addRequest, logined.Username, logined.Password);

            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeBadRequest();
                replaceBookResponse.VerifyErrorPayload(ErrorConst.NOT_OWN_BOOK_CODE, ErrorConst.NOT_OWN_BOOK_MSG);
            }
        }
        /// <summary>
        /// NewBook ISBN not in the system
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("replace"), Category("fail")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.ReplaceNewBookNotFound))]
        public async Task ReplaceToNewBookNotFoundFail(ReplaceBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeBadRequest();
                replaceBookResponse.VerifyErrorPayload(ErrorConst.NOT_FOUND_BOOK_CODE, ErrorConst.NOT_FOUND_BOOK_MSG);
            }
        }
        /// <summary>
        /// UserId not in the system
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("replace"), Category("fail")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.ReplaceBookNotFoundUser))]
        public async Task ReplaceBookUserNotFoundFail(ReplaceBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeUnauthorized();
                replaceBookResponse.VerifyErrorPayload(ErrorConst.NOT_FOUND_OR_WRONG_USER_ID_CODE, ErrorConst.WRONG_USER_ID_MSG);
            }
        }
        /// <summary>
        /// UserId does not match with logged in user
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("replace"), Category("fail")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.ReplaceBookDifferentUser))]
        public async Task ReplaceBookDifferentUserIdFail(ReplaceBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeUnauthorized();
                replaceBookResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
            }
        }
        /// <summary>
        /// User is not logged in
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("replace"), Category("fail")]
        [TestCaseSource(typeof(ReplaceBookDataProvider), nameof(ReplaceBookDataProvider.NoAuthen))]
        public async Task ReplaceBookNoAuthenFail(ReplaceBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto oldBook = BookDataProvider.LoadBookDataFile(data.OldBookKey);
            BookDto newBook = BookDataProvider.LoadBookDataFile(data.NewBookKey);
            //Make sure user does not own book
            await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);

            var replaceBookResponse = await BookService.ReplaceBookUsingUnameAndPassAsync(owner.UserId, oldBook.Isbn, newBook.Isbn, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                replaceBookResponse.VerifyStatusCodeUnauthorized();
                replaceBookResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
            }
        }
    }
}
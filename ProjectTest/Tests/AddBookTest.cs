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
    public class AddBookTest : BaseTest
    {
        [Test]
        [Category("add"), Category("success")]
        [TestCaseSource(typeof(AddBookDataProvider), nameof(AddBookDataProvider.AddValidBook))]
        public async Task AddBookSucccess(AddBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            //setup: delete to make sure the created book is unique, user does not have books
            //It is ok for delete api to fail
            BookService.DeleteAllBooksWithUnameAndPass(owner.UserId, logined.Username, logined.Password);

            //To delete the prep/duplicate, after test case 
            BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);

            //Begin test
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, book.Isbn);
            var addBookResponse = await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);
            // var addBookResponse = await BookService.AddBookWithTokenAsync(addBookdata, token);

            //Verify
            var getDetailResponse = await AccountService.GetDetailUserWithUnameAndPasswordAsync(owner.UserId, logined.Username, logined.Password);
            using (new AssertionScope())
            {
                addBookResponse.VerifyStatusCodeCreated();
                addBookResponse.Data.Should().NotBeNull();
                addBookResponse.Data.CollectionOfIsbns.Should().ContainEquivalentOf(new IsbnDto(book.Isbn));
                getDetailResponse.Data?.Books.Select(x=>x.Isbn).Contains(book.Isbn);
            }

        }
        /// <summary>
        /// Add book when user already own that book
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("add"), Category("fail")]
        [TestCaseSource(typeof(AddBookDataProvider), nameof(AddBookDataProvider.AddDuplicateBook))]
        public async Task AddDuplicateBookFail(AddBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, book.Isbn);
            //setup: add to make sure user has the book already
            //It is ok for add api to fail( duplicate)
            await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);

            //To delete the prep/duplicate, after test case 
            BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);

            //Begin test
            var addBookResponse = await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);

            //verify
            using (new AssertionScope())
            {
                addBookResponse.VerifyStatusCodeBadRequest();
                addBookResponse.Data.Should().NotBeNull();
                addBookResponse.VerifyErrorPayload(ErrorConst.ADD_DUP_BOOK_CODE, ErrorConst.ADD_DUP_BOOK_MSG);
            }
        }
        /// <summary>
        /// Add a not existing book
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("add"), Category("fail")]
        [TestCaseSource(typeof(AddBookDataProvider), nameof(AddBookDataProvider.AddBookNotFoundBook))]
        public async Task AddBookIsbnNotFoundFail(AddBookData data)
        {
            //Begin test
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, book.Isbn);
            var addBookResponse = await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);

            //To delete the prep/duplicate, after test case in case Book is created
            if (addBookResponse.StatusCode == HttpStatusCode.Created)
            {
                BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);
            }

            //Veriy
            using (new AssertionScope())
            {
                addBookResponse.VerifyStatusCodeBadRequest();
                addBookResponse.Data.Should().NotBeNull();
                addBookResponse.VerifyErrorPayload(ErrorConst.NOT_FOUND_BOOK_CODE, ErrorConst.NOT_FOUND_BOOK_MSG);
            }
        }
        /// <summary>
        /// UserId does not exist
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("add"), Category("fail")]
        [TestCaseSource(typeof(AddBookDataProvider), nameof(AddBookDataProvider.UserNotFound))]
        public async Task AddBookUserIdNotFoundFail(AddBookData data)
        {
            // string token = await GetToken(logined.Username, logined.Password);
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, book.Isbn);

            //Begin test
            var addBookResponse = await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);

            //To delete the prep/duplicate, after test case in case Book is created
            if (addBookResponse.StatusCode == HttpStatusCode.Created)
            {
                BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);
            }

            //veriy
            using (new AssertionScope())
            {
                addBookResponse.VerifyStatusCodeUnauthorized();
                addBookResponse.Data.Should().NotBeNull();
                addBookResponse.VerifyErrorPayload(ErrorConst.NOT_FOUND_OR_WRONG_USER_ID_CODE, ErrorConst.WRONG_USER_ID_MSG);
            }
        }
        /// <summary>
        /// UserId does not match with logged in user
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("add"), Category("fail")]
        [TestCaseSource(typeof(AddBookDataProvider), nameof(AddBookDataProvider.AddBookWrongUser))]
        public async Task AddBookDifferentUserFail(AddBookData data)
        {
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, book.Isbn);

            //Begin test
            var addBookResponse = await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);

            //To delete the prep/duplicate, after test case in case Book is created
            if (addBookResponse.StatusCode == HttpStatusCode.Created)
            {
                BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);
            }

            //Verify
            using (new AssertionScope())
            {
                addBookResponse.VerifyStatusCodeUnauthorized();
                addBookResponse.Data.Should().NotBeNull();
                addBookResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
            }
        }
        /// <summary>
        /// Add book without logging in
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Test]
        [Category("add"), Category("fail")]
        [TestCaseSource(typeof(AddBookDataProvider), nameof(AddBookDataProvider.NoAuthen))]
        public async Task AddBookNoAuthenFail(AddBookData data)
        {
            //Begin test
            AccountDto owner = AccountDataProvider.LoadAccountDataFile(data.OwnerAccountKey);
            AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);
            BookDto book = BookDataProvider.LoadBookDataFile(data.BookKey);
            AddBookRequest addBookdata = new AddBookRequest(owner.UserId, book.Isbn);
            var addBookResponse = await BookService.AddBookWithUnameAndPasswordAsync(addBookdata, logined.Username, logined.Password);

            //To delete the prep/duplicate, after test case in case Book is created
            if (addBookResponse.StatusCode == HttpStatusCode.Created)
            {
                BookService.StoreUserToDeleteBookLater(owner.UserId, logined.Username, logined.Password, this.GetType().Name);
            }

            //verify
            using (new AssertionScope())
            {
                addBookResponse.VerifyStatusCodeUnauthorized();
                addBookResponse.Data.Should().NotBeNull();
                addBookResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
            }
        }
    }
}
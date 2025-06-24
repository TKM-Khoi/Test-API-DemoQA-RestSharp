using FluentAssertions;
using FluentAssertions.Execution;

using ProjectTest.DataModels;

using Service.Const;
using Service.Extensions;
using Service.Models.DTOs;
using Service.Models.Resquests;

using Test.Core.Extensions;
using Test.DataProvider;

namespace Test.Tests;

public class GetUserDetailTest : BaseTest
{
    [Test]
    [Category("detail"), Category("success")]
    [TestCaseSource(typeof(GetAccountDetailDataProvider), nameof(GetAccountDetailDataProvider.GetDetailValid))]

    public async Task GetUserDetailSuccess(GetAccountDetailData data)
    {
        AccountDto accountWithUserId = AccountDataProvider.LoadAccountDataFile(data.AccountWithUserIdKey);
        AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);

        //Setup: Add books into account
        ICollection<BookDto> books = data.BookKeys.Select(key => BookDataProvider.LoadBookDataFile(key)).ToList();

        //Cannot use Add multiple Book api due to api bug (only add the last book isbn)
        //=>Have to add books 1 by 1
        foreach (BookDto book in books)
        {
            var addBookdto = new AddBookRequest(accountWithUserId.UserId, book.Isbn);
            await BookService.AddBookWithUnameAndPasswordAsync(addBookdto, logined.Username, logined.Password);
        }
        if (books.Count > 0)
        {
            BookService.StoreUserToDeleteBookLater(accountWithUserId.UserId, logined.Username, logined.Password, this.GetType().Name);
        }
        var getDetailResponse = await AccountService.GetDetailUserWithUnameAndPasswordAsync(accountWithUserId.UserId, logined.Username, logined.Password);
        using (new AssertionScope())
        {
            getDetailResponse.VerifyStatusCodeOk();
            getDetailResponse.Data?.UserId.Should().Be(accountWithUserId.UserId);
            getDetailResponse.Data?.Username.Should().Be(logined.Username);
            getDetailResponse.Data?.Books.Should().BeEquivalentTo(books);
        }
    }
    [Test]
    [Category("detail"), Category("fail")]
    [TestCaseSource(typeof(GetAccountDetailDataProvider), nameof(GetAccountDetailDataProvider.GetDetailUserNotFound))]

    public async Task GetUserDetailNotFoundUserFail(GetAccountDetailData data)
    {
        AccountDto accountWithUserId = AccountDataProvider.LoadAccountDataFile(data.AccountWithUserIdKey);
        AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);

        var getDetailResponse = await AccountService.GetDetailUserWithUnameAndPasswordAsync(accountWithUserId.UserId, logined.Username, logined.Password);
        using (new AssertionScope())
        {
            getDetailResponse.VerifyStatusCodeUnauthorized();
            getDetailResponse.VerifyErrorPayload(ErrorConst.NOT_FOUND_OR_WRONG_USER_ID_CODE, ErrorConst.NOT_FOUND_USER_ID_MSG);
        }
    }
    [Test]
    [Category("detail"), Category("fail")]
    [TestCaseSource(typeof(GetAccountDetailDataProvider), nameof(GetAccountDetailDataProvider.GetDetailDifferentUser))]

    public async Task GetUserDetailDifferentUserFail(GetAccountDetailData data)
    {
        AccountDto accountWithUserId = AccountDataProvider.LoadAccountDataFile(data.AccountWithUserIdKey);
        AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);

        var getDetailResponse = await AccountService.GetDetailUserWithUnameAndPasswordAsync(accountWithUserId.UserId, logined.Username, logined.Password);
        using (new AssertionScope())
        {
            getDetailResponse.VerifyStatusCodeUnauthorized();
            getDetailResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
        }
    }
     [Test]
    [Category("detail"), Category("fail")]
    [TestCaseSource(typeof(GetAccountDetailDataProvider), nameof(GetAccountDetailDataProvider.GetDetailNoAuthen))]

    public async Task GetUserDetailNoAuthen(GetAccountDetailData data)
    {
        AccountDto accountWithUserId = AccountDataProvider.LoadAccountDataFile(data.AccountWithUserIdKey);
        AccountDto logined = AccountDataProvider.LoadAccountDataFile(data.LoginAccountKey);

        var getDetailResponse = await AccountService.GetDetailUserWithUnameAndPasswordAsync(accountWithUserId.UserId, logined.Username, logined.Password);
        using (new AssertionScope())
        {
            getDetailResponse.VerifyStatusCodeUnauthorized();
            getDetailResponse.VerifyErrorPayload(ErrorConst.NOT_AUTHORIZED_CODE, ErrorConst.NOT_AUTHORIZED_MSG);
        }
    }
}
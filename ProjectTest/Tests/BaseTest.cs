using Core.Client;
using Core.Reports;
using Core.ShareData;
using Core.Utils;

using NUnit.Framework.Interfaces;

using Service.Services;

using Test.Core.Extensions;

namespace Test.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class BaseTest
    {
        protected AccountService AccountService;
        protected BookService BookService;
        protected ApiClient Client;
        public BaseTest()
        {
            Client = new ApiClient(ConfigurationUtils.GetConfigurationByKey("TestUrl"));
            BookService = new BookService(Client);
            AccountService = new AccountService(Client);
            if (ConfigurationUtils.GetConfigurationByKey("Report") == "true")
            {
                ExtentReportHelper.CreateFeature(TestContext.CurrentContext.Test.ClassName);
            }
        }
        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("BaseTest: SetUp for " + TestContext.CurrentContext.Test.MethodName);
            if (ConfigurationUtils.GetConfigurationByKey("Report") == "true")
            {
                ExtentReportHelper.CreateTestCase(TestContext.CurrentContext.Test.Name);
            }
        }
        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("BaseTest: TearDown for " + TestContext.CurrentContext.Test.MethodName);
            Console.WriteLine();
            BookService.DeleteCreatedBooksFromStorage(TestContext.CurrentContext.Test.ClassName);

            TestStatus status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace) ? "" : TestContext.CurrentContext.Result.StackTrace;
            if (ConfigurationUtils.GetConfigurationByKey("Report") == "true")
            {
                ExtentReportHelper.CreateTestResult(Enum.GetName(typeof(TestStatus), status), stackTrace, TestContext.CurrentContext.Test.ClassName, TestContext.CurrentContext.Test.Name);
            }
        }
        public async Task<string> GetToken(string username, string password)
        {
            if (DataStorage.GetData($"AccToken-{username}") is null)
            {
                var res = await AccountService.TryHardGenerateTokenAsync(username, password);
                res.VerifyStatusCodeOk();

                DataStorage.SetData($"AccToken-{username}", "Bearer " + res.Data?.Token);
            }
            return (string)DataStorage.GetData($"AccToken-{username}");
        }
    }
}
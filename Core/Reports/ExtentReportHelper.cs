using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace Core.Reports
{
    public class ExtentReportHelper
    {
        static ExtentReports ExtentManager;
        private static AsyncLocal<ExtentTest> Feature = new AsyncLocal<ExtentTest>();

        private static AsyncLocal<ExtentTest> TestCase = new AsyncLocal<ExtentTest>();
        public static void InitualizeReport(string[] reportPaths)
        {
            ExtentManager = new ExtentReports();
            foreach (string reportPath in reportPaths)
            {
                ExtentSparkReporter htmlReporter = new ExtentSparkReporter(reportPath);
                ExtentManager.AttachReporter(htmlReporter);
            }
            ExtentManager.AddSystemInfo("Enviroment", "Staging");
            Console.WriteLine("Initualize report");
        }
        public static void CreateFeature(string name)
        {
            Feature.Value = ExtentManager.CreateTest(name);
            Console.WriteLine("create test");
        }
        public static void CreateTestCase(string name)
        {
            TestCase.Value = Feature.Value.CreateNode(name);
            Console.WriteLine("create node");
        }
        public static void LogTestStep(string step)
        {
            TestCase.Value.Info(step);
        }
        public static void CreateTestResult(string status, string stackTrace, string className, string testName)
        {
            Status logStatus;
            switch (status)
            {
                case "Failed":
                    {
                        logStatus = Status.Fail;
                        TestCase.Value.Fail($"#Test Name: {testName}, #Status: {logStatus + stackTrace}");
                        break;
                    }
                case "Passed":
                    {
                        logStatus = Status.Pass;
                        TestCase.Value.Pass($"====> Test Name: {testName}, Status: {logStatus}");
                        break;
                    }
                case "Inconclusive":
                    {
                        logStatus = Status.Warning;
                        TestCase.Value.Pass($"====> Test Name: {testName}, Status: {logStatus}");
                        break;
                    }
                case "Skipped":
                    {
                        logStatus = Status.Skip;
                        TestCase.Value.Pass($"====> Test Name: {testName}, Status: {logStatus}");
                        break;
                    }
                default:
                    {
                        logStatus = Status.Pass;
                        TestCase.Value.Pass($"====> Test Name: {testName}, Status: {logStatus}");
                        break;
                    }
            }
        }
        public static void Flush()
        {
            Console.WriteLine("Before flush");
            ExtentManager.Flush();
            Console.WriteLine("After flush");
        }
    }
}
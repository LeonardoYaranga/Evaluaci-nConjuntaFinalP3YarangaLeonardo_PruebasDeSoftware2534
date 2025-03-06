using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;

namespace MoneyTrackerReqnroll.Reports
{
    public static class ExtentReportManager
    {
        private static ExtentReports _extent;
        private static ExtentTest _test;
        private static string _reportPath = Path.Combine(Directory.GetCurrentDirectory(), "TestResult", "ExtentReport.html");

        public static void InitReport()
        {
            var sparkReport = new ExtentSparkReporter(_reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReport);
        }

        public static void StartTest(string testName)
        {
            _test = _extent.CreateTest(testName);
        }

        public static void LogStep(bool success, string stepDetails)
        {
            if (success)
            {
                _test.Log(Status.Pass, stepDetails);
            }
            else
            {
                _test.Log(Status.Fail, stepDetails);
            }
        }

        // Nuevo método para logs informativos
        public static void LogInfo(string details)
        {
            _test.Log(Status.Info, details);
        }

        public static void FlushReport()
        {
            _extent.Flush();
        }
    }
}
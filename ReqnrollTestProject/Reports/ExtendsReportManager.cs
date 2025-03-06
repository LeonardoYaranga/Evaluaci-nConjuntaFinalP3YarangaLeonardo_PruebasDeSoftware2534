using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqnrollTestProject.Reports
{
    public class ExtendsReportManager
    {
        private static ExtentReports _extent;
        private static ExtentTest _test;
        private static string _reportpath =
            Path.Combine(Directory.GetCurrentDirectory(), "TestResult", "ExtentReport.html");

        public static void InitReport()
        {
            var sparkReport = new ExtentSparkReporter(_reportpath);
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReport);
        }

        public static void StartTest(string testname)
        {
            _test = _extent.CreateTest(testname);
        }

        public static void LogStep(bool Success, string stepDetails)
        {
            if (Success)
            {
                _test.Log(Status.Pass, stepDetails);
            }
            else
            {
                _test.Log(Status.Fail, stepDetails);

            }
        }

        public static void FlushReport()
        {
            _extent.Flush();
        }

    }
}



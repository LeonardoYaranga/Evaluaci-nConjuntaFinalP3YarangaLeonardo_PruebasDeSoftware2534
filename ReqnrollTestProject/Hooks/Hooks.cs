using Reqnroll;
using ReqnrollTestProject.Reports;

namespace ReqnrollTestProject.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ExtendsReportManager.InitReport();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            ExtendsReportManager.StartTest(ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext)
        {
            var stepInfo = scenarioContext.StepContext.StepInfo.Text;
            bool isSuccess = scenarioContext.TestError == null;
            ExtendsReportManager.LogStep(isSuccess, isSuccess ? $"Paso exitoso: {stepInfo}" : $"Error : {scenarioContext.TestError.Message}");
        }

    }

}
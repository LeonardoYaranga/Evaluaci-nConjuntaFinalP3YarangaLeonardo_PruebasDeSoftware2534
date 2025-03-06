using MoneyTrackerReqnroll.Reports;
using MoneyTrackerReqnroll.Utilities;
using Reqnroll;

namespace MoneyTrackerReqnroll.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private readonly ScenarioContext _scenarioContext;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ExtentReportManager.InitReport(); // Inicializa el reporte
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            ExtentReportManager.StartTest(_scenarioContext.ScenarioInfo.Title); // Inicia el test para el escenario
        }

        [AfterStep]
        public void AfterStep()
        {
            var stepInfo = _scenarioContext.StepContext.StepInfo.Text;
            bool isSuccess = _scenarioContext.TestError == null;
            ExtentReportManager.LogStep(isSuccess, isSuccess ? $"Paso exitoso: {stepInfo}" : $"Error: {_scenarioContext.TestError.Message}");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                WebDriverManager.QuitDriver();
            }
            catch (Exception ex)
            {
                ExtentReportManager.LogInfo($"Error al cerrar el driver: {ex.Message}");
            }
            finally
            {
                ExtentReportManager.FlushReport();
            }
        }
    }
}
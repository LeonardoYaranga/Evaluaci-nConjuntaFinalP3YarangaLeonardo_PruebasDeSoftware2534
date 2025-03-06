using System;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using MoneyTrackerReqnroll.Utilities;
using Reqnroll;
using OpenQA.Selenium;

namespace MoneyTrackerReqnroll.StepDefinitions
{
    [Binding]
    public class IngresoSistemaStepDefinitions
    {

        private IWebDriver _driver;
        private static ExtentReports _extent;
        private static ExtentTest _test;
        private readonly ScenarioContext _scenarioContext;
        public IngresoSistemaStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var sparkReporter = new ExtentSparkReporter("IngresoSistemaReport.html");
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverManager.GetDriver("edge");
            _test = _extent.CreateTest(_scenarioContext.ScenarioInfo.Title);
        }

        [Given("El usuario se encuentra en la página principal.")]
        public void GivenElUsuarioSeEncuentraEnLaPaginaPrincipal_()
        {
            _driver.Navigate().GoToUrl("http://localhost:3000");         
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _test.Log(Status.Pass, "El usuario navegó a la página principal");

        }

        [When("El usuario ingresa su correo y contraseña.")]
        public void WhenElUsuarioIngresaSuCorreoYContrasena_(DataTable dataTable)
        {
            var row = dataTable.Rows[0];
            _driver.FindElement(By.Id("email")).SendKeys(row["Correo"]);
            _driver.FindElement(By.Id("password")).SendKeys(row["Contraseña"]);
            _test.Log(Status.Info, $"Correo: {row["Correo"]}, Contraseña: {row["Contraseña"]} ingresados");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

        [When("El usuario hace clic en el botón {string}.")]
        public void WhenElUsuarioHaceClicEnElBoton_(string p0)
        {
            _driver.FindElement(By.XPath($"//button[text()='{p0}']")).Click();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _test.Log(Status.Info, $"Clic en el botón '{p0}' realizado");
        }

        [Then("El sistema muestra un mensaje de éxito {string}.")]
        public void ThenElSistemaMuestraUnMensajeDeExito_(string p0)
        {
            try
            {
                string actualMessage = _driver.FindElement(By.ClassName("text-green-500")).Text; // Ajusta si el mensaje de éxito usa otra clase
                if (actualMessage.Contains(p0))
                {
                    _test.Log(Status.Pass, $"Mensaje esperado encontrado: {actualMessage}");
                }
                else
                {
                    _test.Log(Status.Fail, $"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
                    throw new Exception($"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
                }
            }
            catch (NoSuchElementException)
            {
                _test.Log(Status.Fail, $"No se encontró el mensaje de éxito esperado: '{p0}'");
                throw;
            }
        }

        [Then("El usuario debería ser redirigido al apartado Home con la URL {string}.")]
        public void ThenElUsuarioDeberiaSerRedirigidoAlApartadoHomeConLaURL_(string p0)
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8);
            Thread.Sleep(5000);
            string actualUrl = _driver.Url;
            if (actualUrl == p0)
            {
                _test.Log(Status.Pass, $"Redirigido a la URL esperada: {actualUrl}");
            }
            else
            {
                _test.Log(Status.Fail, $"Esperado: '{p0}', pero se obtuvo: '{actualUrl}'");
                throw new Exception($"Esperado: '{p0}', pero se obtuvo: '{actualUrl}'");
            }
        }

        [Then("El campo de correo muestra una validación HTML {string}.")]
        public void ThenElCampoDeCorreoMuestraUnaValidacionHTML_(string p0)
        {
            var emailField = _driver.FindElement(By.Id("email"));
            string actualMessage = emailField.GetAttribute("validationMessage");
            if (actualMessage.Contains(p0))
            {
                _test.Log(Status.Pass, $"Validación HTML encontrada: {actualMessage}");
            }
            else
            {
                _test.Log(Status.Fail, $"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
                throw new Exception($"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
            }
        }

        [Then("El sistema muestra un mensaje de error {string}.")]
        public void ThenElSistemaMuestraUnMensajeDeError_(string p0)
        {
            try
            { //<div class="mt-4 text-red-500">Password is incorrect</div>
                string actualMessage = _driver.FindElement(By.ClassName("text-red-500")).Text; // Ajusta si el mensaje de error usa otra clase
                if (actualMessage.Contains(p0))
                {
                    _test.Log(Status.Pass, $"Mensaje esperado encontrado: {actualMessage}");
                }
                else
                {
                    _test.Log(Status.Fail, $"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
                    throw new Exception($"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
                }
            }
            catch (NoSuchElementException)
            {
                _test.Log(Status.Fail, $"No se encontró el mensaje de error esperado: '{p0}'");
                throw;
            }
        }

        [Then("El campo de contraseña muestra una validación HTML {string}.")]
        public void ThenElCampoDeContrasenaMuestraUnaValidacionHTML_(string p0)
        {
            var passwordField = _driver.FindElement(By.Id("password"));
            string actualMessage = passwordField.GetAttribute("validationMessage");
            if (actualMessage.Contains(p0))
            {
                _test.Log(Status.Pass, $"Validación HTML encontrada: {actualMessage}");
            }
            else
            {
                _test.Log(Status.Fail, $"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
                throw new Exception($"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driver.Quit();
            _extent.Flush();
        }
    }
}

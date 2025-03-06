using System;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using MoneyTrackerReqnroll.Utilities;
using Reqnroll;
using OpenQA.Selenium;

namespace MoneyTrackerReqnroll.StepDefinitions
{
    [Binding]
    public class RegistroUsuarioStepDefinitions
    {
        private IWebDriver _driver;
        private static ExtentReports _extent;
        private static ExtentTest _test;
        private readonly ScenarioContext _scenarioContext;

        public RegistroUsuarioStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var sparkReporter = new ExtentSparkReporter("RegistroUsuarioReport.html");
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverManager.GetDriver("edge");
            _test = _extent.CreateTest(_scenarioContext.ScenarioInfo.Title);
        }

        [Given("El usuario se encuentra en la página principal")]
        public void GivenElUsuarioSeEncuentraEnLaPaginaPrincipal()
        {
            _driver.Navigate().GoToUrl("http://localhost:3000");          
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _test.Log(Status.Pass, "El usuario navegó a la página principal");
        }

        [When("El usuario hace clic en el enlace {string}")]
        public void WhenElUsuarioHaceClicEnElEnlace(string linkText)
        {
            try
            {
                _driver.FindElement(By.LinkText(linkText)).Click();
                _test.Log(Status.Info, $"Clic en el enlace '{linkText}' realizado");
                Thread.Sleep(2000);
            }
            catch (NoSuchElementException)
            {
                _test.Log(Status.Fail, $"No se encontró el enlace '{linkText}'");
                throw;
            }
        }

        [When("El usuario llena el formulario con")]
        public void WhenElUsuarioLlenaElFormularioCon(Table table)
        {
            var row = table.Rows[0];
            _driver.FindElement(By.Id("name")).SendKeys(row["Nombre"]);
            _driver.FindElement(By.Id("lastname")).SendKeys(row["Apellido"]);
            _driver.FindElement(By.Id("email")).SendKeys(row["Correo"]);
            _driver.FindElement(By.Id("password")).SendKeys(row["Contraseña"]);
            _driver.FindElement(By.Id("passwordConfirm")).SendKeys(row["Confirmación"]);
            _test.Log(Status.Info, $"Formulario llenado con: {string.Join(", ", row)}");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

        [When("El usuario hace clic en el botón {string}")]
        public void WhenElUsuarioHaceClicEnElBoton(string buttonText)
        {
            _driver.FindElement(By.XPath($"//button[text()='{buttonText}']")).Click();
            _test.Log(Status.Info, $"Clic en el botón '{buttonText}' realizado");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            Thread.Sleep(2000);
            
        }

        [Then("El sistema muestra un mensaje de éxito {string}")]
        public void ThenElSistemaMuestraUnMensajeDeExito(string expectedMessage)
        {
            try
            {//<div class="mt-4 text-green-500 text-sm">User created successfully</div>
                string actualMessage = _driver.FindElement(By.ClassName("text-green-500")).Text;
                if (actualMessage.Contains(expectedMessage))
                {
                    _test.Log(Status.Pass, $"Mensaje esperado encontrado: {actualMessage}");
                }
                else
                {
                    _test.Log(Status.Fail, $"Mensaje esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
                    throw new Exception($"Mensaje esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
                }
            }
            catch (NoSuchElementException)
            {
                _test.Log(Status.Fail, $"No se encontró el mensaje de éxito esperado: '{expectedMessage}'");
                throw;
            }
        }

        [Then("El sistema muestra un mensaje de error {string}")]
        public void ThenElSistemaMuestraUnMensajeDeError(string expectedMessage)
        {
            try
            {
                string actualMessage = _driver.FindElement(By.ClassName("text-red-500")).Text; // Cambia a clase de error
                if (actualMessage.Contains(expectedMessage))
                {
                    _test.Log(Status.Pass, $"Mensaje esperado encontrado: {actualMessage}");
                }
                else
                {
                    _test.Log(Status.Fail, $"Mensaje esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
                    throw new Exception($"Mensaje esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
                }
            }
            catch (NoSuchElementException)
            {
                _test.Log(Status.Fail, $"No se encontró el mensaje de error esperado: '{expectedMessage}'");
                throw;
            }
        }

        [Then("El campo de correo muestra una validación HTML {string}")]
        public void ThenElCampoDeCorreoMuestraUnaValidacionHTML(string expectedMessage)
        {
            var emailField = _driver.FindElement(By.Id("email"));
            string actualMessage = emailField.GetAttribute("validationMessage");
            if (actualMessage.Contains(expectedMessage))
            {
                _test.Log(Status.Pass, $"Validación HTML encontrada: {actualMessage}");
            }
            else
            {
                _test.Log(Status.Fail, $"Esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
                throw new Exception($"Esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
            }
        }

        [Then("El campo de nombre muestra una validación HTML {string}")]
        public void ThenElCampoDeNombreMuestraUnaValidacionHTML(string expectedMessage)
        {
            var nameField = _driver.FindElement(By.Id("name"));
            string actualMessage = nameField.GetAttribute("validationMessage");
            if (actualMessage.Contains(expectedMessage))
            {
                _test.Log(Status.Pass, $"Validación HTML encontrada: {actualMessage}");
            }
            else
            {
                _test.Log(Status.Fail, $"Esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
                throw new Exception($"Esperado: '{expectedMessage}', pero se obtuvo: '{actualMessage}'");
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
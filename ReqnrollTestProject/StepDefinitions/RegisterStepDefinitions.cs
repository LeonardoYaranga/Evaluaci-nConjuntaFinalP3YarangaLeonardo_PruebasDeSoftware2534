//using System;
//using AventStack.ExtentReports;
//using AventStack.ExtentReports.Reporter;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;
//using Reqnroll;
//using ReqnrollTestProject.Utilities;

//namespace ReqnrollTestProject.StepDefinitions
//{
//    [Binding]
//    public class RegisterStepDefinitions
//    {
//        private IWebDriver _driver;
//        private static ExtentReports _extent;
//        private static ExtentTest _test;
//        private readonly ScenarioContext _scenarioContext;


//        public RegisterStepDefinitions(ScenarioContext scenarioContext)
//        {
//            _scenarioContext = scenarioContext;
//        }
//        [BeforeTestRun]
//        public static void BeforeTestRun()
//        {
//            var sparkReporter = new ExtentSparkReporter("RegisterReport.html");
//            _extent = new ExtentReports();
//            _extent.AttachReporter(sparkReporter);

//        }
//        [BeforeScenario]
//        public void BeforeScenario()
//        {
//            _driver = WebDriverManager.GetDriver("firefox");
//            _test = _extent.CreateTest(_scenarioContext.ScenarioInfo.Title);
//        }
//        [Given("El usuario se encuentra en la pagina de inicio.")]
//        public void GivenElUsuarioSeEncuentraEnLaPaginaDeInicio_()
//        {
//            _driver.Navigate().GoToUrl("https://automationexercise.com/login");
//            _test.Log(Status.Pass, "Se ha ingresado a la página de inicio");
//        }

//        [When("El usuario ingresa un nombre {string} y  un correo {string}")]
//        public void WhenElUsuarioIngresaUnNombreYUnCorreo(string leo, string p1)
//        {
//            _driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div[3]/div/form/input[2]")).SendKeys(leo);
//            _driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div[3]/div/form/input[3]")).SendKeys(p1);

//            _test.Log(Status.Info, $"Usuario con nombre {leo} y correo {p1}");
//        }

//        [When("El usuario hace clic en el botón de Signup")]
//        public void WhenElUsuarioHaceClicEnElBotonDeSignup()
//        {
            
//            _driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div[3]/div/form/button")).Click();
//            _test.Log(Status.Info, "Usuario ha hecho click en el botón de Signup");
//        }

//        [When("Completa el formulario de registro con los datos requeridos")]
//        public void WhenCompletaElFormularioDeRegistroConLosDatosRequeridos(DataTable dataTable)
//        {
//            var data = dataTable.Rows[0]; // Obtiene la primera fila de datos

//            _driver.FindElement(By.Id("password")).SendKeys(data["password"]);
//            _driver.FindElement(By.Id("days")).SendKeys(data["dia"]);
//            _driver.FindElement(By.Id("months")).SendKeys(data["mes"]);
//            _driver.FindElement(By.Id("years")).SendKeys(data["anio"]);
//            _driver.FindElement(By.Id("first_name")).SendKeys(data["nombre"]);
//            _driver.FindElement(By.Id("last_name")).SendKeys(data["apellido"]);
//            _driver.FindElement(By.Id("company")).SendKeys(data["empresa"]);
//            _driver.FindElement(By.Id("address1")).SendKeys(data["direccion"]);

//            var countryDropdown = _driver.FindElement(By.Id("country"));
//            new SelectElement(countryDropdown).SelectByText(data["pais"]);

//            _driver.FindElement(By.Id("state")).SendKeys(data["estado"]);
//            _driver.FindElement(By.Id("city")).SendKeys(data["ciudad"]);
//            _driver.FindElement(By.Id("zipcode")).SendKeys(data["zip"]);
//            _driver.FindElement(By.Id("mobile_number")).SendKeys(data["movil"]);

//            _test.Log(Status.Info, "Usuario completó el formulario con los datos del feature file.");
//        }

//        [When("Hace clic en el botón de Create Account")]
//        public void WhenHaceClicEnElBotonDeCreateAccount()
//        {

//            _driver.FindElement(By.CssSelector("button[data-qa='create-account']")).Click();
//            _test.Log(Status.Info, "Usuario hizo clic en el botón de Create Account");
//        }

//        [Then("El usuario debería ver un mensaje de confirmación Account Created!")]
//        public void ThenElUsuarioDeberiaVerUnMensajeDeConfirmacionAccountCreated()
//        {
//           //Deberia encontrarse el mensaje de erro
//           //< h2 class="title text-center" data-qa="account-created" style="color: green;"><b>Account Created!</b></h2>
//           try
//            {
//                var accountCreated = _driver.FindElement(By.CssSelector("h2[data-qa='account-created']"));
//                _test.Log(Status.Pass, "Usuario ha sido redirigido a la página de cuenta creada");
//            }
//            catch (NoSuchElementException)
//            {
//                _test.Log(Status.Fail, "No se mostro el mensaje de creacion esperado");
//            }

//        }

//        [AfterScenario]
//        public void Down()
//        {
//            _driver.Quit();
//            _extent.Flush();
//        }
//    }
//}

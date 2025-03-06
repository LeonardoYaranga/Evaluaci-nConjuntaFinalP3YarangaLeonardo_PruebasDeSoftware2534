//using System;
//using AventStack.ExtentReports;
//using AventStack.ExtentReports.Reporter;
//using OpenQA.Selenium;
//using Reqnroll;
//using ReqnrollTestProject.Utilities;

//namespace ReqnrollTestProject.StepDefinitions
//{
//    [Binding]
//    public class LoginStepDefinitions
//    {

//        private IWebDriver _driver;
//        private static ExtentReports _extent;
//        private static ExtentTest _test;
//        private readonly ScenarioContext _scenarioContext;

//        public LoginStepDefinitions(ScenarioContext scenarioContext)
//        {
//            _scenarioContext = scenarioContext;
//        }
//        [BeforeTestRun]
//        public static void BeforeTestRun()
//        {
//           var sparkReporter = new ExtentSparkReporter("LoginReport.html");
//            _extent = new ExtentReports();
//            _extent.AttachReporter(sparkReporter);

//        }
//        [BeforeScenario]
//        public void BeforeScenario()
//        {
//            _driver = WebDriverManager.GetDriver("firefox");
//            _test = _extent.CreateTest(_scenarioContext.ScenarioInfo.Title);
//        }

//        [Given("El usuario se encuentra en la página de inicio")]
//        public void GivenElUsuarioSeEncuentraEnLaPaginaDeInicio()
//        {
//            _driver.Navigate().GoToUrl("https://automationexercise.com/login"); //https://www.automationexercise.com/login
//            _test.Log(Status.Pass, "Se ha ingresado a la página de inicio");
//        }

//        [When("El usuario ingresa su nombre de usuario {string} y contraseña {string}")]
//        public void WhenElUsuarioIngresaSuNombreDeUsuarioYContrasena(string p0, string p1)
//        {
//            _driver.FindElement(By.Name("email")).SendKeys(p0);
//            _driver.FindElement(By.Name("password")).SendKeys(p1);

//            _test.Log(Status.Info, $"Usuario con correo {p0} y contraseña {p1}");
//        }

//        [When("Hacer clikc en el boton de Login")]
//        public void WhenHacerClikcEnElBotonDeLogin()
//        {

//            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
//            _test.Log(Status.Info, "Usuario ha hecho click en el botón de login");
//        }

//        [Then("El usuario debería ser dirigido a la página de inicio")]
//        public void ThenElUsuarioDeberiaSerDirigidoALaPaginaDeInicio()
//        {
//            try
//            {
//                bool isErrorVisible = _driver.FindElement(By.ClassName("Login-error")) is null;
//                _test.Log(Status.Pass, "Usuario ha sido dirigido a la página de inicio");
//            }
//            catch (NoSuchElementException)
//            {
//                _test.Log(Status.Fail, "No se mostro el mensaje de error esperado");
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

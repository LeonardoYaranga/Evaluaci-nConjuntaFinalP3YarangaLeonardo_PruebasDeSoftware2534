using System;
using AventStack.ExtentReports;
using MoneyTrackerReqnroll.Reports;
using MoneyTrackerReqnroll.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;

namespace MoneyTrackerReqnroll.StepDefinitions
{
    [Binding]
    public class GestionNotificacionesStepDefinitions
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;

        // XPaths para el formulario de pago
        private const string NombreInput = "//input[@placeholder='De un nombre descriptivo a su ingreso/egreso']";
        private const string MontoInput = "//input[@placeholder='Ingrese su monto en números']";
        private const string TipoActividadInput = "//select[@id='multiSelect']/following-sibling::div//button";
        private const string DescripcionTextArea = "//textarea[@placeholder='Ingrese una Descripción para controlar sus acciones financieras']";
        private const string EstadoInput = "//select[@id='selectStatus']/following-sibling::div//button";
        private const string FechaLimiteInput = "//input[@type='date']";
        private const string BotonGuardar = "//button[@type='submit' and text()='Guardar']";

        // XPath para la campana de notificaciones
        private const string CampanaNotificaciones = "//svg[@width='18' and @height='18' and contains(@viewBox, '0 0 18 18')]";

        public GestionNotificacionesStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = WebDriverManager.GetDriver("edge");
        }

        [Given("El usuario está en la página principal después de iniciar sesión")]
        public void GivenElUsuarioEstaEnLaPaginaPrincipalDespuesDeIniciarSesion()
        {
            _driver.Navigate().GoToUrl("http://localhost:3000/");
            ExtentReportManager.LogInfo("Navegado a la página de login");

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var emailField = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email")));
            emailField.Clear();
            emailField.SendKeys("jppinza@espe.edu.ec");
            ExtentReportManager.LogInfo("Correo ingresado: jppinza@espe.edu.ec");

            var passwordField = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("password")));
            passwordField.Clear();
            passwordField.SendKeys("Jppa2004");
            ExtentReportManager.LogInfo("Contraseña ingresada");

            var loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='Iniciar Sesión']")));
            loginButton.Click();
            ExtentReportManager.LogInfo("Clic en el botón de login realizado");

            wait.Until(drv => drv.Url.Contains("/main/home"));
            ExtentReportManager.LogInfo("Login exitoso, URL actual: " + _driver.Url);
        }

        [When("El usuario llena el formulario de pago con datos válidos y fecha límite del día siguiente")]
        public void WhenElUsuarioLlenaElFormularioConDatosValidosYFechaLimite(Table table)
        {
            var row = table.Rows[0];
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Asumimos que estamos en /main/payments o similar para crear el pago
            _driver.Navigate().GoToUrl("http://localhost:3000/main/payments"); // Ajusta según tu app
            ExtentReportManager.LogInfo("Navegado a la página de creación de pagos");

            var nombreElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(NombreInput)));
            nombreElement.Clear();
            nombreElement.SendKeys(row["Nombre"]);
            ExtentReportManager.LogInfo($"Nombre ingresado: {row["Nombre"]}");

            var montoElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(MontoInput)));
            montoElement.Clear();
            montoElement.SendKeys(row["Monto"]);
            ExtentReportManager.LogInfo($"Monto ingresado: {row["Monto"]}");

            var tipoActividadButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(TipoActividadInput)));
            tipoActividadButton.Click();
            var tipoOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//select[@id='multiSelect']/following-sibling::div//div[contains(@class, 'cursor-pointer') and .//div[text()='{row["TipoActividad"]}']]")));
            tipoOption.Click();
            ExtentReportManager.LogInfo($"Tipo de Actividad seleccionado: {row["TipoActividad"]}");

            var descripcionElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(DescripcionTextArea)));
            descripcionElement.Clear();
            descripcionElement.SendKeys(row["Descripción"]);
            ExtentReportManager.LogInfo($"Descripción ingresada: {row["Descripción"]}");

            var fechaLimiteElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(FechaLimiteInput)));
            fechaLimiteElement.Clear();
            fechaLimiteElement.SendKeys(row["FechaLimite"]); // Formato MM/DD/YYYY
            ExtentReportManager.LogInfo($"Fecha límite ingresada: {row["FechaLimite"]}");

            var estadoButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(EstadoInput)));
            estadoButton.Click();
            var estadoOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//select[@id='selectStatus']/following-sibling::div//div[contains(@class, 'cursor-pointer') and .//div[text()='{row["Estado"]}']]")));
            estadoOption.Click();
            ExtentReportManager.LogInfo($"Estado seleccionado: {row["Estado"]}");
        }


        [When("El usuario hace clic en el botón {string} en pago")]
        public void WhenElUsuarioHaceClicEnElBotonEnPago(string guardar)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var guardarButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(BotonGuardar)));
            guardarButton.Click();
            ExtentReportManager.LogInfo("Clic en el botón Guardar realizado");
        }


        [When("El usuario abre la campana de notificaciones al día siguiente")]
        public void WhenElUsuarioAbreLaCampanaDeNotificacionesAlDiaSiguiente()
        {
            // Simulamos "al día siguiente" asumiendo que la fecha límite ya está seteada
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var campanaButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(CampanaNotificaciones)));
            campanaButton.Click();
            ExtentReportManager.LogInfo("Campana de notificaciones abierta");
        }

        [Then("El sistema debe mostrar una notificación de deuda pendiente con el mensaje {string}")]
        public void ThenElSistemaDebeMostrarUnaNotificacionDeDeudaPendiente(string expectedMessage)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var notification = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[contains(@class, 'flex flex-col') and .//p[text()='{expectedMessage}']]")));

            if (notification.Displayed)
            {
                ExtentReportManager.LogStep(true, $"Notificación encontrada: '{expectedMessage}'");
            }
            else
            {
                ExtentReportManager.LogStep(false, "No se encontró la notificación esperada");
                throw new Exception($"No se encontró la notificación con el mensaje: '{expectedMessage}'");
            }
        }

        [When("El usuario abre la campana de notificaciones")]
        public void WhenElUsuarioAbreLaCampanaDeNotificaciones()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var campanaButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(CampanaNotificaciones)));
            campanaButton.Click();
            ExtentReportManager.LogInfo("Campana de notificaciones abierta");
        }

        [Then("El sistema debe mostrar un mensaje de que no hay notificaciones pendientes si las fechas de pago ya pasaron")]
        public void ThenElSistemaDebeMostrarMensajeDeNoNotificacionesPendientes()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var notifications = _driver.FindElements(By.XPath("//div[contains(@class, 'flex flex-col') and contains(@class, 'border-t')]"));

            if (notifications.Count == 0)
            {
                ExtentReportManager.LogStep(true, "No hay notificaciones pendientes, como se esperaba");
            }
            else
            {
                ExtentReportManager.LogStep(false, "Se encontraron notificaciones cuando no debería haberlas");
                throw new Exception("Se encontraron notificaciones inesperadas cuando las fechas de pago ya pasaron");
            }
        }
    }
}
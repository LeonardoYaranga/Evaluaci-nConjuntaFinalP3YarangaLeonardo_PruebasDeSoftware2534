
using System;
using AventStack.ExtentReports;
using MoneyTrackerReqnroll.Reports;
using MoneyTrackerReqnroll.Utilities;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Reqnroll;
using SeleniumExtras.WaitHelpers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit;

namespace MoneyTrackerReqnroll.StepDefinitions
{
    [Binding]

    public class GestionDePagosStepDefinitions
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        // XPaths para los campos de la página de gestión de pagos

        private const string NombreInput = "//input[@placeholder='De un nombre descriptivo a su ingreso/egreso']";
        private const string MontoInput = "//input[@placeholder='Ingrese su monto en números']";
        private const string TipoActividadInput = "//input[@placeholder='Seleccione una opción' and preceding-sibling::select[@id='multiSelect']]";
        private const string DescripcionTextArea = "//textarea[@placeholder='Ingrese una Descripción para controlar sus acciones financieras']";
        private const string EstadoInput = "//input[@placeholder='Seleccione una opción' and preceding-sibling::select[@id='selectStatus']]";
        
        private const string BotonGuardar = "//button[@type='submit' and text()='Guardar']";

        // XPaths para edición y eliminación
        private const string BtnEditar = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/tr[1]/td[6]/div/div/button[2]";
        private const string BtnBorrar = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/tr[1]/td[6]/div/div/button[1]";

        // XPaths para el modal de edición
        private const string ModalNombreInput = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/div/div/div/form/div/div[1]/div[2]/div[1]/input";
        private const string ModalMontoInput = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/div/div/div/form/div/div[1]/div[2]/div[2]/input";
        private const string ModalTipoActividadInput = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/div/div/div/form/div/div[1]/div[2]/div[3]/div/div/div/div/div/div[1]/div/div[1]/div/input";
        private const string ModalDescripcionTextArea = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/div/div/div/form/div/div[2]/div[2]/div[1]/textarea";
        private const string ModalEstadoInput = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/div/div/div/form/div/div[2]/div[2]/div[2]/div/div/div/div/div/div[1]/div/div[1]/div/input";
        private const string ModalBtnGuardar = "//*[@id='__next']/div/div/div[2]/main/div/div[2]/div/div/div/table/tbody/div/div/div/form/div/div[2]/div[2]/div[3]/button[2]";

        public GestionDePagosStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            // Se utiliza Edge según lo indicado
            _driver = WebDriverManager.GetDriver("edge");
        }

        // --- Paso común de Login ---
        [Given("Estar en la página principal de la aplicación")]
        public void DadoEstarEnLaPaginaPrincipalDeLaAplicacion()
        {
            _driver.Navigate().GoToUrl("http://localhost:3000/");
            ExtentReportManager.LogInfo("Navegado a la página de login");

            // Ingresar correo
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var emailField = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email")));
            emailField.Clear();
            emailField.SendKeys("jppinza@espe.edu.ec");
            ExtentReportManager.LogInfo("Correo ingresado: jppinza@espe.edu.ec");

            // Ingresar contraseña
            var passwordField = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("password")));
            passwordField.Clear();
            passwordField.SendKeys("Jppa2004");
            ExtentReportManager.LogInfo("Contraseña ingresada");

            // Clic en el botón de login con un selector más robusto
            var loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='Iniciar Sesión']")));
            loginButton.Click();
            ExtentReportManager.LogInfo("Clic en el botón de login realizado");

            // Esperar redirección a la página principal
            wait.Until(drv => drv.Url.Contains("/main/home"));
            ExtentReportManager.LogInfo("Login exitoso, URL actual: " + _driver.Url);
        }


        // --- Escenario GP_001: Crear pago con datos correctos ---
        [When("El usuario llena el apartado del formulario con los datos")]
        public void CuandoElUsuarioLlenaElFormularioConDatos(Table table)
        {
            var row = table.Rows[0];
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Campo Nombre
            var nombreElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='De un nombre descriptivo a su ingreso/egreso']")));
            nombreElement.Clear();
            nombreElement.SendKeys(row["Nombre"]);
            ExtentReportManager.LogInfo($"Nombre ingresado: {row["Nombre"]}");

            // Campo Monto
            var montoElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Ingrese su monto en números']")));
            montoElement.Clear();
            montoElement.SendKeys(row["Monto"]);
            ExtentReportManager.LogInfo($"Monto ingresado: {row["Monto"]}");

            // Campo Tipo de Actividad (dropdown)
            var tipoActividadButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//select[@id='multiSelect']/following-sibling::div//button")));
            tipoActividadButton.Click(); // Abre el dropdown
            var tipoOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//select[@id='multiSelect']/following-sibling::div//div[contains(@class, 'cursor-pointer') and .//div[text()='{row["TipoActividad"]}']]")));
            tipoOption.Click();
            ExtentReportManager.LogInfo($"Tipo de Actividad seleccionado: {row["TipoActividad"]}");

            // Campo Descripción
            var descripcionElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//textarea[@placeholder='Ingrese una Descripción para controlar sus acciones financieras']")));
            descripcionElement.Clear();
            descripcionElement.SendKeys(row["Descripción"]);
            ExtentReportManager.LogInfo($"Descripción ingresada: {row["Descripción"]}");

            // Campo Estado (dropdown)
            var estadoButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//select[@id='selectStatus']/following-sibling::div//button")));
            estadoButton.Click(); // Abre el dropdown
            var estadoOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//select[@id='selectStatus']/following-sibling::div//div[contains(@class, 'cursor-pointer') and .//div[text()='{row["Estado"]}']]")));
            estadoOption.Click();
            ExtentReportManager.LogInfo($"Estado seleccionado: {row["Estado"]}");

            ExtentReportManager.LogInfo("Formulario de pago llenado con datos correctos");
        }

        [When("El usuario hace clic en el botón Guardar")]
        public void WhenElUsuarioHaceClicEnElBotoNGuardar()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            // Hacer clic fuera para cerrar cualquier dropdown abierto
            var form = wait.Until(ExpectedConditions.ElementToBeClickable(By.TagName("form")));
            form.Click();
            ExtentReportManager.LogInfo("Clic fuera realizado para cerrar dropdowns");

            var guardarButton = _driver.FindElement(By.XPath(BotonGuardar));
            // Si está deshabilitado, registra y falla
            if (!guardarButton.Enabled)
            {
                ExtentReportManager.LogStep(false, "El botón Guardar está deshabilitado");
                throw new Exception("El botón Guardar está deshabilitado");
            }

            guardarButton.Click();
            ExtentReportManager.LogInfo("Clic en el botón Guardar realizado");
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }


        [Then("El registro se hizo correctamente y se muestra un mensaje de éxito")]
        public void EntoncesRegistroExitosoConMensaje()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var successMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".alert.text-green-500")));

            if (successMessage.Displayed)
            {
                string messageText = successMessage.Text;
                ExtentReportManager.LogStep(true, $"Mensaje de éxito mostrado correctamente: {messageText}");
            }
            else
            {
                ExtentReportManager.LogStep(false, "No se mostró mensaje de éxito");
                throw new Exception("Error al crear el pago: mensaje de éxito no encontrado");
            }
        }

        // --- Escenario GP_002: Crear pago con campos vacíos ---
        [When("El usuario llena el apartado del formulario con datos vacios")]
        public void CuandoElUsuarioLlenaElFormularioConDatosVacios(Table table)
        {
            var row = table.Rows[0];
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Campo Nombre
            var nombreElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(NombreInput)));
            nombreElement.Clear();
            if (!string.IsNullOrEmpty(row["Nombre"]))
            {
                nombreElement.SendKeys(row["Nombre"]);
            }
            ExtentReportManager.LogInfo($"Nombre ingresado: '{row["Nombre"]}'");

            // Campo Monto
            var montoElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(MontoInput)));
            montoElement.Clear();
            if (!string.IsNullOrEmpty(row["Monto"]))
            {
                montoElement.SendKeys(row["Monto"]);
            }
            ExtentReportManager.LogInfo($"Monto ingresado: '{row["Monto"]}'");

            // Campo Tipo de Actividad (dropdown, no seleccionamos si está vacío)
            if (!string.IsNullOrEmpty(row["TipoActividad"]))
            {
                var tipoActividadButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//select[@id='multiSelect']/following-sibling::div//button")));
                tipoActividadButton.Click();
                var tipoOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//select[@id='multiSelect']/following-sibling::div//div[contains(@class, 'cursor-pointer') and .//div[text()='{row["TipoActividad"]}']]")));
                tipoOption.Click();
                ExtentReportManager.LogInfo($"Tipo de Actividad seleccionado: '{row["TipoActividad"]}'");
            }
            else
            {
                ExtentReportManager.LogInfo("Tipo de Actividad dejado vacío");
            }

            // Campo Descripción
            var descripcionElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(DescripcionTextArea)));
            descripcionElement.Clear();
            if (!string.IsNullOrEmpty(row["Descripción"]))
            {
                descripcionElement.SendKeys(row["Descripción"]);
            }
            ExtentReportManager.LogInfo($"Descripción ingresada: '{row["Descripción"]}'");

            // Campo Estado (dropdown, no seleccionamos si está vacío)
            if (!string.IsNullOrEmpty(row["Estado"]))
            {
                var estadoButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//select[@id='selectStatus']/following-sibling::div//button")));
                estadoButton.Click();
                var estadoOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//select[@id='selectStatus']/following-sibling::div//div[contains(@class, 'cursor-pointer') and .//div[text()='{row["Estado"]}']]")));
                estadoOption.Click();
                ExtentReportManager.LogInfo($"Estado seleccionado: '{row["Estado"]}'");
            }
            else
            {
                ExtentReportManager.LogInfo("Estado dejado vacío");
            }

            ExtentReportManager.LogInfo("Formulario de pago llenado con datos vacíos");
        }

        [Then("El sistema no debería habilitar el botón del Guardar el pago")]
        public void EntoncesBotonGuardarDeshabilitado()
        {
            var guardarButton = _driver.FindElement(By.XPath(BotonGuardar));
            if (!guardarButton.Enabled)
            {
                ExtentReportManager.LogStep(true, "El botón Guardar está deshabilitado como se esperaba");
            }
            else
            {
                ExtentReportManager.LogStep(false, "El botón Guardar está habilitado cuando debería estar deshabilitado");
                throw new Exception("El botón Guardar está habilitado pese a campos vacíos");
            }
        }


        // --- Escenario GP_003: Crear pago con monto negativo ---
        [When("El usuario llena el apartado del formulario con los datos para monto negativo")]
        public void CuandoElUsuarioLlenaElFormularioConDatosMontoNegativo(Table table)
        {
            var row = table.Rows[0];

            _driver.FindElement(By.XPath(NombreInput)).Clear();
            _driver.FindElement(By.XPath(NombreInput)).SendKeys(row["Nombre"]);

            _driver.FindElement(By.XPath(MontoInput)).Clear();
            _driver.FindElement(By.XPath(MontoInput)).SendKeys(row["Monto"]);

            _driver.FindElement(By.XPath(TipoActividadInput)).Clear();
            _driver.FindElement(By.XPath(TipoActividadInput)).SendKeys(row["TipoActividad"]);

            _driver.FindElement(By.XPath(DescripcionTextArea)).Clear();
            _driver.FindElement(By.XPath(DescripcionTextArea)).SendKeys(row["Descripción"]);

            _driver.FindElement(By.XPath(EstadoInput)).Clear();
            _driver.FindElement(By.XPath(EstadoInput)).SendKeys(row["Estado"]);

            ExtentReportManager.LogInfo("Formulario de pago llenado con monto negativo");
        }

        [Then("El sistema deberá mostrar una alerta de que el monto no puede ser negativo")]
        public void EntoncesAlertaMontoNegativo()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
                var alert = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".text-red-500")));
                string alertText = alert.Text.Trim();

                ExtentReportManager.LogInfo($"Mensaje de alerta encontrado: '{alertText}'");
                if (alertText.Contains("El monto debe ser un número positivo"))
                {
                    ExtentReportManager.LogStep(true, "Alerta por monto negativo mostrada correctamente");
                }
                else
                {
                    ExtentReportManager.LogStep(false, $"La alerta mostrada no es la esperada. Texto encontrado: '{alertText}'");
                    throw new Exception($"Alerta inesperada para monto negativo. Esperado: 'El monto debe ser un número positivo', Obtenido: '{alertText}'");
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                ExtentReportManager.LogStep(false, "No se mostró alerta por monto negativo dentro del tiempo de espera");
                throw new Exception("No se mostró alerta por monto negativo dentro de 20 segundos", ex);
            }
        }

        // --- Escenario GP_004: Crear pago con nombre de más de 250 caracteres ---
        [When("El usuario llena el apartado nombre con más de 250 caracteres {string}")]
        public void CuandoElUsuarioLlenaElNombreConMasDe250Caracteres(string longName)
        {
            var nombreElement = _driver.FindElement(By.XPath(NombreInput));
            nombreElement.Clear();
            nombreElement.SendKeys(longName);
            ExtentReportManager.LogInfo("Nombre con más de 250 caracteres ingresado");
        }

        [Then("El sistema deberá mostrar una alerta que indique que el nombre no debe superar un limite de caracteres")]
        public void EntoncesAlertaNombreExcedeLimite()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
                var alert = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".text-red-500")));
                string alertText = alert.Text.Trim();

                ExtentReportManager.LogInfo($"Mensaje de alerta encontrado: '{alertText}'");
                if (alertText.Contains("El nombre debe tener entre 3 y 100 caracteres"))
                {
                    ExtentReportManager.LogStep(true, "Alerta por longitud excesiva del nombre mostrada correctamente");
                }
                else
                {
                    ExtentReportManager.LogStep(false, $"La alerta mostrada no es la esperada. Texto encontrado: '{alertText}'");
                    throw new Exception($"Alerta inesperada para nombre muy largo. Esperado: 'El nombre debe tener entre 3 y 100 caracteres', Obtenido: '{alertText}'");
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                ExtentReportManager.LogStep(false, "No se mostró alerta por nombre que supera el límite de caracteres dentro del tiempo de espera");
                throw new Exception("No se mostró alerta por nombre demasiado largo dentro de 20 segundos", ex);
            }
        }

        // --- Escenario GP_005: Editar pago con datos correctos ---


        [Given("Dar click en el lapiz para editar")]
        public void GivenDarClickEnElLapizParaEditar()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Log de la URL actual para depurar
            ExtentReportManager.LogInfo($"URL actual: {_driver.Url}");

            // Espera que la tabla tenga al menos una fila
            var firstRow = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//table[@class='w-full table-auto']/tbody/tr[1]")));
            ExtentReportManager.LogInfo("Primera fila de la tabla localizada");

            // Busca el botón de edición dentro de la primera fila usando el id
            var editarButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//table[@class='w-full table-auto']/tbody/tr[1]//button[@id='editButton']")));
            editarButton.Click();
            ExtentReportManager.LogInfo("Clic en el ícono de edición realizado");
        }
        [When("El usuario modifica el dato nombre con {string}")]
        public void CuandoElUsuarioModificaElDatoNombreCon(string nuevoNombre)
        {
            var modalNombre = _driver.FindElement(By.XPath(ModalNombreInput));
            modalNombre.Clear();
            modalNombre.SendKeys(nuevoNombre);
            ExtentReportManager.LogInfo($"Nombre modificado a: {nuevoNombre}");
        }

        [When("Da click en el botón \"Guardar\"")]
        public void CuandoDaClickEnElBotonGuardarModal()
        {
            var modalGuardarButton = _driver.FindElement(By.XPath(ModalBtnGuardar));
            modalGuardarButton.Click();
            ExtentReportManager.LogInfo("Clic en el botón Guardar del modal realizado");
        }

        [Then("El sistema deberá mostrar un mensaje de éxito")]
        public void EntoncesMensajeExitoEdicion()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5)); // 5 segundos para capturar mensaje efímero
            var successMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".alert.text-green-500")));

            string messageText = successMessage.Text.Trim();
            ExtentReportManager.LogInfo($"Mensaje de éxito encontrado: '{messageText}'");

            if (successMessage.Displayed)
            {
                ExtentReportManager.LogStep(true, "Mensaje de éxito mostrado tras edición");
            }
            else
            {
                ExtentReportManager.LogStep(false, "No se mostró mensaje de éxito tras edición");
                throw new Exception("Error al editar el pago: mensaje de éxito no encontrado");
            }
        }

        // --- Escenario GP_006: Editar pago con datos vacíos ---
        [When("El usuario modifica el dato nombre con datos vacios \"\"")]
        public void CuandoElUsuarioModificaNombreVacio()
        {
            var modalNombre = _driver.FindElement(By.XPath(ModalNombreInput));
            modalNombre.Clear();
            modalNombre.SendKeys("   ");
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(2000);
            ExtentReportManager.LogInfo("Nombre modificado a vacío");
        }

        [Then("El sistema deberá mostrar el botón \"Guardar\" como deshabilitado")]
        public void EntoncesBotonGuardarModalDeshabilitado()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var modalGuardarButton = _driver.FindElement(By.XPath(ModalBtnGuardar));
            if (!modalGuardarButton.Enabled)
            {
                ExtentReportManager.LogStep(true, "El botón Guardar del modal está deshabilitado como se esperaba");
            }
            else
            {
                ExtentReportManager.LogStep(false, "El botón Guardar del modal está habilitado cuando debería estar deshabilitado");
                throw new Exception("El botón Guardar del modal está habilitado pese a campo vacío");
            }
        }

        // --- Escenario GP_007: Eliminar pago existente ---
    

        [Given("Dar click en el ícono de papelera para eliminar")]
        public void GivenDarClickEnElIConoDePapeleraParaEliminar()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Log de la URL actual para depurar
            ExtentReportManager.LogInfo($"URL actual: {_driver.Url}");

            // Espera que la tabla tenga al menos una fila
            var firstRow = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//table[@class='w-full table-auto']/tbody/tr[1]//button[@id='deleteButton']")));
            ExtentReportManager.LogInfo("Primera fila de la tabla localizada");

            var borrarButton = _driver.FindElement(By.XPath(BtnBorrar));
            borrarButton.Click();
            ExtentReportManager.LogInfo("Clic en el ícono de papelera para eliminar realizado");
        }


        [Then("El sistema deberá mostrar un botón de confirmación para eliminar el pago")]
        public void EntoncesMostrarBotonConfirmacionEliminar()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var confirmButton = wait.Until(drv => drv.FindElement(By.XPath("//button[text()='Confirmar']")));
            if (confirmButton.Displayed)
            {
                ExtentReportManager.LogStep(true, "Botón de confirmación para eliminar el pago mostrado");
            }
            else
            {
                ExtentReportManager.LogStep(false, "No se mostró botón de confirmación para eliminar el pago");
                throw new Exception("No se mostró el botón de confirmación en la eliminación del pago");
            }
        }

        [When("El usuario da click en el botón de confirmación")]
        public void CuandoElUsuarioDaClickEnElBotonConfirmacion()
        {
            var confirmButton = _driver.FindElement(By.XPath("//button[text()='Confirmar']"));
            confirmButton.Click();
            ExtentReportManager.LogInfo("Clic en el botón de confirmación realizado");
        }

        [Then("El usuario deberá ser borrado")]
        public void EntoncesPagoEliminado()
        {
            // Verifica que el registro ya no aparezca en la lista
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(drv => drv.FindElement(By.XPath("//table/tbody/tr")));
                ExtentReportManager.LogStep(false, "El pago aún aparece en la lista después de la eliminación");
                throw new Exception("El pago no fue eliminado");
            }
            catch (WebDriverTimeoutException)
            {
                ExtentReportManager.LogStep(true, "El pago fue eliminado correctamente");
            }
        }
    }
}
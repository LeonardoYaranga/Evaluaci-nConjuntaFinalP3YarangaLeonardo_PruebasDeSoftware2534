using OpenQA.Selenium;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using ReqnrollTestProject.Utilities;
using TDDTestingMVC.Data;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Reqnroll;

namespace ReqnrollTestProject.StepDefinitions
{
    [Binding]
    public class ClienteCrudStepDefinitions
    {
        private IWebDriver _driver;
        private static ExtentReports _extent;
        private static ExtentTest _test;
        private readonly ScenarioContext _scenarioContext;
        private readonly Cliente cliente1;
        public ClienteCrudStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var sparkReporter = new ExtentSparkReporter("ClienteCrudReport.html");
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverManager.GetDriver("firefox");
            _test = _extent.CreateTest(_scenarioContext.ScenarioInfo.Title);
        }

        [Given("voy a la página de Crear en \"(.*)\"")]
        public void GivenVoyALaPaginaDeCrearEn(string url)
        {
            _driver.Navigate().GoToUrl(url);
            _test.Log(Status.Info, $"Navegando a {url}");
        }

        [Given("hay un cliente existente con ID \"(.*)\" en \"(.*)\"")]
        public void GivenHayUnClienteExistenteConIDEn(string id, string url)
        {
            _driver.Navigate().GoToUrl(url);
            _test.Log(Status.Info, $"Navegando a la página de edición para el cliente con ID {id}");
        }

        [When("ingreso los siguientes detalles del cliente")]
        public void WhenIngresoLosSiguientesDetallesDelCliente(Table table)
        {
            var cliente = table.CreateInstance<Cliente>();

            // Método auxiliar para esperar y llenar un campo
            void FillField(string id, string value)
            {
                try
                {
                    var element = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                        .Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
                    element.Clear();
                    element.SendKeys(value);
                    _test.Log(Status.Info, $"Campo {id} llenado con '{value}'");
                }
                catch (WebDriverTimeoutException ex)
                {
                    _test.Log(Status.Fail, $"No se encontró el elemento con ID '{id}': {ex.Message}");
                    throw;
                }
            }

            // Llenar los campos con los IDs correctos
            FillField("cedula", cliente.Cedula); // ID ajustado a minúsculas
            FillField("Nombres", cliente.Nombres);
            FillField("Apellidos", cliente.Apellidos);
            FillField("FechaNacimiento", cliente.FechaNacimiento.ToString("yyyy-MM-dd"));
            FillField("Mail", cliente.Mail);
            FillField("Telefono", cliente.Telefono);
            FillField("Direccion", cliente.Direccion);

            // Manejar el checkbox Estado con SendKeys
            try
            {
                var estadoCheckbox = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementExists(By.Id("Estado")));

                // Desplazar el elemento a la vista
                ((IJavaScriptExecutor)_driver).ExecuteScript(
                    "arguments[0].scrollIntoView({block: 'center', inline: 'center', behavior: 'smooth'});",
                    estadoCheckbox
                );

                // Esperar a que sea interactuable
                estadoCheckbox = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementToBeClickable(By.Id("Estado")));

                bool isChecked = estadoCheckbox.Selected; // Verificar estado actual
                if (isChecked != cliente.Estado) // Solo envía Espacio si el estado deseado es diferente
                {
                    estadoCheckbox.SendKeys(Keys.Space); // Usa Espacio para togglear
                    _test.Log(Status.Info, cliente.Estado ? "Checkbox Estado marcado con Espacio" : "Checkbox Estado desmarcado con Espacio");
                }
                else
                {
                    _test.Log(Status.Info, "Checkbox Estado ya en el estado deseado");
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                _test.Log(Status.Fail, $"No se encontró o no fue interactuable el checkbox Estado: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _test.Log(Status.Fail, $"Error al interactuar con el checkbox Estado: {ex.Message}");
                throw;
            }

            _test.Log(Status.Info, "Todos los detalles del cliente ingresados");
        }

        [Then("los datos en el ultimo registro deben ser iguales a los ingresados")]
        public void ThenLosDatosEnElUltimoRegistroDebenSerIgualesALosIngresados(Table dataTable)
        {
            // Convertimos la tabla de Cucumber a un objeto Cliente
            var expectedCliente = dataTable.CreateInstance<Cliente>();

            try
            {
                // Esperamos hasta que la tabla esté visible
                var tablaClientes = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementIsVisible(By.Id("clientesTable")));

                // Obtenemos la última fila de la tabla
                var ultimaFila = _driver.FindElement(By.XPath("//table[@id='clientesTable']/tbody/tr[last()]"));

                // Extraemos los valores de la fila
                var valoresFila = ultimaFila.FindElements(By.TagName("td"));

                // Verificamos que la cantidad de columnas sea la esperada
                if (valoresFila.Count < 9)
                {
                    _test.Log(Status.Fail, "No se encontraron suficientes columnas en la última fila de la tabla.");
                    throw new Exception("Faltan columnas en la última fila de la tabla.");
                }

                // Obtenemos los valores de la fila
                string cedula = valoresFila[1].Text.Trim();
                string apellidos = valoresFila[2].Text.Trim();
                string nombres = valoresFila[3].Text.Trim();
                string fechaNacimiento = valoresFila[4].Text.Trim();
                string mail = valoresFila[5].Text.Trim();
                string telefono = valoresFila[6].Text.Trim();
                string direccion = valoresFila[7].Text.Trim();
                string estado = valoresFila[8].Text.Trim(); // Puede ser "Activo" o "Inactivo"

                // Convertimos el estado esperado a "Activo" o "Inactivo"
                string estadoEsperado = expectedCliente.Estado ? "activo" : "inactivo";

                Assert.Equal(expectedCliente.Cedula, cedula); 
                Assert.Equal(expectedCliente.Apellidos, apellidos); 
                Assert.Equal(expectedCliente.Nombres, nombres); 
                Assert.Equal(expectedCliente.FechaNacimiento.ToString("yyyy-MM-dd"), fechaNacimiento); 
                Assert.Equal(expectedCliente.Mail, mail); 
                Assert.Equal(expectedCliente.Telefono, telefono); 
                Assert.Equal(expectedCliente.Direccion, direccion); 
                Assert.Equal(estadoEsperado.ToLower(), estado.ToLower()); 

                _test.Log(Status.Pass, "Los datos en la última fila coinciden con los ingresados.");
            }
            catch (Exception ex)
            {
                _test.Log(Status.Fail, $"Error al validar los datos de la última fila: {ex.Message}");
                throw;
            }
        }

        [When("envío el formulario de creación")]
        public void WhenEnvioElFormularioDeCreacion()
        {
            var crearButton = _driver.FindElement(By.Id("btn-create"));
            crearButton.SendKeys(Keys.Enter);  
            _test.Log(Status.Info, "Formulario de creación enviado");
        }

        [When("envío el formulario de edición")]
        public void WhenEnvioElFormularioDeEdicion()
        {
            _driver.FindElement(By.Id("btn-Update")).Click();
            _test.Log(Status.Info, "Formulario de edición enviado");
        }

        [Then("debo ser redirigido a la lista de Clientes en \"(.*)\"")]
        public void ThenDeboSerRedirigidoALaListaDeClientesEn(string url)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            Thread.Sleep(2000);
            Assert.Equal(url, _driver.Url);
            _test.Log(Status.Pass, $"Redirigido correctamente a {url}");
        }

        [Then("debo ver un mensaje de éxito \"(.*)\"")]
        public void ThenDeboVerUnMensajeDeExito(string mensaje)
        {
            var mensajeElemento = _driver.FindElement(By.ClassName("alert-danger")); // Ajusta si el éxito usa otra clase
            Assert.Equal(mensaje, mensajeElemento.Text);
            _test.Log(Status.Pass, $"Mensaje de éxito encontrado: {mensaje}");
        }
        [Then("debo ver un mensaje de error de campo vacio {string}")]
        public void ThenDeboVerUnMensajeDeErrorDeCampoVacio(string mensajeEsperado)
        {
            try
            {
                // Obtener todos los mensajes de error en la página
                var mensajesError = _driver.FindElements(By.ClassName("field-validation-error"));

                if (mensajesError.Count == 0)
                {
                    _test.Log(Status.Fail, "No se encontró ningún mensaje de error en la página.");
                    throw new Exception("No se encontraron mensajes de error.");
                }

                // Tomar el primer mensaje de error visible
                var primerMensaje = mensajesError[0].Text.Trim();

                // Comparar con el mensaje esperado
                if (primerMensaje == mensajeEsperado)
                {
                    _test.Log(Status.Pass, $"Mensaje de error correcto: '{primerMensaje}'");
                }
                else
                {
                    _test.Log(Status.Fail, $"Mensaje de error incorrecto. Esperado: '{mensajeEsperado}', Encontrado: '{primerMensaje}'");
                    throw new Exception($"Mensaje de error incorrecto. Esperado: '{mensajeEsperado}', Encontrado: '{primerMensaje}'");
                }

                Assert.Equal(mensajeEsperado, primerMensaje);
            }
            catch (NoSuchElementException)
            {
                _test.Log(Status.Fail, "No se encontró ningún mensaje de error.");
                throw new Exception("No se encontró ningún mensaje de error.");
            }
        }

        [Then("los datos en el registro con ID {string} deben ser iguales a los ingresados")]
        public void ThenLosDatosEnElRegistroConIDDebenSerIgualesALosIngresados(string p0, DataTable dataTable)
        {
            var clienteEsperado = dataTable.CreateInstance<Cliente>();
            var row = _driver.FindElement(By.XPath($"//table//tr[td='{p0}']"));
            var cells = row.FindElements(By.TagName("td"));
            var estadoCliente = clienteEsperado.Estado ? "activo" : "inactivo";
            Assert.Equal(p0, cells[0].Text); // ID
            Assert.Equal(clienteEsperado.Cedula, cells[1].Text);
            Assert.Equal(clienteEsperado.Nombres, cells[3].Text);
            Assert.Equal(clienteEsperado.Apellidos, cells[2].Text);
            Assert.Equal(clienteEsperado.FechaNacimiento.ToString("yyyy-MM-dd"), cells[4].Text);
            Assert.Equal(clienteEsperado.Mail, cells[5].Text);
            Assert.Equal(clienteEsperado.Telefono, cells[6].Text);
            Assert.Equal(clienteEsperado.Direccion, cells[7].Text);
            Assert.Equal(estadoCliente.ToLower(), cells[8].Text.ToLower());

            _test.Log(Status.Pass, $"Datos del registro con ID {p0} verificados correctamente");
        }


        //#<span class="text-danger field-validation-error" data-valmsg-for="Cedula" data-valmsg-replace="true">La cédula es obligatoria.</span>

        [Then("debo ver un mensaje de error \"(.*)\"")]
        public void ThenDeboVerUnMensajeDeError(string mensaje)
        {
            try
            {
                // Intentar encontrar el elemento directamente
                var mensajeElemento = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementIsVisible(By.ClassName("alert-danger")));

                // Desplazar el elemento a la vista
                ((IJavaScriptExecutor)_driver).ExecuteScript(
                    "arguments[0].scrollIntoView({block: 'center', inline: 'center', behavior: 'smooth'});",
                    mensajeElemento
                );

                // Verificar el texto
                Assert.Equal(mensaje, mensajeElemento.Text);
                _test.Log(Status.Pass, $"Mensaje de error encontrado: {mensaje}");
            }
            catch (WebDriverTimeoutException)
            {
                // Si no se encuentra inmediatamente, desplazar la página hacia abajo y buscar de nuevo
                _test.Log(Status.Info, "Mensaje alert-danger no encontrado inicialmente, desplazando hacia abajo...");
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                // Esperar un momento para que el scroll termine y buscar otra vez
                var mensajeElemento = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementIsVisible(By.ClassName("alert-danger")));

                Assert.Equal(mensaje, mensajeElemento.Text);
                _test.Log(Status.Pass, $"Mensaje de error encontrado después de scroll: {mensaje}");
            }
            catch (Exception ex)
            {
                _test.Log(Status.Fail, $"No se pudo encontrar el mensaje de error: {ex.Message}");
                throw;
            }
        }

        [AfterScenario]
        public void Down()
        {
            _driver.Quit();
            _extent.Flush();
        }
    }

   
}
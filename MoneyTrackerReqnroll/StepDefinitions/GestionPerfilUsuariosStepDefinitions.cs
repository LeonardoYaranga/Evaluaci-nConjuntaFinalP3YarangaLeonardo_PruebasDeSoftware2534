using System;
using AventStack.ExtentReports;
using MoneyTrackerReqnroll.Reports;
using MoneyTrackerReqnroll.Utilities;
using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MoneyTrackerReqnroll.StepDefinitions
{
    [Binding]
    public class GestionPerfilUsuariosStepDefinitions
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;

        public GestionPerfilUsuariosStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = WebDriverManager.GetDriver("edge");  
        }

        [When("El usuario navega a la página de inicio de sesión")]
        public void WhenElUsuarioNavegaALaPaginaDeInicioDeSesion()
        {
            if (_driver.WindowHandles.Count > 1)
            {
                _driver.SwitchTo().Window(_driver.WindowHandles[0]);
                foreach (var handle in _driver.WindowHandles.Skip(1))
                {
                    _driver.SwitchTo().Window(handle).Close();
                }
                _driver.SwitchTo().Window(_driver.WindowHandles[0]);
            }
            _driver.Navigate().GoToUrl("http://localhost:3000"); // Ajusta a /login si aplica
            ExtentReportManager.LogInfo("Navegado a la página de inicio de sesión");
        }

        [When("El usuario ingresa su correo y contraseña")]
        public void WhenElUsuarioIngresaSuCorreoYContrasena(Table table)
        {
            ExtentReportManager.LogInfo($"URL actual: {_driver.Url}");
            var row = table.Rows[0];
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var emailField = wait.Until(drv => drv.FindElement(By.Id("email")));
            emailField.SendKeys(row["Correo"]);
            var passwordField = _driver.FindElement(By.Id("password"));
            passwordField.SendKeys(row["Contraseña"]);
            ExtentReportManager.LogInfo($"Correo: {row["Correo"]}, Contraseña: {row["Contraseña"]} ingresados");
        }

        [When("El usuario navega a la página principal autenticada")]
        public void WhenElUsuarioNavegaALaPaginaPrincipalAutenticada()
        {
            _driver.Navigate().GoToUrl("http://localhost:3000/main/home"); // Ajusta la URL real
            ExtentReportManager.LogInfo("Navegado a la página principal autenticada");
        }

        [When("El usuario se dirige al apartado de Perfil")]
        public void WhenElUsuarioSeDirigeAlApartadoDePerfil()
        {
            _driver.Navigate().GoToUrl("http://localhost:3000/main/usersettings");
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            ExtentReportManager.LogInfo("Navegado directamente al apartado de Perfil");
            // Espera que el formulario esté presente
          
        }

        [Then("Los datos ingresados coinciden con los datos mostrados en el perfil")]
        public void ThenLosDatosIngresadosCoincidenConLosDatosMostradosEnElPerfil(Table table)
        {
            var row = table.Rows[0];
            string displayedName = _driver.FindElement(By.Id("name")).GetAttribute("value");
            string displayedLastname = _driver.FindElement(By.Id("lastname")).GetAttribute("value");
            string displayedEmail = _driver.FindElement(By.Id("emailAddress")).GetAttribute("value");

            if (displayedName == row["Nombre"] && displayedLastname == row["Apellido"] && displayedEmail == row["Correo"])
            {
                ExtentReportManager.LogStep(true, $"Datos coinciden: Nombre={displayedName}, Apellido={displayedLastname}, Correo={displayedEmail}");
            }
            else
            {
                ExtentReportManager.LogStep(false, $"Esperado: Nombre={row["Nombre"]}, Apellido={row["Apellido"]}, Correo={row["Correo"]}, pero se obtuvo: Nombre={displayedName}, Apellido={displayedLastname}, Correo={displayedEmail}");
                throw new Exception($"Datos no coinciden. Esperado: {row["Nombre"]}, {row["Apellido"]}, {row["Correo"]}, pero se obtuvo: {displayedName}, {displayedLastname}, {displayedEmail}");
            }
        }

        [When("El usuario edita el formulario de perfil con")]
        public void WhenElUsuarioEditaElFormularioDePerfilCon(Table table)
        {
            var row = table.Rows[0];
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Campo Nombre
            var nameField = _driver.FindElement(By.Id("name"));
            ExtentReportManager.LogInfo($"Antes de editar - Nombre: Enabled={nameField.Enabled}, Displayed={nameField.Displayed}, Value={nameField.GetAttribute("value")}");
            nameField.Click(); // Enfoca el campo
            nameField.SendKeys(Keys.Control + "a"); // Selecciona todo
            nameField.SendKeys(Keys.Delete); // Borra el contenido seleccionado
            if (!string.IsNullOrEmpty(row["Nombre"]))
            {
                nameField.SendKeys(row["Nombre"]);
            }
            nameField.SendKeys(Keys.Tab); // Desenfoca para disparar onBlur u otros eventos
            ExtentReportManager.LogInfo($"Después de editar - Nombre: Value={nameField.GetAttribute("value")}");
             wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            // Campo Apellido
            var lastnameField = _driver.FindElement(By.Id("lastname"));
            ExtentReportManager.LogInfo($"Antes de editar - Apellido: Enabled={lastnameField.Enabled}, Displayed={lastnameField.Displayed}, Value={lastnameField.GetAttribute("value")}");
            lastnameField.Click();
            lastnameField.SendKeys(Keys.Control + "a");
            lastnameField.SendKeys(Keys.Delete);
            if (!string.IsNullOrEmpty(row["Apellido"]))
            {
                lastnameField.SendKeys(row["Apellido"]);
            }
            lastnameField.SendKeys(Keys.Tab); // Desenfoca para disparar eventos
            ExtentReportManager.LogInfo($"Después de editar - Apellido: Value={lastnameField.GetAttribute("value")}");

            ExtentReportManager.LogInfo($"Formulario editado con: Nombre={row["Nombre"]}, Apellido={row["Apellido"]}, del Correo={row["Correo"]}");
        }

        [When("El usuario edita el formulario de perfil con biografía")]
        public void WhenElUsuarioEditaElFormularioDePerfilConBiografia(Table table)
        {
            var row = table.Rows[0];
            var nameField = _driver.FindElement(By.Id("name"));
            nameField.Clear();
            nameField.SendKeys("");
            nameField.SendKeys(row["Nombre"]);
            var lastnameField = _driver.FindElement(By.Id("lastname"));
            lastnameField.Clear();
            nameField.SendKeys("");
            lastnameField.SendKeys(row["Apellido"]);
           
            var bioField = _driver.FindElement(By.Id("bio"));
            bioField.Clear();
            bioField.SendKeys(row["Biografía"]);
            ExtentReportManager.LogInfo($"Formulario editado con: Nombre={row["Nombre"]}, Apellido={row["Apellido"]},del Correo={row["Correo"]}, Biografía={row["Biografía"]}");
        }

        [When("El usuario sube una foto al campo {string} con el archivo {string}")]
        public void WhenElUsuarioSubeUnaFotoAlCampoConElArchivo(string fieldName, string fileName)
        {
            _driver.FindElement(By.XPath("//button[text()='Actualizar']")).Click();
            var photoField = _driver.FindElement(By.CssSelector("input[type='file']"));
            photoField.SendKeys(@"C:\Users\leona\Downloads\ArchivoCarnet.png");
            ExtentReportManager.LogInfo($"Foto '{fileName}' subida al campo '{fieldName}'");
        }

        [Then("La foto subida se muestra en la sección {string}")]
        public void ThenLaFotoSubidaSeMuestraEnLaSeccion(string sectionName)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Espera que el elemento <img> esté presente y visible
            var photoElement = _driver.FindElement(By.CssSelector(".h-14.w-14 img"));

            // Verifica que el src contenga "blob" y no esté vacío
            string srcValue = photoElement.GetAttribute("src");
            ExtentReportManager.LogInfo($"Valor del src: {srcValue}");

            if (photoElement.Displayed && !string.IsNullOrEmpty(srcValue) && srcValue.Contains("blob"))
            {
                ExtentReportManager.LogStep(true, $"La foto subida se muestra en la sección '{sectionName}' con src: {srcValue}");
            }
            else
            {
                ExtentReportManager.LogStep(false, $"La foto no se muestra correctamente en la sección '{sectionName}'. Src: {srcValue}, Displayed: {photoElement.Displayed}");
                throw new Exception($"La foto no se muestra correctamente en la sección '{sectionName}'. Src: {srcValue}");
            }
        }

        [Then("El campo de apellido muestra una validacion HTML {string}")]
        public void ThenElCampoDeApellidoMuestraUnaValidacionHTML(string p0)
        {
            var nameField = _driver.FindElement(By.Id("lastname"));
            string actualMessage = nameField.GetAttribute("validationMessage");
            if (actualMessage.Contains(p0))
            {
                ExtentReportManager.LogStep(true, $"Validación HTML encontrada: {actualMessage}");
            }
            else
            {
                ExtentReportManager.LogStep(false, $"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
                throw new Exception($"Esperado: '{p0}', pero se obtuvo: '{actualMessage}'");
            }
        }

[When("El usuario confirma la eliminación haciendo clic en {string}")]
    public void WhenElUsuarioConfirmaLaEliminacionHaciendoClicEn(string buttonText)
    {
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // Espera de hasta 10s
            Thread.Sleep(3000);
        try
        {
 
            IWebElement deleteButton = _driver.FindElement(By.XPath($"//button[contains(@class, 'bg-gray-500') and normalize-space(text())='{buttonText}']"));

            deleteButton.Click(); // Hace clic en el botón
        wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // Espera de hasta 10s
                Thread.Sleep(3000);
                ExtentReportManager.LogInfo($"Confirmación de eliminación realizada con clic en '{buttonText}'");
        }
        catch (WebDriverTimeoutException)
        {
            ExtentReportManager.LogStep(false,$"No se encontró el botón '{buttonText}' dentro del tiempo límite.");
            throw new Exception($"No se encontró el botón '{buttonText}' en la página.");
        }
    }


    [Then("El sistema redirige a la página de login con la URL {string}")]
        public void ThenElSistemaRedirigeALaPaginaDeLoginConLaURL(string expectedUrl)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); 
            string actualUrl = _driver.Url;
            if (actualUrl == expectedUrl)
            {
                ExtentReportManager.LogStep(true, $"Redirigido a la URL esperada: {actualUrl}");
            }
            else
            {
                ExtentReportManager.LogStep(false, $"Esperado: '{expectedUrl}', pero se obtuvo: '{actualUrl}'");
                throw new Exception($"Esperado: '{expectedUrl}', pero se obtuvo: '{actualUrl}'");
            }
        }
    }
}
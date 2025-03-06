using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenQA.Selenium.BiDi.Modules.Network.AuthCredentials;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;

namespace ProductTests
{
    public class PracticeForm : IDisposable
    {
        

        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public PracticeForm()
        {
            _driver = new EdgeDriver();

            //var options = new ChromeOptions();
            //options.BinaryLocation = @"C:\Users\leona\AppData\Local\BraveSoftware\Brave-Browser\Application\brave.exe"; // Ruta del ejecutable de Bra
            //_driver = new ChromeDriver(options);

            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        //Navegar a la pagina de autenticacion   //https://demoqa.com/automation-practice-form

        [Fact]
        public void navigateToDemoqa()
        {
            // Navegar a la URL
            _driver.Navigate().GoToUrl("https://demoqa.com/automation-practice-form");
            Thread.Sleep(2000);
            //<h1 class="text-center">Practice Form</h1><h1 class="text-center">Practice Form</h1>
            var header = _driver.FindElement(By.TagName("h1"));
            Thread.Sleep(2000);

            Assert.Equal("Practice Form", header.Text);
        }

        //Enviar formulario vacio y verificar que aparezcan los errores de validacion
        //<form novalidate="" id="userForm" class=""></form> sin errores
        //<form novalidate="" id="userForm" class="was-validated"></form> con errores
        [Fact]
        public void submitEmptyForm()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/automation-practice-form");
            Thread.Sleep(2000);
            var submitButton=_driver.FindElement(By.Id("submit"));
           
            Thread.Sleep(1000);
            submitButton.SendKeys(Keys.Enter); //Se envia con enter en lugar de click porque el boton esta mas abajo y no se puede hacer click

            Thread.Sleep(2000);
            var errorValidacion = _driver.FindElement(By.ClassName("was-validated"));

            Assert.True(errorValidacion != null, "Faltan completar campos");
        
        }

        //Formulario con datos invalid (como numero de 10 digitos) verificar que no se envie
        [Fact]
        public void ValidarFormularioConTelefonoInvalido()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/automation-practice-form");
            Thread.Sleep(1000);
            Actions actions = new Actions(_driver);
            actions.MoveToElement(_driver.FindElement(By.Id("submit"))).Perform();
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("firstName")).SendKeys("Leonardo");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("lastName")).SendKeys("Yaranga");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("userEmail")).SendKeys("leonardoY@gmail.com");
            Thread.Sleep(1000);

            // _driver.FindElement(By.Id("gender-radio-1")).Click();  //genero male
            _driver.FindElement(By.XPath("//label[@for='gender-radio-1']")).Click();

            Thread.Sleep(1000);
            // numero +10 digitos
            _driver.FindElement(By.Id("userNumber")).SendKeys("012345678901141");
            Thread.Sleep(1000);

            //fecha, se pone automaticamente
            // _driver.FindElement(By.Id("dateOfBirthInput")).SendKeys("");
            Thread.Sleep(1000);

          
            _driver.FindElement(By.Id("subjectsInput")).SendKeys("Math");
            Thread.Sleep(1000);

            // Hacer clic fuera del dropdown para cerrarlo
            IWebElement body = _driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Escape);

            //_driver.FindElement(By.Id("hobbies-checkbox-1")).Click(); // "Sports".
            _driver.FindElement(By.XPath("//label[@for='hobbies-checkbox-1']")).Click();
            Thread.Sleep(1000);


            // sin imagen.

            _driver.FindElement(By.Id("currentAddress")).SendKeys("Calle 123");
            Thread.Sleep(1000);

            IWebElement stateDropdown = _driver.FindElement(By.Id("react-select-3-input"));
            Thread.Sleep(1000);
            stateDropdown.SendKeys("NCR");
            Thread.Sleep(1000);
            stateDropdown.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            IWebElement cityDropdown = _driver.FindElement(By.Id("react-select-4-input"));
            Thread.Sleep(1000);
            cityDropdown.SendKeys("Delhi");
            Thread.Sleep(1000);
            cityDropdown.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("submit")).SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            var errorValidacion = _driver.FindElement(By.ClassName("was-validated"));

            Assert.True(errorValidacion != null, "Numero invalido");
            //Assert.True(errorValidacion == null, "Numero valido");

        }

        [Fact]
        public void ValidarFormularioValido()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/automation-practice-form");
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(_driver.FindElement(By.Id("submit"))).Perform();
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("firstName")).SendKeys("Leonardo");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("lastName")).SendKeys("Yaranga");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("userEmail")).SendKeys("leonardoY@gmail.com");
            Thread.Sleep(1000);
            //género male
            _driver.FindElement(By.XPath("//label[@for='gender-radio-1']")).Click();
            Thread.Sleep(1000);

            // telefono 10 digitos
            _driver.FindElement(By.Id("userNumber")).SendKeys("0995667373");
            Thread.Sleep(1000);

            //fecha, se pone automaticamente

            _driver.FindElement(By.Id("subjectsInput")).SendKeys("Math");
            Thread.Sleep(1000);

            // Hacer clic fuera del dropdown para cerrarlo
            IWebElement body = _driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Escape);

            _driver.FindElement(By.XPath("//label[@for='hobbies-checkbox-1']")).Click();
            Thread.Sleep(1000);

            // sin imagen.

            _driver.FindElement(By.Id("currentAddress")).SendKeys("Sangolqui");
            Thread.Sleep(1000);

            IWebElement stateDropdown = _driver.FindElement(By.Id("react-select-3-input"));
            stateDropdown.SendKeys("NCR");
            stateDropdown.SendKeys(Keys.Enter);

            Thread.Sleep(1000);
            IWebElement cityDropdown = _driver.FindElement(By.Id("react-select-4-input"));
            cityDropdown.SendKeys("Delhi");
            cityDropdown.SendKeys(Keys.Enter);

            //Enviar
            _driver.FindElement(By.Id("submit")).SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            IWebElement modalTitle = _driver.FindElement(By.Id("example-modal-sizes-title-lg"));
            Thread.Sleep(1000);
            Assert.True(modalTitle.Displayed, "El formulario se envió correctamente.");
            Thread.Sleep(1000);
            string expectedMessage = "Thanks for submitting the form";
            Assert.Equal(expectedMessage, modalTitle.Text);
           
        }

        //Validar el correo y cedula(no hay cedula)
        [Fact]
        public void ValidarCorreo()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/automation-practice-form");
            Thread.Sleep(1000);
            Actions actions = new Actions(_driver);
            actions.MoveToElement(_driver.FindElement(By.Id("submit"))).Perform();
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("firstName")).SendKeys("Leonardo");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("lastName")).SendKeys("Yaranga");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("userEmail")).SendKeys("leonardoY");
            Thread.Sleep(1000);

            // _driver.FindElement(By.Id("gender-radio-1")).Click();  //genero male
            _driver.FindElement(By.XPath("//label[@for='gender-radio-1']")).Click();

            Thread.Sleep(1000);
            // numero +10 digitos
            _driver.FindElement(By.Id("userNumber")).SendKeys("012345678901141");
            Thread.Sleep(1000);

            //fecha, se pone automaticamente
            // _driver.FindElement(By.Id("dateOfBirthInput")).SendKeys("");
            Thread.Sleep(1000);


            _driver.FindElement(By.Id("subjectsInput")).SendKeys("Math");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("subjectsInput")).SendKeys(Keys.Enter);

            // Hacer clic fuera del dropdown para cerrarlo
            IWebElement body = _driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Escape);

            //_driver.FindElement(By.Id("hobbies-checkbox-1")).Click(); // "Sports".
            _driver.FindElement(By.XPath("//label[@for='hobbies-checkbox-1']")).Click();
            Thread.Sleep(1000);


            // sin imagen.

            _driver.FindElement(By.Id("currentAddress")).SendKeys("Calle 123");
            Thread.Sleep(1000);

            IWebElement stateDropdown = _driver.FindElement(By.Id("react-select-3-input"));
            Thread.Sleep(1000);
            stateDropdown.SendKeys("NCR");
            Thread.Sleep(1000);
            stateDropdown.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            IWebElement cityDropdown = _driver.FindElement(By.Id("react-select-4-input"));
            Thread.Sleep(1000);
            cityDropdown.SendKeys("Delhi");
            Thread.Sleep(1000);
            cityDropdown.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("submit")).SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            var errorValidacion = _driver.FindElement(By.ClassName("was-validated"));

            Assert.True(errorValidacion != null, "Correo invalido");
        }



        public void Dispose()
        {
            _driver.Quit();
        }

    }
}

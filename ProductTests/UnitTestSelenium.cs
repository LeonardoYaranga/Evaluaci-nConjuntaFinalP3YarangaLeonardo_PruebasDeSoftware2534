
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Xunit; 

namespace ProductTests
{
    public class UnitTestSelenium
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        public UnitTestSelenium()
        {
            _driver = new EdgeDriver();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
        }
        //public static bool EsMailValido(string email)
        //{
        //    return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        //}

        //[Theory]
        //[InlineData("usuario@gmail.com", true)]
        //[InlineData("usuario.gmail.com", false)]
        //[InlineData("usuario@hotmail.es", true)]
        //[InlineData("usuario@dominio.online", true)] 
        //[InlineData("usuario@.com", false)] 
        //[InlineData("usuario@dominio", false)]
        //public void ValidarDetectarMailValido(string email, bool expected)
        //{
        //    bool resultado = EsMailValido(email);
        //    Assert.Equal(expected, resultado);
        //}
        [Fact]
        public void Test_NavegadorGoogle()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://www.bing.com");

                var buscarTexto = _wait.Until(d => d.FindElement(By.Name("q")));

                // Enviar la búsqueda
                Thread.Sleep(2000);
                buscarTexto.SendKeys("Clima");
                Thread.Sleep(2000);
                buscarTexto.SendKeys(Keys.Enter);
                Thread.Sleep(2000);


                var resultados = _wait.Until(d => d.FindElements(By.CssSelector("h2")).ToList());
                Thread.Sleep(5000);

                Assert.True(resultados.Count > 0, "No se encontraron resultados");

                Console.WriteLine("Se encontraron resultados");
                Console.WriteLine("Cantidad de resultados: " + resultados.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                _driver.Quit();
            }
        }

    }
}
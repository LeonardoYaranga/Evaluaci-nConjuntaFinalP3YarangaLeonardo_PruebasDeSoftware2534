using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTests
{
    public class TestCliente : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public TestCliente()
        {
            _driver = new EdgeDriver();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
        }

        [Fact]
        public void ObtenerListaUsuarios_TieneEncabezadosYRegistros()
        {
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente");

            // Buscar la tabla por ID
            var tablaUsuarios = _driver.FindElement(By.Id("clientesTable"));
            Assert.NotNull(tablaUsuarios);
            Thread.Sleep(1000); 
            var filas = tablaUsuarios.FindElements(By.TagName("tr"));

            // Verificar que hay al menos una fila en el tbody
            Assert.True(filas.Count > 1, "No se encontraron usuarios en la tabla."); //EL tr 1 es de los encabezados

            var encabezados = tablaUsuarios.FindElements(By.TagName("th"));
            string[] columnasEsperadas = { "Codigo", "Cedula", "Apellidos", "Nombres", "FechaNacimiento", "Mail", "Telefono", "Direccion", "Estado" };

            for (int i = 0; i < columnasEsperadas.Length; i++)
            {
                Assert.Equal(columnasEsperadas[i], encabezados[i].Text);
            }
        }


        [Fact]
        public void Create_ReturnCreateView()
        {
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente/Create"); 

            _driver.FindElement(By.Name("Cedula")).SendKeys("1726781402");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Nombres")).SendKeys("Andres");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Apellidos")).SendKeys("Silva");
            Thread.Sleep(1000);
            //Mandar en formato fecha          

            var fechaInput = _driver.FindElement(By.Name("FechaNacimiento"));
            fechaInput.SendKeys("02-20-2003"); 
            
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Mail")).SendKeys("sandres@gmail.com");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Telefono")).SendKeys("0987774321");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Direccion")).SendKeys("Quito");
            Thread.Sleep(1000);
            //_driver.FindElement(By.Name("Estado")).SendKeys("true");  //Si no se manda, se queda como false
            _driver.FindElement(By.Id("btn-create")).SendKeys(Keys.Enter);
            Thread.Sleep(2000);
            Assert.Equal("http://localhost:5051/Cliente", _driver.Url);
        }
        [Fact]
        public void Create_CedulaInvalida_Ecuatoriana()
        {
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente/Create");
            _driver.FindElement(By.Name("Cedula")).SendKeys("1726781492"); 
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Nombres")).SendKeys("Carlos");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Apellidos")).SendKeys("Alca");
            Thread.Sleep(1000);
            var fechaInput2 = _driver.FindElement(By.Name("FechaNacimiento"));
            fechaInput2.SendKeys("05-15-1990");

            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Mail")).SendKeys("calca@gmail.com");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Telefono")).SendKeys("0998765432");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Direccion")).SendKeys("Quito");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("btn-create")).SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            // Verifica que el mensaje de error es el correcto
            var mensajeError = _driver.FindElement(By.CssSelector(".alert-danger")).Text;
            Assert.Contains("Error: La Cedula no es valida.", mensajeError);

        }

        [Fact]
        public void Create_ReturnCreateView_WithDuplicateCedulaError()
        {
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente/Create");

            // Primer registro con una cédula
            _driver.FindElement(By.Name("Cedula")).SendKeys("1722263009");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Nombres")).SendKeys("Paco");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Apellidos")).SendKeys("Quiroga");
            Thread.Sleep(1000);
            var fechaInput = _driver.FindElement(By.Name("FechaNacimiento"));
            fechaInput.SendKeys("02-20-2002");
            

            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Mail")).SendKeys("pacoq@gmail.com");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Telefono")).SendKeys("0987567814");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Direccion")).SendKeys("Quito");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("btn-create")).SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            Assert.Equal("http://localhost:5051/Cliente", _driver.Url);

            // Ahora intenta registrar otro cliente con la misma cédula
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente/Create");
            _driver.FindElement(By.Name("Cedula")).SendKeys("1722263009");  // La misma cédula
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Nombres")).SendKeys("Carlos");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Apellidos")).SendKeys("Alca");
            Thread.Sleep(1000);
            var fechaInput2 = _driver.FindElement(By.Name("FechaNacimiento"));
            fechaInput2.SendKeys("05-15-1990");

            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Mail")).SendKeys("calca@gmail.com");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Telefono")).SendKeys("0998765432");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Direccion")).SendKeys("Quito");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("btn-create")).SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            // Verifica que el mensaje de error es el correcto
            var mensajeError = _driver.FindElement(By.CssSelector(".alert-danger")).Text;
            Assert.Contains("Error: La Cedula ya esta registrada.", mensajeError);
        }


        //public string CodigoClienteGlobal { get; private set; }

        //[Fact]
        //public void ObtenerCodigoCliente()
        //{
        //    _driver.Navigate().GoToUrl("http://localhost:5051/Cliente/Index");

        //    // Obtener todas las filas de la tabla
        //    var tablaClientes = _driver.FindElement(By.Id("clientesTable"));
        //    var filas = tablaClientes.FindElements(By.TagName("tr"));
        //    var ultimaFila = filas.Last();
        //    var codigoCliente = ultimaFila.FindElements(By.TagName("td")).First().Text;

        //    // Almacenar el código globalmente
        //    CodigoClienteGlobal = codigoCliente;

        //    // Imprimir para verificar
        //    Console.WriteLine("Código Cliente (último creado): " + CodigoClienteGlobal);
        //}

        [Fact]
        public void EditarUltimoCliente()
        {
                     
            //O sino obtener el ultimo codigo aqui directamente
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente");
            Thread.Sleep(1000);
            var tablaClientes = _driver.FindElement(By.Id("clientesTable"));
            var filas = tablaClientes.FindElements(By.TagName("tr"));
            var ultimaFila = filas.Last();
            var codigoCliente = ultimaFila.FindElements(By.TagName("td")).First().Text;
            Thread.Sleep(1000);

            _driver.Navigate().GoToUrl($"http://localhost:5051/Cliente/Edit/{codigoCliente}");
            Thread.Sleep(1000);
            //1100848835
            _driver.FindElement(By.Name("Cedula")).Clear();
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Cedula")).SendKeys("1100848835");

            _driver.FindElement(By.Name("Nombres")).Clear();
            _driver.FindElement(By.Name("Nombres")).SendKeys("Raul Editado");

            _driver.FindElement(By.Id("btn-Update")).SendKeys(Keys.Enter);
            Thread.Sleep(3000);
            // Verificar que el cliente fue editado correctamente
            Assert.Equal("http://localhost:5051/Cliente",_driver.Url);
        }

        [Fact]
        public void EditarCliente_RepitiendoCedula()
        {
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente/Create");

            // Primer registro con una cédula
            _driver.FindElement(By.Name("Cedula")).SendKeys("1755008925");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Nombres")).SendKeys("Jua");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Apellidos")).SendKeys("Pa");
            Thread.Sleep(1000);
            var fechaInput = _driver.FindElement(By.Name("FechaNacimiento"));
            fechaInput.SendKeys("02-20-2002");


            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Mail")).SendKeys("jpinza@gmail.com");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Telefono")).SendKeys("0997548978");
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Direccion")).SendKeys("Quito");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("btn-create")).SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            Assert.Equal("http://localhost:5051/Cliente", _driver.Url);

            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente");
            Thread.Sleep(1000);
            var tablaClientes = _driver.FindElement(By.Id("clientesTable"));
            var filas = tablaClientes.FindElements(By.TagName("tr"));

            var segundaFila = filas.ElementAtOrDefault(1);  
            var codigoCliente = segundaFila.FindElements(By.TagName("td")).First().Text;
            Thread.Sleep(1000);

            _driver.Navigate().GoToUrl($"http://localhost:5051/Cliente/Edit/{codigoCliente}");
            Thread.Sleep(1000);

            _driver.FindElement(By.Name("Cedula")).Clear();
            Thread.Sleep(1000);
            _driver.FindElement(By.Name("Cedula")).SendKeys("1755008925");
            
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("btn-Update")).SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            // Verifica que el mensaje de error es el correcto
            var mensajeError = _driver.FindElement(By.CssSelector(".alert-danger")).Text;
            Assert.Contains("Error: La Cedula ya esta registrada.", mensajeError);
        }

        //eliminar el ultimo cliente
        [Fact]
        public void EliminarUltimoCliente()
        {
            // Verifica si se ha obtenido un código válido
            //Assert.NotNull(CodigoClienteGlobal);

            //O sino obtener el ultimo codigo aqui directamente
            _driver.Navigate().GoToUrl("http://localhost:5051/Cliente");
            Thread.Sleep(1000);
            var tablaClientes = _driver.FindElement(By.Id("clientesTable"));
            var filas = tablaClientes.FindElements(By.TagName("tr"));
            var ultimaFila = filas.Last();
            var codigoCliente = ultimaFila.FindElements(By.TagName("td")).First().Text;
            Thread.Sleep(1000);
            _driver.Navigate().GoToUrl($"http://localhost:5051/Cliente/Delete/{codigoCliente}");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("btn-delete")).SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            //Debe redirigir a la lista de clientes
            Assert.Equal("http://localhost:5051/Cliente", _driver.Url);
        }


        public void Dispose()
        {
            _driver.Quit();
        }
    }
}

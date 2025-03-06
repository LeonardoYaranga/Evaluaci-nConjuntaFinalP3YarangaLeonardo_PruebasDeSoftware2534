//using System;
//using Reqnroll;
//using TDDTestingMVC.Models;
//using FluentAssertions;

//using TDDTestingMVC.Data;
//namespace ReqnrollTestProject.StepDefinitions
//{
//    [Binding]
//    public class InsertStepDefinitions
//    {
//        private readonly ClienteDataAccessLayer clienteDAL = new ClienteDataAccessLayer();


//        [Given("Llenar los campos en el formulario")]
//        public void GivenLlenarLosCamposEnElFormulario(DataTable dataTable)
//        {
//            var resultado = dataTable.Rows.Count();
//           // Assert.True(resultado > 0); Este tambien vale
//           resultado.Should().BeGreaterThanOrEqualTo(1);



//        }

//        [When("Ingresar registros en la base de datos")]
//        public void WhenIngresarRegistrosEnLaBaseDeDatos(DataTable dataTable)
//        {
//            var cliente = dataTable.CreateSet<Cliente>().ToList();

//            Cliente clientToInsert = new Cliente();
//            foreach (var item in cliente)
//            {
//                clientToInsert.Cedula = item.Cedula;
//                clientToInsert.Nombres = item.Nombres;
//                clientToInsert.Apellidos = item.Apellidos;
//                clientToInsert.FechaNacimiento = item.FechaNacimiento;
//                clientToInsert.Mail = item.Mail;
//                clientToInsert.Telefono = item.Telefono;
//                clientToInsert.Direccion = item.Direccion;
//                clientToInsert.Estado = item.Estado;
//            }
//            clienteDAL.addCliente(clientToInsert);
//        }

//        [Then("Verificacion de ingreso a la base de datos")]
//        public void ThenVerificacionDeIngresoALaBaseDeDatos(DataTable dataTable)
//        {
//            var cliente = dataTable.CreateSet<Cliente>().ToList();

//            //Lista de clientes
//            var clientes = clienteDAL.getAllClientes();
//            //Obtene el id del ultimo cliente ingresado
//            int codigoCliObtenido = (int)clientes.FindLast(x => x.Cedula == cliente.First().Cedula).Codigo;

//            codigoCliObtenido.Should().BeGreaterThan(0);
//        }

//        }
//    }

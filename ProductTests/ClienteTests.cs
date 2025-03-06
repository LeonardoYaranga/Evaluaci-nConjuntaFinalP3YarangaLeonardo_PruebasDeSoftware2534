using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TDDTestingMVC.Data;
using TDDTestingMVC.Models;
namespace ProductTests
{
    //internal class ClienteTests
    public class ClienteTests
    {
        private readonly Mock<ClienteDataAccessLayer> _mockClienteDAL;

        public ClienteTests()
        {
            _mockClienteDAL = new Mock<ClienteDataAccessLayer>();
        }

        [Fact]
        public void GetAllClientes_DeberiaRetornarListaClientes()
        {
            // Arrange: Configurar Moq para devolver datos simulados
            var listaClientes = new List<Cliente>
            {
                new Cliente
                {
                    Codigo = 1,
                    Cedula = "1726781408",
                    Nombres = "Juan",
                    Apellidos = "Pérez",
                    FechaNacimiento = new DateTime(1990, 1, 1),
                    Mail = "juan@example.com",
                    Telefono = "0987654321",
                    Direccion = "Quito",
                    Estado = true
                },                new Cliente
                {
                    Codigo = 2,
                    Cedula = "1726781409",
                    Nombres = "María",
                    Apellidos = "Gómez",
                    FechaNacimiento = new DateTime(1992, 2, 2),
                    Mail = "maria@example.com",
                    Telefono = "0987654322",
                    Direccion = "Guayaquil",
                    Estado = false
                }
            };

            _mockClienteDAL.Setup(x => x.getAllClientes()).Returns(listaClientes);

            // Act: Llamar al método
            var resultado = _mockClienteDAL.Object.getAllClientes();

            // Assert: Verificar que los datos simulados son devueltos correctamente
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Juan", resultado[0].Nombres);
            Assert.Equal("María", resultado[1].Nombres);
            Assert.Equal("Quito", resultado[0].Direccion);
            Assert.Equal("Guayaquil", resultado[1].Direccion);
        }

        [Fact]
        public void AddCliente_DeberiaLlamarMetodoAddCliente()
        {
            var cliente = new Cliente
            {
                Codigo = 1,
                Cedula = "1234567890",
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Mail = "juan@example.com",
                Telefono = "0987654321",
                Direccion = "Quito",
                Estado = true
            };

            _mockClienteDAL.Object.addCliente(cliente);

            _mockClienteDAL.Verify(x => x.addCliente(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public void GetClienteById_DeberiaRetornarCliente()
        {
            var cliente = new Cliente { Codigo = 1, Nombres = "Juan" };
            _mockClienteDAL.Setup(x => x.getClienteById(1)).Returns(cliente);

            var resultado = _mockClienteDAL.Object.getClienteById(1);

            Assert.NotNull(resultado);
            Assert.Equal("Juan", resultado.Nombres);
        }

        [Fact]
        public void UpdateCliente_DeberiaLlamarMetodoUpdateCliente()
        {
            var cliente = new Cliente { Codigo = 1, Nombres = "Juan Actualizado" };

            _mockClienteDAL.Object.updateCliente(cliente);

            _mockClienteDAL.Verify(x => x.updateCliente(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public void DeleteCliente_DeberiaLlamarMetodoDeleteCliente()
        {
            _mockClienteDAL.Object.deleteCliente(1);

            _mockClienteDAL.Verify(x => x.deleteCliente(1), Times.Once);
        }
    }
}

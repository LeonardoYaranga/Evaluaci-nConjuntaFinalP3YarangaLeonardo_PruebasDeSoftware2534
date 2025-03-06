Feature: ClienteCrud
    CRUD sobre Clientes
    Para gestionar los datos de los clientes de manera efectiva


@create
Scenario: Crear un nuevo Cliente Valido
    Given voy a la página de Crear en "http://localhost:5051/Cliente/Create"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail          | Telefono   | Direccion | Estado |
        | 1726781402 | Leo     | Perez     | 1990-05-15      | leoP@gmail.com| 0977654321 | Calle A   | true   |
    And envío el formulario de creación
    Then debo ser redirigido a la lista de Clientes en "http://localhost:5051/Cliente"
	And los datos en el ultimo registro deben ser iguales a los ingresados
     | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail          | Telefono   | Direccion | Estado |
        | 1726781402 | Leo     | Perez     | 1990-05-15      | leoP@gmail.com| 0977654321 | Calle A   | true   |

@create
Scenario: Crear un nuevo Cliente con Cedula Invalida
    Given voy a la página de Crear en "http://localhost:5051/Cliente/Create"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail           | Telefono   | Direccion | Estado |
        | 1234567890 | Juan    | Perez     | 1990-05-15      | leoP@gmail.com| 0995879641 | Calle 123 | true   |
    And envío el formulario de creación
    Then debo ver un mensaje de error "Error: La Cedula no es valida."

@update
Scenario: Intentar actualizar un Cliente con Cedula Invalida
    Given hay un cliente existente con ID "3" en "http://localhost:5051/Cliente/Edit/3"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail           | Telefono   | Direccion | Estado |
        | 1234567890 | Juan    | Perez     | 1990-05-15      | juan@example.com| 0995878641 | Calle 123 | true   |
    And envío el formulario de edición
    Then debo ver un mensaje de error "Error: La Cedula no es valida."

@update2
Scenario: Intentar actualizar un usuario con campos vacios
Given hay un cliente existente con ID "3" en "http://localhost:5051/Cliente/Edit/3"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail           | Telefono   | Direccion | Estado |
        |  |     |      |      | |  |  |    |
    And envío el formulario de edición
    Then debo ver un mensaje de error de campo vacio "La cédula es obligatoria."

@updateCorrecto
Scenario: Actualizar un Cliente correctamente
    Given hay un cliente existente con ID "3" en "http://localhost:5051/Cliente/Edit/3"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail            | Telefono   | Direccion   | Estado |
        | 1100848835 | Raul     | Editado     | 1990-12-25      | luis.martinez@mail.com | 0965544332 | Centro de la Ciudad | true   |
    And envío el formulario de edición
    Then debo ser redirigido a la lista de Clientes en "http://localhost:5051/Cliente"
    And los datos en el registro con ID "3" deben ser iguales a los ingresados
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail            | Telefono   | Direccion   | Estado |
        | 1100848835 | Raul     | Editado     | 1990-12-25      | luis.martinez@mail.com | 0965544332 | Centro de la Ciudad | true   |


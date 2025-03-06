Feature: ClienteCrud
    CRUD sobre Clientes
    Para gestionar los datos de los clientes de manera efectiva


@create
Scenario: Crear un nuevo Cliente Valido
    Given voy a la página de Crear en "http://localhost:5051/Cliente/Create"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail          | Telefono   | Direccion | Estado |
        | 1726781402 | Leo     | Perez     | 1990-05-15      | leoP@gmail.com| 0987654321 | Calle A   | true   |
    And envío el formulario de creación
    Then debo ser redirigido a la lista de Clientes en "http://localhost:5051/Cliente"
    And debo ver un mensaje de éxito "Cliente agregado correctamente."

@create
Scenario: Crear un nuevo Cliente con Cedula Invalida
    Given voy a la página de Crear en "http://localhost:5051/Cliente/Create"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail           | Telefono   | Direccion | Estado |
        | 1234567890 | Juan    | Perez     | 1990-05-15      | juan@example.com| 0987654321 | Calle 123 | true   |
    And envío el formulario de creación
    Then debo ver un mensaje de error "Error: La Cedula no es valida."

@update
Scenario: Intentar actualizar un Cliente con Cedula Invalida
    Given hay un cliente existente con ID "34" en "http://localhost:5051/Cliente/Edit/34"
    When ingreso los siguientes detalles del cliente
        | Cedula     | Nombres | Apellidos | FechaNacimiento | Mail           | Telefono   | Direccion | Estado |
        | 1234567890 | Juan    | Perez     | 1990-05-15      | juan@example.com| 0987654321 | Calle 123 | true   |
    And envío el formulario de edición
    Then debo ver un mensaje de error "Error: La Cedula no es valida."

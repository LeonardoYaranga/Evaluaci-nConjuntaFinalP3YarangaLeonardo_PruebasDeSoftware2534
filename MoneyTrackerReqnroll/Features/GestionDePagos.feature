Feature: GestionDePagos

Realización de pruebas de el apartado de gestión de pagos

@GP_001
Scenario: Crear pago tipo ingreso con datos correctos.
	Given Estar en la página principal de la aplicación
	When El usuario llena el apartado del formulario con los datos
		| Nombre | Monto | TipoActividad | Descripción | Estado |
		| Pago1  | 1000  | Ingreso       | Pago de renta | Pendiente |
	And El usuario hace clic en el botón Guardar
	Then El registro se hizo correctamente y se muestra un mensaje de éxito

@GP_002
Scenario: Crear pagos con campos vacios.
	Given Estar en la página principal de la aplicación
	When El usuario llena el apartado del formulario con datos vacios
		| Nombre | Monto | TipoActividad | Descripción | Estado |
		| | | | | |
	Then El sistema no debería habilitar el botón del Guardar el pago

@GP_003
Scenario: Crear pago tipo ingreso con monto negativo
	Given Estar en la página principal de la aplicación
	When El usuario llena el apartado del formulario con los datos
		| Nombre | Monto | TipoActividad | Descripción | Estado |
		| Pago1  | -30  | Ingreso       | Pago de renta | Pendiente |
	Then El sistema deberá mostrar una alerta de que el monto no puede ser negativo

@GP_004
Scenario: Crear pago con nombre de más de 250 caracteres. 
	Given Estar en la página principal de la aplicación
		When El usuario llena el apartado nombre con más de 250 caracteres "G3zv9qL2m8YxFj6rTp0kQs1nH0WlVz5b7SgZk8A3o9yJ1UqH5tLrNpXjKwFg9k8mV2Tq3pO0sY6hCn7dMvFz5G1a1zKbHrYw3J6kZl1qXt5NcB9pV3u1tGz8dQ4b7Y3Kl9xTzJpL"
	Then El sistema deberá mostrar una alerta que indique que el nombre no debe superar un limite de caracteres

@GP_005
Scenario: Editar pago con datos correctos
	Given Estar en la página principal de la aplicación
	And Dar click en el lapiz para editar
	When El usuario modifica el dato nombre con "Comida"
	And Da click en el botón "Guardar"
	Then El sistema deberá mostrar un mensaje de éxito

@GP_006
Scenario: Editar pago con datos vacios
	Given Estar en la página principal de la aplicación
	And Dar click en el lapiz para editar
	When El usuario modifica el dato nombre con datos vacios ""
	Then El sistema deberá mostrar el botón "Guardar" como deshabilitado

@GP_007
Scenario: Eliminar pago existente en el sistema. 
	Given Estar en la página principal de la aplicación
	And Dar click en el ícono de papelera para eliminar
	Then El sistema deberá mostrar un botón de confirmación para eliminar el pago
	When El usuario da click en el botón de confirmación
	Then El usuario deberá ser borrado
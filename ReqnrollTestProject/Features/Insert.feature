Feature: Insert

Insertar un cliente

@tag1
Scenario: Insertar cliente
	Given Llenar los campos en el formulario 
		| Cedula     | Nombres | Apellidos | FechaNacimiento | Mail | Telefono | Direccion | Estado |
		| 1726781402 | Juan    | Perez     | 02-20-2002		 | jp@gmail.com   | 0987654323 | Quito    | 1      |
	When Ingresar registros en la base de datos
		| Cedula     | Nombres | Apellidos | FechaNacimiento | Mail | Telefono | Direccion | Estado |
		| 1726781402 | Juan    | Perez     | 02-20-2002		 | jp@gmail.com   | 0987654323 | Quito    | 1      |
	Then Verificacion de ingreso a la base de datos
		| Cedula     | Nombres | Apellidos | FechaNacimiento | Mail | Telefono | Direccion | Estado |
		| 1726781402 | Juan    | Perez     | 02-20-2002		 | jp@gmail.com   | 0987654323 | Quito    | 1      |
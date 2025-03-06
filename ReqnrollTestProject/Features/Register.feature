Feature: Register

Registro de usuario con exito

@tag1
Scenario: El usuario se registra con exito

    Given El usuario se encuentra en la pagina de inicio.
	When El usuario ingresa un nombre "Leon" y  un correo "leon.y@gmail.com"
    And El usuario hace clic en el botón de Signup
    And Completa el formulario de registro con los datos requeridos
    |title    | nombre          | correo | password | dia       | mes  | anio    | apellido | empresa   | direccion     | pais      | estado | ciudad | zip        | movil |
    | Mr. | Leon | leon.y@gmail.com | 123456 | 10       | September | 2006 | Yaranga | ESPE     | Sangolqui | United States | Rumiñahui | Quito  | 175050 | 0995667373 |
    And Hace clic en el botón de Create Account
    Then El usuario debería ver un mensaje de confirmación Account Created!

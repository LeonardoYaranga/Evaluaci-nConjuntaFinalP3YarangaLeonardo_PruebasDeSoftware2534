Feature: RegistroUsuario

  Pruebas para la funcionalidad de registro de usuarios en la aplicación web

@RU_001
Scenario: Registro de usuario con información válida
  Given El usuario se encuentra en la página principal
  When El usuario hace clic en el enlace "Crea una nueva Cuenta"
  And El usuario llena el formulario con 
    | Nombre | Apellido | Correo                | Contraseña | Confirmación |
    | Diego  | Rivera   | di_rivera78@gmail.com | diego123   | diego123     |
  And El usuario hace clic en el botón "Crear Cuenta"
  Then El sistema muestra un mensaje de éxito "User created successfully"

@RU_002
Scenario: Registro de usuario con correo inválido
  Given El usuario se encuentra en la página principal
  When El usuario hace clic en el enlace "Crea una nueva Cuenta"
  And El usuario llena el formulario con
    | Nombre | Apellido | Correo        | Contraseña | Confirmación |
    | Diego  | Rivera   | invalid-email | diego123   | diego123     |
  And El usuario hace clic en el botón "Crear Cuenta"
  Then El campo de correo muestra una validación HTML "Please include an '@' in the email address"

@RU_003
Scenario: Registro de usuario con contraseñas no compatibles
  Given El usuario se encuentra en la página principal
  When El usuario hace clic en el enlace "Crea una nueva Cuenta"
  And El usuario llena el formulario con
    | Nombre | Apellido | Correo                | Contraseña | Confirmación |
    | Diego  | Rivera   | di_rivera79@gmail.com | diego123   | diego1234    |
  And El usuario hace clic en el botón "Crear Cuenta"
  Then El sistema muestra un mensaje de error "Las contraseñas no coinciden."

@RU_004
Scenario: Registro de usuario con contraseña de menos de 8 caracteres
  Given El usuario se encuentra en la página principal
  When El usuario hace clic en el enlace "Crea una nueva Cuenta"
  And El usuario llena el formulario con
    | Nombre | Apellido | Correo                | Contraseña | Confirmación |
    | Diego  | Rivera   | di_rivera80@gmail.com | diego      | diego        |
  And El usuario hace clic en el botón "Crear Cuenta"
  Then El sistema muestra un mensaje de error "La contraseña debe tener al menos 8 caracteres"
  # Nota: Esto fallará porque no está validado aún

@RU_005
Scenario: Registro de usuario sin información
  Given El usuario se encuentra en la página principal
  When El usuario hace clic en el enlace "Crea una nueva Cuenta"
  And El usuario llena el formulario con
    | Nombre | Apellido | Correo | Contraseña | Confirmación |
    |        |          |        |            |              |
  And El usuario hace clic en el botón "Crear Cuenta"
  Then El campo de nombre muestra una validación HTML "Please fill out this field"

@RU_006
Scenario: Registro de usuario con nombre incorrecto
  Given El usuario se encuentra en la página principal
  When El usuario hace clic en el enlace "Crea una nueva Cuenta"
  And El usuario llena el formulario con
    | Nombre | Apellido | Correo                | Contraseña | Confirmación |
    | @ '' - | Rivera   | di_rivera81@gmail.com | diego123   | diego123     |
  And El usuario hace clic en el botón "Crear Cuenta"
  Then El sistema muestra un mensaje de error "El nombre no es valido"
  # Nota: Esto fallará porque no está validado aún
Feature: IngresoSistema

  Pruebas para la funcionalidad de ingreso al sistema en la aplicación web

@IS_001
Scenario: Ingreso al sistema con correo válido
  Given El usuario se encuentra en la página principal.
  When El usuario ingresa su correo y contraseña.
    | Correo                | Contraseña |
    | di_rivera78@gmail.com | diego123   |
  And El usuario hace clic en el botón "Iniciar Sesión".
  Then El usuario debería ser redirigido al apartado Home con la URL "http://localhost:3000/main/home".

@IS_002
Scenario: Ingreso al sistema con formulario vacío
  Given El usuario se encuentra en la página principal.
  When El usuario ingresa su correo y contraseña.
    | Correo | Contraseña |
    |        |            |
  And El usuario hace clic en el botón "Iniciar Sesión".
  Then El campo de correo muestra una validación HTML "Please fill out this field".

@IS_003
Scenario: Ingreso al sistema con contraseña incorrecta y correo correcto
  Given El usuario se encuentra en la página principal.
  When El usuario ingresa su correo y contraseña.
    | Correo                | Contraseña |
    | di_rivera78@gmail.com | wwwww      |
  And El usuario hace clic en el botón "Iniciar Sesión".
  Then El sistema muestra un mensaje de error "Password is incorrect".
 
@IS_004
Scenario: Ingreso al sistema con correo incorrecto y contraseña correcta
  Given El usuario se encuentra en la página principal.
  When El usuario ingresa su correo y contraseña.
    | Correo            | Contraseña |
    | di_rivera78gmailcom | diego123   |
  And El usuario hace clic en el botón "Iniciar Sesión".
  Then El campo de correo muestra una validación HTML "Please include an '@' in the email address".

@IS_005
Scenario: Ingreso al sistema con correo de más de 100 caracteres
  Given El usuario se encuentra en la página principal.
  When El usuario ingresa su correo y contraseña.
    | Correo                                                                                                      | Contraseña |
    | di_rivera789999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999@gmail.com | diego123   |
  And El usuario hace clic en el botón "Iniciar Sesión".
  Then El sistema muestra un mensaje de error "El correo no puede tener más de 100 caracteres".

@IS_006
Scenario: Ingreso al sistema con correo vacío y contraseña correcta
  Given El usuario se encuentra en la página principal.
  When El usuario ingresa su correo y contraseña.
    | Correo | Contraseña |
    |        | diego123   |
  And El usuario hace clic en el botón "Iniciar Sesión".
  Then El campo de correo muestra una validación HTML "Please fill out this field".

@IS_007
Scenario: Ingreso al sistema con correo correcto y contraseña vacía
  Given El usuario se encuentra en la página principal.
  When El usuario ingresa su correo y contraseña.
    | Correo                | Contraseña |
    | di_rivera78@gmail.com |            |
  And El usuario hace clic en el botón "Iniciar Sesión".
  Then El campo de contraseña muestra una validación HTML "Please fill out this field".
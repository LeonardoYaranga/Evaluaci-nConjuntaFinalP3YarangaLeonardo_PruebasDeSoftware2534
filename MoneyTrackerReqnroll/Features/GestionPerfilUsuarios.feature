Feature: GestionPerfilUsuarios

  Pruebas para la gestión del perfil de usuarios, verificando acciones de edición y eliminación

@GPU_001
Scenario: Verificar que los datos para crear usuario se guarden correctamente en “Mi Perfil”
  Given El usuario se encuentra en la página principal
  When El usuario hace clic en el enlace "Crea una nueva Cuenta"
  And El usuario llena el formulario con 
    | Nombre  | Apellido | Correo              | Contraseña | Confirmación |
    | Lindsay | Ordoñez  | domlin516@gmail.com | 654321     | 654321       |
  And El usuario hace clic en el botón "Crear Cuenta"
  Then El sistema muestra un mensaje de éxito "User created successfully"
  When El usuario hace clic en el enlace "Volver a la página de inicio"
  And El usuario navega a la página de inicio de sesión
  And El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin516@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  Then Los datos ingresados coinciden con los datos mostrados en el perfil
    | Nombre  | Apellido | Correo              |
    | Lindsay | Ordoñez  | domlin516@gmail.com |

@GPU_002
Scenario: Editar el Nombre dejando el campo vacío
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario edita el formulario de perfil con
    | Nombre | Apellido | Correo              |
    |        | Ordoñez  | domlin515@gmail.com |
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El campo de nombre muestra una validación HTML "Please fill out this field"

@GPU_003
Scenario: Editar el Apellido dejando el campo vacío
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario edita el formulario de perfil con
    | Nombre  | Apellido | Correo              |
    | Lindsay |          | domlin515@gmail.com |
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El campo de apellido muestra una validacion HTML "Please fill out this field"

@GPU_004
Scenario: Editar el apellido con caracteres especiales
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario edita el formulario de perfil con
    | Nombre  | Apellido    | Correo              |
    | Lindsay | Ordonez123  | domlin515@gmail.com |
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El campo de apellido muestra una validacion HTML "Please match the requested format."
  
  #no pasara
@GPU_005
Scenario: Editar la sección de Biografía con más de 150 caracteres
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario edita el formulario de perfil con biografía
    | Nombre  | Apellido | Correo              | Biografía                                                                 |
    | Lindsay | Ordoñez  | domlin515@gmail.com | Hola mi nombre es Lindsay Domenique Barrionuevo Ordoñez y esta es mi biografia inicial en Money Tracker, quiero probar este sistema de gestión de mis finanzas. |
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El sistema muestra un mensaje de éxito "Usuario actualizado con éxito"
  #Deberia pasar

@GPU_006
Scenario: Editar la sección de Biografía con más de 500 caracteres
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario edita el formulario de perfil con biografía
    | Nombre  | Apellido | Correo              | Biografía                                                                                                                                                                                                                     |
    | Lindsay | Ordoñez  | domlin515@gmail.com | Hola mi nombre es Lindsay Domenique Barrionuevo Ordoñez y esta es mi biografía inicial en Money Tracker, quiero probar este sistema de gestión de finanzas personales. Tengo 21 años, en unos meses cumpliré 22. Me considero amante de los gatos y de los perros porque son animalitos muy lindos y tiernos. Estoy cursando el Sexto Semestre de Ingeniería en Software y ha sido una carrera muy dura. Esta aplicación es uno de mis proyectos personales realizado junto a mis mejores amigos y compañeros de esta carrera. |
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El sistema muestra un mensaje de error "La biografía no puede exceder los 500 caracteres"

@GPU_007
Scenario: Intentar editar el Correo Electrónico dejando el campo vacío
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario edita el formulario de perfil con
    | Nombre  | Apellido | Correo |
    | Lindsay | Ordoñez  |        |
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El sistema muestra un mensaje de error "El correo electrónico no puede estar vacío"

@GPU_008
Scenario: Subir foto para el perfil en sección “Tu Foto”
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario sube una foto al campo "Tu Foto" con el archivo "ArchivoCarnet.png"
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El sistema muestra un mensaje de éxito "Usuario actualizado con éxito"
  And La foto subida se muestra en la sección "Tu Foto"

@GPU_009
Scenario: Editar el Nombre y Apellido con Datos Válidos
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin515@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario edita el formulario de perfil con
    | Nombre    | Apellido | Correo              |
    | Domenique | Rivera   | domlin515@gmail.com |
  And El usuario hace clic en el botón "Guardar Cambios"
  Then El sistema muestra un mensaje de éxito "Usuario actualizado con éxito"

@GPU_010
Scenario: Eliminar cuenta desde la sección de Mi Perfil
  Given El usuario se encuentra en la página principal
  When El usuario ingresa su correo y contraseña
    | Correo              | Contraseña |
    | domlin514@gmail.com | 654321     |
  And El usuario hace clic en el botón "Iniciar Sesión"
  And El usuario se dirige al apartado de Perfil
  And El usuario hace clic en el botón "Eliminar"
  And El usuario confirma la eliminación haciendo clic en "Eliminar"
  Then El sistema redirige a la página de login con la URL "http://localhost:3000/"
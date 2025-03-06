Feature: GestionNotificaciones

  Como usuario del sistema de gestión financiera,
  Quiero visualizar notificaciones de deudas pendientes próximas a vencer,
  Para estar al tanto de mis obligaciones financieras y evitar retrasos en los pagos.

@GN_001
Scenario: Visualizar la notificación de una deuda que expira un día después
  Given El usuario está en la página principal después de iniciar sesión
  When El usuario llena el formulario de pago con datos válidos y fecha límite del día siguiente
    | Nombre         | Monto | TipoActividad  | Descripción                          | FechaLimite | Estado    |
    | Pagar Genesita | 20    | Deuda a Pagar  | Se le debe 20 a Genesita de la compra | 24/02/2025 | Pendiente |
  And El usuario hace clic en el botón "Guardar" en pago
  And El usuario abre la campana de notificaciones al día siguiente
  Then El sistema debe mostrar una notificación de deuda pendiente con el mensaje "Te quedan 1 día(s) para saldar el pago Pagar Genesita"

@GN_002
Scenario: No visualizar notificaciones de registros antiguos
  Given El usuario está en la página principal después de iniciar sesión
  When El usuario abre la campana de notificaciones
  Then El sistema debe mostrar un mensaje de que no hay notificaciones pendientes si las fechas de pago ya pasaron
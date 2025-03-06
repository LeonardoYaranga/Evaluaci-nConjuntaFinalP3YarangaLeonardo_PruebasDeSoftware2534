Feature: login

 login a la pagina "https://www.automationexercise.com/login"

@tag1
Scenario: El usuario inicia sesión con éxito
	Given El usuario se encuentra en la página de inicio
	When El usuario ingresa su nombre de usuario "leo.y@gmai.com" y contraseña "123456"
	And Hacer clikc en el boton de Login
	Then El usuario debería ser dirigido a la página de inicio

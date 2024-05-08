# Chatroom app



## Funcionalidad del Servidor

### Program.cs (BasicServerFunctionality)

Este archivo contiene el código principal para el servidor de la aplicación de chat. Básicamente, se encarga de escuchar conexiones entrantes de los clientes y mostrar información cuando un cliente se conecta.

### SignUpAndSignIn.cs (ChatroomWithUserIdentification)

Aquí se maneja todo lo relacionado con la creación de cuentas y el inicio de sesión de usuarios. Guarda la información de los usuarios en una base de datos MySQL.

## Interfaz del Cliente

### CreateUser.cs (FormSideOfTheChat)

Este formulario permite a los usuarios crear nuevas cuentas introduciendo un nombre de usuario y una contraseña.

### Form1.cs (FormSideOfTheChat)

Esta es la interfaz principal para que los usuarios inicien sesión en la aplicación. Les pide que ingresen sus credenciales y pueden iniciar sesión. Si no tienen una cuenta, pueden crear una nueva desde aquí.

## Dependencias

- System
- System.Collections.Generic
- System.ComponentModel
- System.Data
- System.Drawing
- System.Linq
- System.Text
- System.Threading.Tasks
- System.Windows.Forms
- ChatroomWithUserIdentification

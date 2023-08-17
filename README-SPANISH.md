# Customer-Management

![Logo de la Aplicación](./Customer_Management.Web/wwwroot/assets/images/customers-logo.png)

## Descripción
Con esta Aplicación usted puede manejar los Clientes y Compañias. Básicamente, cada cliente pertenece a una compañía. Teniendo eso en mente, pudede tener un buen manejo de sus registros y demás.

## Características
- Las princiaples acciones (Crear - Leer - Modificar - Eliminar) para las compañías.
- Las princiaples acciones (Crear - Leer - Modificar - Eliminar) para los clientes (pero cada cliente está vinculado con una compañía).
- Una colección donde todos los clientes de una compañía en especifico se listan.

## Tecnologías implementadas
- **.NET Core 6:** Es la LTS version hasta el momento para .NET core.
- **SQL Server:** Un servidor de Base de Datos para almacenar la data.
- **Entity Framework Core:** Se base en ser un ORM para mapear y manejar las entidades de la Base de Datos.
- **NToastNotify:** Un paquete que permite a los Desarrolladores mostrar una notificación de tipo Toast cuando sea necesario.

## Técnicas y buenas practicas implementadas
- Principios de POO
- Principios SOLID
- Patrones de Diseño
- Code First
- Logging
- Notificaciones de tipo Toast

## Estructura del proyecto
Básicamente este es un Proyecto Web que se conecta con una Base de Datos usando SQL Server. El proyecto está diseñado con una **Estructura Modelo-Vista-Controlador (MVC)** pero se implementan los patrones **DAO** y **DTO**.

_Estos son las principales carpetas para la funcionalidad de la aplicación._
- **Customer_Management.Web**
  - **Classes:** Almacena clases utiles que pueden ser usadas en la aplicación, pero estos elementos no representan entidades de la Base de Datos.
    
  - **Contracts:** Toma los contratos (Interfaces) que clases especificas tienen que implementar.
  - **Controllers:** Almacena los elementos que manejan la comunicación entre los objectos DAO y las Vistas, y también, las peticiones de las vistas.
  - **DAO:** Almacena las clases que están definidas para ser usadas para la comunicación con los **Modelos (Models)**. En resumen, las instancias de estas clases pueden acceder a la data almacenada en la Base de Datos usando el **DbContext**.
  - **DTO:** Almacena los elementos que las **Vistas (Views)** reciben.
  - **Data:** Almacena el **DbContext** de la **Aplicación**.
  - **Helpers:** Define clases estáticas para que los Desarrolladores puedan usarlas para manejar algunos escenarios. Por ejemplo: Una clase que muestra una **Notificación** cuando una acción ha sido realizada.
  - **Migrations:** Una carpeta que almacena las migraciones creadas en el **Proceso de Desarrollo**.
  - **Models:** Contiene las entidades que son definidas en la Base de Datos.
  - **StaticValues:** Define algunos **valores constantes** dentro de **clases estáticas**. Básicamente, estos son valores que serán usados en toda la aplicación.
  - **Views:** Contiene las diferentes vistas y páginas usadas por las entidades (Compañías y Clientes).
  - **wwwroot:** Almacena algunas carpetas y archivos para las funcionalidades JS, estilos CSS e imágenes usadas en el proyecto.

## ¿Cómo ejecutar el proyecto localmente?

  1. Crear un Fork de la aplicación para tener acceso desde su Repositorio.
  2. Vaya a la carpeta **Database** ubicada en la raíz del Repositorio, luego abra el archivo **DDL.sql** y ejecutelo en un SGBD que soporte **Conexiones para SQL Server**. Por ejemplo, **SQL Server Management Studio (SSMS)**.
  3. Después de eso, dirijase al archivo **appsettings.json** y modifique el valor de la Cadena de Conexión llamada **DbCustomerManagement**.
  4. Puede abrir el proyecto usando **Visual Studio** y ejecutar la aplicación con dicha aplicación.
  5. Una vez que la aplicación se esté ejecutando, puede dirigirse a su navegador preferido y abrir la siguiente URL _http://localhost:5014_

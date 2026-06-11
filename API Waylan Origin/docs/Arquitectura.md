# Documentacion de arquitectura

## Estilo de arquitecura
Implementaremos una arquitectura por capas con el fin de tener un codigo escalable y mantenible, separamos el frontend del backend para trabajar simultaneamente 
y agilizar el proceso de desarrollo.

## Pila tecnologica
**lenguaje**  C# (.NET Core)
**ORM** Entity framework
**base de datos** MySql
**Nube** Azure
**Seguridad** JWT

## Carpetas de la arquitectura
Para mantener el proyecto ordenado y fácil de entender, el código de nuestra API estará dividido en las siguientes carpetas principales:

* **Controllers:** Son los puntos de entrada. Reciben las peticiones del frontend (las URLs) y devuelven las respuestas (éxito o error). Aquí no va código complejo, solo dirigen el tráfico.
* **Interfaces:** Son los contratos que definen qué acciones pueden hacer nuestros servicios. Son clave para la inyección de dependencias en .NET.
* **Services:** Aquí vive la "lógica de negocio" del e-commerce. Por ejemplo, aquí validamos que haya stock antes de una compra, o calculamos el total del carrito.
* **Models (o Entities):** Son las clases que representan exactamente las tablas de nuestra base de datos en MySQL (Ej: `Producto.cs`, `Usuario.cs`, `Pedido.cs`).
* **DTOs (Data Transfer Objects):** Son clases que usamos como "filtros" para enviar o recibir datos de forma segura. Por ejemplo, al consultar un usuario, el DTO se asegura de enviar el nombre y el correo al frontend, pero oculta la contraseña.
* **Data:** Aquí va la configuración de Entity Framework (el archivo `DbContext`) y todo lo relacionado con la conexión directa a la base de datos.

## Arquitectura en la Nube (Azure)
Para alojar la plataforma de Waylan Origin utilizaremos Microsoft Azure, dividiendo la infraestructura en tres servicios principales:

* **Azure App Service (Para la API):** Es el servicio que mantendrá nuestra API de .NET Core corriendo en internet las 24 horas. Se encarga de recibir las peticiones del frontend y ejecutar la lógica del negocio de forma segura sin que tengamos que configurar servidores Linux desde cero.
* **Azure Database for MySQL (Para los Datos):** Es el motor donde vivirá nuestra base de datos. Elegimos la opción "Flexible Server" en su capa económica porque Azure se encarga automáticamente del mantenimiento y de sacar copias de seguridad diarias por si algo falla.
* **Azure Blob Storage (Para los Archivos):** Es un disco duro virtual en la nube sumamente económico. Aquí guardaremos de forma organizada todas las fotos de los productos, imágenes de los aliados y videos de los productores. No se guardan en el servidor de la API para evitar saturarlo.

**¿Cómo funciona el flujo?**
Cuando un cliente entra a la tienda, el Frontend le pide información a la API (App Service). La API va a MySQL por los textos y precios, y toma las fotos directamente desde el Blob Storage para mostrárselas al usuario.

## Paquetes instalados
*Microsoft.EntityFrameworkCore
*Pomelo.EntityFrameworkCore.MySql
*Microsoft.EntityFrameworkCore.Tools (Incluye Relational y Design automáticamente en la mayoría de casos)
*Microsoft.AspNetCore.Authentication.JwtBearer
*Swashbuckle.AspNetCore (o Microsoft.AspNetCore.OpenApi)
*BCrypt.Net-Next (Última estable)
*AutoMapper





# Documentacion para Frontend

## Crear y actualizar la base de datos
El proyecto utiliza Entity Framework Core (Code First). No necesitas instalar herramientas de administración ni ejecutar scripts SQL manuales; el backend creará las tablas automáticamente con el siguiente comando:

**dotnet ef database update**.

se debe crear el appsettings.json con la conexion a la base de datos local y la clave JWT

## Ejecutar el proyecto
Para restaurar los paquetes de NuGet automáticamente y levantar el servidor web de desarrollo, ejecuta:

**dotnet run**

despues de eso ya puedes ejecutar el proyecto o abrirlo en https://localhost:7123/swagger/index.html (Verifica el puerto exacto en la salida de tu consola).

## Especificaciones tecnicas y reglas de negocio
Para evitar comportamientos inesperados o errores de servidor (400 BadRequest), es vital que la interfaz de usuario respete las siguientes reglas de datos construidas en el backend:.

*A. Carga de Imágenes (Crear y Actualizar Productos)*

**Tipo de Contenido Obligatorio:** Los endpoints de creación (POST) y actualización (PUT) de productos NO reciben un JSON convencional. Debes enviar la petición utilizando un formulario de tipo multipart/form-data.

**El Campo Imagen:** La propiedad destinada a la foto del producto se llama exactamente Imagen. Debe ser un archivo físico real (Blob/File enviado desde un <input type="file">).

**Comportamiento en el Update:** Al actualizar un producto, si el usuario no modifica la foto, simplemente deja el campo Imagen como null o no lo envíes; el backend mantendrá la imagen que ya existía en el disco de manera segura.

*B. Vinculación de Notas (Relación Muchos a Muchos)*

**Formato de Envío:** Para asignar notas a un producto, debes enviar una lista de enteros con los IDs correspondientes. Ejemplo en el formulario: IdNotas = [1, 2, 5].

**Sistema Anti-Fraude (Validación de IDs):** El backend cuenta con un detector estricto que limpia IDs duplicados enviados por error y compara la cantidad de notas solicitadas contra las existentes en la base de datos.

**Regla de Oro:** Todos los IDs que envíes en la lista deben existir previamente. Si el usuario intenta inyectar un ID inexistente, inventado o eliminado (por ejemplo, enviar [1, 999] donde el 999 no existe), el backend frenará en seco la operación y arrojará un error de validación.

##
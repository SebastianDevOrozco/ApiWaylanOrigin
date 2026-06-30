
### 🔐 Módulo 1: Seguridad y Usuarios (`UsuariosController`)

| Método | URL (Endpoint) | Descripción | Permisos |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/usuarios/login` | Recibe email y contraseña. Devuelve el Token JWT. | Público |
| **POST** | `/api/usuarios/registro` | Crea un nuevo cliente en la base de datos. | Público |
| **GET** | `/api/usuarios/perfil` | Devuelve los datos del usuario que inició sesión. | Cliente / Admin |
| **GET** | `/api/usuarios` | Lista todos los usuarios registrados en el sistema. | Solo Admin |
| **DELETE**| `/api/usuarios/{id}` | Desactiva a un usuario (Borrado lógico `Activo = false`). | Solo Admin |
| **PATCH**| `/api/usuarios/{id}/cambiar-estado` | Cambia el estado del usuario | Solo Admin|

### 📦 Módulo 2: Catálogo e Inventario (`ProductosController` y `CategoriasController`)

| Método | URL (Endpoint) | Descripción | Permisos |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/categorias` | Crea una categoria nueva. | Solo Admin |
| **PUT** | `/api/categorias/{id}` | Actualiza datos de una categoria | Solo Admin |
| **DELETE**| `/api/categorias/{id}` | Oculta una categoria (Borrado lógico `Activo = false`). | Solo Admin |
| **GET** | `/api/categorias` | Lista todas las categorías disponibles. | Sola Admin |
| **GET** | `/api/categorias/activas` | Lista todas las categorías disponibles filtradas(activo = true). | Público |
| **PATCH**| `/api/categorias/{id}/cambiar-estado` | Cambia el estado de la categoria | Solo Admin|
| **GET** | `/api/productos/activos` | Lista todos los productos (solo los `Activo = true`). | Público |
| **GET** | `/api/productos/` | Lista todos los productos | Solo Admin |
| **GET** | `/api/productos/{id}` | Muestra el detalle de un solo producto. | Público |
| **POST** | `/api/productos` | Crea un producto nuevo. | Solo Admin |
| **PUT** | `/api/productos/{id}` | Actualiza datos de un producto (precio, stock, etc.). | Solo Admin |
| **DELETE**| `/api/productos/{id}` | Oculta un producto (Borrado lógico `Activo = false`). | Solo Admin |
| **PATCH**| `/api/productos/{id}/cambiar-estado` | Cambia el estado del producto | Solo Admin|

### 🛒 Módulo 3: Ventas (`PedidosController`)

| Método | URL (Endpoint) | Descripción | Permisos |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/pedidos` | Recibe el carrito del cliente y crea el pedido. | Cliente |
| **GET** | `/api/pedidos/mis-pedidos` | Muestra el historial de compras del usuario logueado. | Cliente |
| **GET** | `/api/pedidos` | Lista **todos** los pedidos de la tienda. | Solo Admin |
| **GET** | `/api/pedidos/{codigo}` | Trae el detalle exacto de una factura (ítems, precios). | Admin |
| **PUT** | `/api/pedidos/{codigo}/estado` | Cambia el estado (ej: de "Pendiente" a "Enviado"). | Solo Admin |
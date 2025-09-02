
***************************************************
*** 🏦 BANCO DEVSU - README COMPLETO 🏦 ***
***************************************************

¡Hola! 👋 Bienvenido al proyecto del Banco Devsu. Esta es una guía completa para que entiendas su estructura y puedas ponerlo en marcha rápidamente.

---
## 🎯 ¿QUÉ HACE ESTE PROYECTO?
---
Es una aplicación bancaria que permite:
* 👤 **Gestionar Clientes:** Crear, editar, eliminar y consultar clientes.
* 💳 **Administrar Cuentas:** Abrir cuentas de ahorro o corrientes para los clientes.
* 💸 **Registrar Movimientos:** Realizar depósitos y retiros en las cuentas.
* 📊 **Generar Reportes:** Consultar estados de cuenta detallados por cliente y fechas.

---
## 💻 PILA TECNOLÓGICA (TECH STACK)
---
El proyecto se divide en dos grandes componentes:

* **🚀 Backend (.NET 8):**
    * **Lenguaje:** C#
    * **Framework:** ASP.NET Core 8
    * **Base de Datos:** MySQL (con Entity Framework Core como ORM)
    * **Contenerización:** Docker 🐳

* **🎨 Frontend (Angular 17):**
    * **Lenguaje:** TypeScript
    * **Framework:** Angular
    * **Estilos:** CSS moderno y responsivo

---
## 🏗️ ARQUITECTURA Y PATRONES DE DISEÑO
---
El proyecto sigue principios de software robustos para ser escalable y fácil de mantener.

### >> Backend (Arquitectura Limpia)

El backend está construido usando **Arquitectura Limpia (Clean Architecture)**. Esto significa que el código se organiza en capas, donde las reglas de negocio y la lógica principal no dependen de detalles externos como la base de datos o la interfaz de usuario.

* **🏛️ Domain (Dominio):** Es el núcleo de la aplicación.
    * **Entidades:** (`Cliente`, `Cuenta`, `Movimiento`): Representan los objetos de negocio puros.
    * **Interfaces de Repositorios:** (`IClienteRepository`, etc.): Definen los "contratos" de lo que se puede hacer con los datos, sin saber *cómo* se hace.

* **🧠 Application (Aplicación):** Contiene la lógica de negocio y orquesta las operaciones.
    * **Patrón de Servicio (Service Pattern):** Los `Services` (`ClienteService`, etc.) coordinan los pasos para cumplir una solicitud (ej. "crear un cliente"). Usan los repositorios para acceder a los datos.
    * **DTOs (Data Transfer Objects):** Objetos planos (`ClienteDto`, etc.) que se usan para enviar y recibir datos desde y hacia la API, evitando exponer las entidades del dominio directamente.
    * **Mapeo:** Se usa `AutoMapper` para convertir fácilmente entre Entidades y DTOs.

* **🔌 Infrastructure (Infraestructura):** Implementa los detalles técnicos.
    * **Patrón Repositorio (Repository Pattern):** Los `Repositories` (`ClienteRepository`, etc.) contienen la lógica para acceder a la base de datos usando Entity Framework. Cumplen con los contratos definidos en el Dominio.
    * **Patrón Unidad de Trabajo (Unit of Work):** Asegura que múltiples operaciones (como crear una cuenta y su primer movimiento) se realicen en una única transacción. Si algo falla, todo se revierte (rollback).

* **📡 Presentation (API):** La capa más externa.
    * **Controladores API REST:** Exponen los endpoints (ej. `GET /api/clientes`) que el frontend consume. Reciben DTOs y llaman a los servicios de la capa de aplicación.

### >> Frontend (Arquitectura por Capas/Módulos)

El frontend está estructurado para separar responsabilidades y facilitar la reutilización de código.

* **`core`:** Contiene la lógica transversal de la aplicación.
    * **Patrón de Servicio (Service Pattern):** Los `Services` (`cliente.service.ts`, etc.) se encargan de hacer las llamadas HTTP al backend y manejar los datos.
    * **Modelos/DTOs:** Interfaces TypeScript que definen la estructura de los datos que se reciben de la API.

* **`presentation`:** Contiene todo lo visible para el usuario.
    * **Arquitectura Basada en Componentes:** La UI se construye a partir de pequeños componentes reutilizables (`header`, `footer`, `sidebar`).
    * **Componentes de Página:** (`clientes.component`, `cuentas.component`): Son los componentes principales para cada pantalla de la aplicación.

* **`shared`:** Módulos y componentes que se pueden reutilizar en cualquier parte de la aplicación (ej. notificaciones `toast`).

---
## 🚀 CÓMO EJECUTAR EL PROYECTO
---
### Método 1: Con Docker (Recomendado) 🐳

Esta es la forma más fácil, ya que Docker gestiona la base de datos y el backend por ti.

1.  **Clona el repositorio.**
2.  **Ve a la carpeta del Backend:**
    ```
    cd Backend/BancoAPI
    ```
3.  **Levanta los contenedores:**
    ```
    docker-compose up --build
    ```
    Esto creará y ejecutará el backend y la base de datos. La API estará en `http://localhost:5256`.

4.  **Abre otra terminal y ve a la carpeta del Frontend:**
    ```
    cd Frontend/banco-devsu
    ```
5.  **Instala las dependencias y ejecuta:**
    ```
    npm install
    npm start
    ```
    La aplicación web estará disponible en `http://localhost:4200`.

### Método 2: Manualmente

#### Backend
1.  Asegúrate de tener una instancia de MySQL corriendo.
2.  Actualiza la cadena de conexión en `Backend/BancoAPI/BancoAPI/appsettings.json`.
3.  Abre el proyecto con tu IDE (Visual Studio, Rider) o usa la terminal:
    ```bash
    cd Backend/BancoAPI/BancoAPI
    dotnet restore
    dotnet ef database update  # Aplica las migraciones a tu BD
    dotnet run
    ```

#### Frontend
1.  Navega a la carpeta del frontend:
    ```bash
    cd Frontend/banco-devsu
    ```
2.  Instala dependencias y ejecuta:
    ```
    npm install
    npm start
    ```

---
## 🧪 PRUEBAS DE API (POSTMAN)
---
Para probar los endpoints del backend directamente, puedes usar la colección de Postman que se encuentra en la carpeta `Backend/BancoAPI/Postman`. ¡Solo impórtala y empieza a jugar!


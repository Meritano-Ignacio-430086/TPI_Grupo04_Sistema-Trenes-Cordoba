# Trabajo Práctico Grupal Integrador : Trenes

## Problemática a solucionar
Nuestro proyecto busca actualizar y optimizar el consumo de los trenes como servicio de transporte público en Córdoba.
Buscamos realizarlo por medio de un sistema actualizado y confiable de compra de tickets para los usuarios y de gestión de los mantenimientos para la empresa.

## Como solucionarla mediante la programación
Permitiremos a los consumidores y a los administradores crear usuarios con distintos permisos que permitan:

## Funciones
### Tickets
- Dar de alta un Ticket.
- Revisar los tickets creados mediante el dni del cliente.

### Loggin:
- Iniciar sesión (sistema de permisos por rol).

### Mantenimientos:
- Dar de alta un mantenimiento con un mecánico a cargo.
- Finalizar un mantenimiento (solo administrador o mecánico supervisor).
- Consultar los mantenimientos llevados a cabo.

## Tecnologías utilizadas

### Backend : 
- C#, ASP.NET Core Web Api, .NET 8
- LINQ, Entity Framework Core, EF Core Power Tools.
- SQL server.

### Frontend: 
- HTML, CSS, JavaScript 
- [Bootstrap](https://getbootstrap.com/) y Frameworks de JS([Chart](https://www.chartjs.org/))

### Arquitectura utilizada:
- Arquitectura en capas (Controller, Service, Repository)
- Patrón Repository, DTOs e inyección de dependencias

## Ramas
- `main`: versión final, solo se actualiza para la entrega.
- `TPI_Grupo04_Sistema-Trenes-Cordoba`: rama de integración del equipo.
- `Nombre-Individual`: rama de trabajo de cada integrante; se integra a la rama 2.

## Integrantes:
- Meritano, Ignacio.
- Dre, Juan.
- Berrotaran, Francisco.
- Lonzari, Brian.
- Murua, Marcos.
- Caccioppoli, Victorio.

## Ejecución
1. Instalar .NET 8 SDK y SQL Server.
2. Configurar `ConnectionStrings:DefaultConnection` en `appsettings.json`.
3. Ejecutar el script de la base de datos.
4. `dotnet run` y abrir el archivo `index.html`.

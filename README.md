# Laboratorio 10 Hotel y reservas

Aplicacion ASP.NET Core MVC desarrollada con Entity Framework Core Database First para gestionar habitaciones, disponibilidad y reservas de un hotel.

## Requisitos

- .NET SDK 10
- SQL Server o SQL Server Express
- Entity Framework Core Tools

## Puesta en marcha

1. Ejecutar `sql/HotelDB.sql` en SQL Server.
2. Ejecutar `sql/Semana10_Observaciones.sql` para agregar el campo `Observaciones`.
3. Ajustar la cadena `HotelDB` de `Hotel.Web/appsettings.json` si la instancia local es distinta.
4. Restaurar y ejecutar la aplicacion:

```powershell
dotnet restore Hotel.Web/Hotel.Web.csproj
dotnet run --project Hotel.Web/Hotel.Web.csproj
```

## Comando de scaffold

```powershell
dotnet ef dbcontext scaffold "Server=localhost\\SQLEXPRESS;Database=HotelDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context HotelContext --force
```

La aplicacion conserva los modelos generados por scaffold, registra `HotelContext` y los repositorios mediante inyeccion de dependencias, y concentra las consultas LINQ asincronas en la carpeta `Repositorios`.

## Funcionalidades

- Listado, detalle, creacion, edicion y baja logica de habitaciones.
- Filtro por tipo de habitacion y validacion de numeros duplicados.
- Consulta de disponibilidad por rango de fechas.
- Registro de reservas con huesped, habitacion, fechas y observaciones.
- Validacion de cruces de fechas mediante LINQ.
- Listado, filtro y cancelacion logica de reservas.

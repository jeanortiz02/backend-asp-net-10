# Backend

API REST desarrollada con ASP.NET Core para practicar controladores, inyección de dependencias, servicios con distintos ciclos de vida, consumo de APIs externas, validación con FluentValidation, mapeo con AutoMapper y persistencia con Entity Framework Core sobre SQL Server.

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core SQL Server
- FluentValidation
- AutoMapper
- Swagger / OpenAPI
- HttpClient

## Estructura del proyecto

```text
Backend/
├── AutoMappers/       # Perfiles de AutoMapper
├── Controllers/       # Endpoints HTTP de la API
├── Dtos/              # Objetos de transferencia de datos
├── Migrations/        # Migraciones de Entity Framework Core
├── Models/            # Entidades y DbContext
├── Repository/        # Abstracciones y repositorios de datos
├── Services/          # Servicios e interfaces de negocio
├── Validators/        # Validaciones con FluentValidation
├── Program.cs         # Configuración principal de la aplicación
├── Backend.csproj     # Dependencias y versión de .NET
└── appsettings.json   # Configuración de conexión y servicios externos
```

## Requisitos

- .NET SDK 10
- SQL Server disponible en `localhost,1433`
- Herramienta de Entity Framework Core:

```bash
dotnet tool install --global dotnet-ef
```

Si ya la tienes instalada:

```bash
dotnet tool update --global dotnet-ef
```

## Configuración

La cadena de conexión se define en `appsettings.json`:

```json
"ConnectionStrings": {
  "StoreConnection": "Server=localhost,1433;Database=Store;User Id=sa;Password=MyStrongPass123;TrustServerCertificate=True;Encrypt=False;"
}
```

También se configura la URL base usada por `PostsService`:

```json
"baseUrlPost": "https://jsonplaceholder.typicode.com/posts"
```

Para levantar SQL Server localmente con Docker:

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=MyStrongPass123" \
  -p 1433:1433 \
  --name sqlserver-store \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

> Las credenciales incluidas son para desarrollo local. En producción conviene usar variables de entorno, user secrets o un gestor de secretos.

## Instalación y ejecución

Restaura dependencias:

```bash
dotnet restore
```

Aplica las migraciones:

```bash
dotnet ef database update
```

Ejecuta la API:

```bash
dotnet run
```

Según `Properties/launchSettings.json`, la API queda disponible en:

- HTTP: `http://localhost:5069`
- HTTPS: `https://localhost:7167`

En ambiente de desarrollo, Swagger está disponible en:

```text
https://localhost:7167/swagger
```

## Endpoints principales

### Beer

CRUD de cervezas persistido en SQL Server. El controlador usa `ICommomService<BeerDto, BeerInsertDto, BeerUpdateDto>`, `BeerService`, `IRepository<Beer>` y `BeerRepository`.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/Beer` | Lista todas las cervezas |
| GET | `/api/Beer/{id}` | Obtiene una cerveza por id |
| POST | `/api/Beer` | Crea una cerveza |
| PUT | `/api/Beer/{id}` | Actualiza una cerveza |
| DELETE | `/api/Beer/{id}` | Elimina una cerveza |

Ejemplo para crear una cerveza:

```json
{
  "name": "Presidente",
  "brandID": 1,
  "alcohol": 5.0
}
```

Ejemplo para actualizar una cerveza:

```json
{
  "id": 1,
  "name": "Presidente Light",
  "brandID": 1,
  "alcohol": 4.3
}
```

Validaciones principales:

- `name` es obligatorio.
- `name` debe tener entre 2 y 20 caracteres.
- `brandID` debe ser mayor que 0.
- `alcohol` debe ser mayor que 0.
- No puede existir otra cerveza con el mismo nombre.

### People

Endpoints de ejemplo que usan una lista en memoria.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/People/all` | Lista todas las personas |
| GET | `/api/People/{id}` | Obtiene una persona por id |
| GET | `/api/People/search/{search}` | Busca personas por nombre |
| POST | `/api/People` | Agrega una persona |

Ejemplo para agregar una persona:

```json
{
  "id": 4,
  "name": "María",
  "birthday": "2001-05-10T00:00:00"
}
```

> `PeopleController` usa datos en memoria, por lo que los cambios se pierden al reiniciar la aplicación.

### Posts

Consume datos externos desde JSONPlaceholder usando `HttpClient`.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/Posts` | Lista posts externos |

### Random

Endpoint de demostración para comparar ciclos de vida de servicios inyectados con servicios keyed.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/Random` | Retorna valores generados por servicios Singleton, Scoped y Transient |

### Operation

Endpoints de ejemplo para operaciones aritméticas.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/Operation?a=10&b=5` | Suma `a + b` |
| POST | `/api/Operation` | Resta `A - B` usando el cuerpo de la solicitud |
| PUT | `/api/Operation?a=10&b=5` | Multiplica `a * b` |
| DELETE | `/api/Operation?a=10&b=5` | Divide `a / b` |

Ejemplo para `POST /api/Operation`:

```json
{
  "a": 10,
  "b": 5
}
```

### Some

Endpoints de ejemplo para comparar ejecución síncrona y asíncrona.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/Some/sync` | Ejecuta tareas simuladas de forma síncrona |
| GET | `/api/Some/async` | Ejecuta tareas simuladas de forma asíncrona |

## Modelo de datos

El contexto `StoreContext` expone:

- `Beers`
- `Brands`

Relaciones principales:

- Una cerveza (`Beer`) pertenece a una marca (`Brand`).
- `Beer.BrandID` es llave foránea hacia `Brand.BrandID`.
- `Beer.Alcohol` se almacena como `decimal(18,2)`.

## Mapeo de datos

`AutoMappers/MappingProfile.cs` define los mapeos:

- `BeerInsertDto` -> `Beer`
- `BeerUpdateDto` -> `Beer`
- `Beer` -> `BeerDto`

En el mapeo de salida, `Beer.BeerId` se expone como `BeerDto.Id`.

## Migraciones

El proyecto incluye estas migraciones de Entity Framework Core:

- `InitDb`
- `AlcoholInBeer`

Para crear una nueva migración:

```bash
dotnet ef migrations add NombreDeLaMigracion
```

Para aplicar migraciones pendientes:

```bash
dotnet ef database update
```

## Notas

- `PostsController` requiere acceso a internet para consultar JSONPlaceholder.
- Swagger solo se habilita cuando `ASPNETCORE_ENVIRONMENT` es `Development`.
- `Backend.http` contiene una solicitud de ejemplo, pero puede requerir actualización si se agregan nuevos endpoints.

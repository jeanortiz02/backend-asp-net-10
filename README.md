# Backend

API REST desarrollada con ASP.NET Core para practicar controladores, inyección de dependencias, consumo de servicios HTTP externos, validación con FluentValidation y persistencia con Entity Framework Core sobre SQL Server.

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- FluentValidation
- Swagger / OpenAPI
- HttpClient

## Estructura del proyecto

```text
Backend/
├── Controllers/       # Endpoints HTTP de la API
├── Dtos/              # Objetos de transferencia de datos
├── Migrations/        # Migraciones de Entity Framework Core
├── Models/            # Entidades y DbContext
├── Services/          # Servicios e interfaces
├── Validators/        # Validaciones con FluentValidation
├── Program.cs         # Configuración principal de la app
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

La cadena de conexión se encuentra en `appsettings.json`:

```json
"ConnectionStrings": {
  "StoreConnection": "Server=localhost,1433;Database=Store;User Id=sa;Password=MyStrongPass123;TrustServerCertificate=True;Encrypt=False;"
}
```

También se configura la URL base usada por el servicio de posts:

```json
"baseUrlPost": "https://jsonplaceholder.typicode.com/posts"
```

Para ejecutar SQL Server localmente con Docker puedes usar:

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=MyStrongPass123" \
  -p 1433:1433 \
  --name sqlserver-store \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

## Instalación

Restaura las dependencias:

```bash
dotnet restore
```

Aplica las migraciones a la base de datos:

```bash
dotnet ef database update
```

Ejecuta el proyecto:

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

CRUD de cervezas persistido en SQL Server.

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

> `name` es obligatorio y se valida con FluentValidation.

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

### Posts

Consume datos externos desde JSONPlaceholder usando `HttpClient`.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/Posts` | Lista posts externos |

### Random

Endpoint de demostración para comparar ciclos de vida de servicios inyectados.

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/Random` | Retorna valores generados por servicios Singleton, Scoped y Transient |

### Operation

Endpoints de ejemplo para operaciones aritmeticas.

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

- Una cerveza (`Beer`) pertenece a una marca (`Brand`)
- `Beer.BrandID` es llave foránea hacia `Brand.BrandID`

## Migraciones incluidas

El proyecto ya incluye migraciones de Entity Framework Core:

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

## Validaciones

Actualmente existe validación para `BeerInsertDto`:

- `Name` no puede estar vacío.

## Notas

- Las credenciales de SQL Server están en `appsettings.json` para desarrollo local. En un entorno real conviene moverlas a variables de entorno, user secrets o un gestor de secretos.
- `PeopleController` usa datos en memoria, por lo que los cambios no se persisten al reiniciar la aplicación.
- `PostsController` depende de acceso a internet para consultar JSONPlaceholder.

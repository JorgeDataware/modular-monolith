# Comunicación entre módulos con MediatR

Cómo se comunican los módulos de este monolito modular, por qué está diseñado así y qué hace
cada archivo, línea por línea.

- [1. El problema que resuelve](#1-el-problema-que-resuelve)
- [2. Dónde vive cada cosa](#2-dónde-vive-cada-cosa)
- [3. Fase de arranque: el registro](#3-fase-de-arranque-el-registro)
- [4. La cadena de llamadas: request/response](#4-la-cadena-de-llamadas-requestresponse)
- [5. La otra mitad: notificaciones](#5-la-otra-mitad-notificaciones)
- [6. Resumen de artefactos](#6-resumen-de-artefactos)
- [7. La licencia de MediatR](#7-la-licencia-de-mediatr)
- [8. Receta: agregar un mensaje nuevo](#8-receta-agregar-un-mensaje-nuevo)
- [9. Decisiones tomadas y pendientes](#9-decisiones-tomadas-y-pendientes)

---

## 1. El problema que resuelve

El patrón mediator sustituye una llamada directa por una llamada indirecta a través de un
intermediario.

**Sin mediator**, para validar que una película existe antes de agregarla al carrito,
`CartMovieService` tendría que escribir esto:

```csharp
private readonly IMovieService _movieService;   // ← tipo que vive en Movies.Module
var movie = await _movieService.GetMovieByIdAsync(movieId, ct);
```

Para que ese código compile, `Users.Module.csproj` **necesita** una referencia a
`CP.Portal.Movies.Module.csproj`. Y ahí muere la modularidad: Users pasa a ver los repositorios,
el `DbContext` y las entidades de Movies. Nada impediría que mañana alguien escribiera una
consulta EF contra las tablas de películas desde el carrito.

**Con mediator**:

```csharp
var movie = await _sender.Send(new GetMovieSummaryQuery(movieId), ct);
```

Users solo conoce `GetMovieSummaryQuery`, un `record` de una línea que vive en el proyecto de
contratos. **No sabe que existe una clase que atiende esa consulta, ni en qué ensamblado está,
ni cómo se llama.** Ese conocimiento lo tiene únicamente el contenedor de DI, en runtime.

La analogía útil: `ISender` es un **diccionario** donde la *clave es el tipo del mensaje* y el
*valor es el handler*. `Send` hace la búsqueda.

---

## 2. Dónde vive cada cosa

```
app/backend/
├── CP.Contracts/src/Core.Contracts/          ← SHARED KERNEL (primitivas técnicas)
│   ├── Abstractions/                            Result, Error, ApiResponse
│   └── Messaging/
│       ├── ModuleMediatorExtensions.cs          registro del mediador por módulo
│       └── Behaviors/
│           └── LoggingBehavior.cs               middleware de mensajes
│
├── Movies.Module/src/
│   ├── Movies.Module.Contracts/              ← API PÚBLICA de Movies (sin lógica)
│   │   ├── Queries/GetMovieSummaryQuery.cs
│   │   ├── Events/MoviePriceChangedNotification.cs
│   │   ├── Dtos/MovieSummary.cs
│   │   └── Errors/MoviesContractErrors.cs
│   └── CP.Portal.Movies.Module/              ← IMPLEMENTACIÓN de Movies
│       └── Application/Integrations/            handlers de sus contratos públicos
│           └── GetMovieSummaryQueryHandler.cs
│
└── Users.Module/src/Users.Module/            ← IMPLEMENTACIÓN de Users
    └── Application/Integrations/                handlers de contratos AJENOS
        └── MoviePriceChangedNotificationHandler.cs
```

### Dirección de las referencias

Esto es lo que hace cumplir la modularidad, y el compilador lo verifica por ti:

```
Users.Module             →  Movies.Module.Contracts    (consume)
CP.Portal.Movies.Module  →  Movies.Module.Contracts    (implementa lo suyo)
```

Nunca `Users.Module → CP.Portal.Movies.Module`. Los proyectos `*.Contracts` son ensamblados hoja
(su única dependencia es `MediatR.Contracts` + el shared kernel), así que **no puede haber
dependencias circulares por construcción**.

### Por qué los contratos NO van en `Core.Contracts`

Hay que distinguir dos cosas que es tentador juntar:

| | Shared Kernel (`Core.Contracts`) | Integration Contracts (`<Módulo>.Contracts`) |
|---|---|---|
| Contenido | `Result`, `Error`, `ApiResponse`, behaviors | `GetMovieSummaryQuery`, `MovieSummary` |
| Naturaleza | Primitivas técnicas, sin dominio | Dominio, con nombres de negocio |
| Dueño | Todos / nadie | Un módulo específico |

Poner los mensajes de integración en `Core.Contracts` trae tres problemas:

1. **Se pierde el ownership.** ¿De quién es `GetMovieSummaryQuery`? Con un contracts global, de
   nadie. Con `Movies.Module.Contracts`, de Movies, y el compilador lo hace cumplir.
2. **Punto de acoplamiento central.** Cada mensaje nuevo de cualquier módulo modifica el
   ensamblado del que *todos* dependen. Recompilas todo, y el shared kernel crece sin control
   hasta volverse un *god assembly* — exactamente la bola de lodo que el monolito modular
   intenta evitar.
3. **Bloquea el camino a microservicios.** Si mañana se extrae Movies,
   `Movies.Module.Contracts` sale con él o se convierte en el paquete NuGet del cliente. Si los
   mensajes están en un contracts compartido, primero hay que desenredarlo.

**Regla práctica: `Core.Contracts` = cosas sin dominio. Contratos con nombres de negocio →
`<Módulo>.Contracts`.**

---

## 3. Fase de arranque: el registro

Todo esto se ejecuta **una sola vez**, al levantar la aplicación. Es la fase en la que se llena
el diccionario mensaje → handler.

### El punto de entrada

Cada módulo se registra desde su propio `*ModuleExtensions.cs`:

```csharp
// MovieServiceExtensions.cs  y  UsersModuleExtensions.cs
services.AddModuleMediator(config, Assembly.GetExecutingAssembly());
```

`Assembly.GetExecutingAssembly()` devuelve el ensamblado donde está *esa línea de código*, o sea
`CP.Portal.Movies.Module.dll` en un caso y `Users.Module.dll` en el otro. Este es el punto clave
del diseño: **cada módulo se auto-declara**. Movies dice "mis handlers están aquí dentro", Users
dice lo mismo del suyo, y **`Program.cs` nunca necesita enterarse de MediatR** ni de qué
ensamblados existen.

### `Core.Contracts/Messaging/ModuleMediatorExtensions.cs`

```csharp
services.AddMediatR(cfg =>
{
    cfg.LicenseKey = configuration[LicenseKeyPath];
    cfg.RegisterServicesFromAssembly(moduleAssembly);
});
```

**`cfg.LicenseKey`** — Lee `"MediatR:LicenseKey"` de `appsettings.json`. MediatR valida la firma
del JWT al arrancar y escribe el resultado en el log. Si faltara, la librería sigue funcionando
pero deja un `warn` en cada arranque.

**`cfg.RegisterServicesFromAssembly(moduleAssembly)`** — La línea que hace el trabajo real.
MediatR **escanea por reflexión** todos los tipos públicos e internos del ensamblado buscando
clases que implementen sus interfaces (`IRequestHandler<,>`, `INotificationHandler<>`, …) y
registra un servicio por cada una.

En Movies encuentra `GetMovieSummaryQueryHandler` y ejecuta el equivalente a:

```csharp
services.AddTransient<IRequestHandler<GetMovieSummaryQuery, Result<MovieSummary>>,
                      GetMovieSummaryQueryHandler>();
```

Fíjate en la forma del tipo de servicio: `IRequestHandler<GetMovieSummaryQuery, …>`. **El tipo
del mensaje está dentro del tipo del servicio.** Esa es literalmente la clave del diccionario.
Cuando en runtime alguien mande un `GetMovieSummaryQuery`, MediatR construye ese mismo tipo
genérico y le pide al contenedor lo que esté registrado bajo esa clave.

Por eso **el handler puede ser `internal`**: quien lo instancia es el contenedor de DI por
reflexión, y a la reflexión no le importan los modificadores de acceso. Nadie fuera de Movies
puede escribir `new GetMovieSummaryQueryHandler(...)`; ni siquiera puede nombrar el tipo.

### Las tres interfaces del mediador

`AddMediatR` también registra estas tres, todas como `Transient` (el default de MediatR):

| Interfaz | Qué expone | Cuándo inyectarla |
|---|---|---|
| `ISender` | `Send()` | La clase **pregunta** algo y espera respuesta |
| `IPublisher` | `Publish()` | La clase **anuncia** un hecho |
| `IMediator` | ambas | Cuando de verdad necesita las dos |

Inyectar `ISender` en `CartMovieService` e `IPublisher` en `MovieService` no es cosmética: al
leer el constructor ya sabes qué hace la clase con el mediador, sin leer el cuerpo. **Preferir
la mitad correcta sobre `IMediator` es la convención de este repositorio.**

### El registro del behavior

```csharp
services.TryAddEnumerable(ServiceDescriptor.Transient(
    typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)));
```

Cada pieza importa:

- **`typeof(IPipelineBehavior<,>)`** — Los `<,>` vacíos son un **genérico abierto**: el tipo sin
  cerrar. Se registra así porque no conocemos de antemano todas las combinaciones
  `(request, response)` que existirán. Cuando alguien pida
  `IPipelineBehavior<GetMovieSummaryQuery, Result<MovieSummary>>`, el contenedor cierra el
  genérico por sí solo. **Un solo registro cubre todos los mensajes presentes y futuros.**

- **`TryAddEnumerable`** — Evita un bug real. `AddModuleMediator` se llama **dos veces** (Movies
  y Users). Con un `AddTransient` normal, `IPipelineBehavior` quedaría registrado dos veces, y
  como MediatR resuelve `IEnumerable<IPipelineBehavior<,>>` obtendría **dos instancias del mismo
  behavior**: cada log saldría duplicado y una hipotética transacción se abriría dos veces.
  `TryAddEnumerable` deduplica por el par (tipo de servicio, tipo de implementación), así que la
  segunda llamada no hace nada.

- **`Transient`** — Instancia nueva por mensaje. Correcto, porque el behavior guarda estado por
  mensaje (el `Stopwatch`).

---

## 4. La cadena de llamadas: request/response

Recorrido completo de `POST /api/CartMovie/{MovieId}`. Lo interesante son los saltos entre
ensamblados.

```
[Users.Module.dll]                      [Core.Contracts.dll]         [CP.Portal.Movies.Module.dll]

 AddCartMovieEndpoint
        │
        ▼
 CartMovieService
        │  _sender.Send(new GetMovieSummaryQuery(id), ct)
        ▼
   ISender ─────────────► Mediator (implementación)
                              │  1. busca el handler en DI
                              │  2. busca los behaviors en DI
                              │  3. los anida
                              ▼
                         LoggingBehavior
                              │  await next(ct)
                              ▼
                                                    GetMovieSummaryQueryHandler
                                                              │
                                                              ▼
                                                       IMovieRepository → SQL
```

### Paso 1 — `AddCartMovieEndpoint.HandleAsync`

```csharp
if (!User.TryGetUserId(out var userId)) { await Send.UnauthorizedAsync(ct); return; }
var movieId = Route<Guid>("MovieId");
var result = await _cartService.AddCartMovieAsync(movieId, userId, ct);
```

Todavía no hay MediatR. Nota para evitar una confusión frecuente: ese `Send.UnauthorizedAsync`
es de **FastEndpoints**, no tiene relación con `ISender.Send`. Solo coinciden en el nombre.

### Paso 2 — `CartMovieService.AddCartMovieAsync`

```csharp
var movie = await _sender.Send(new GetMovieSummaryQuery(movieId), ct);
```

**`new GetMovieSummaryQuery(movieId)`** construye el mensaje:

```csharp
public sealed record GetMovieSummaryQuery(Guid MovieId) : IRequest<Result<MovieSummary>>;
```

El `record` sirve porque un mensaje es un dato, no un objeto con comportamiento: inmutable, con
igualdad por valor, y sin lógica que pueda desincronizarse entre módulos.

El `: IRequest<Result<MovieSummary>>` es la parte que importa: **es lo único que declara el tipo
de retorno**. Por eso `Send` es genérico y sabe devolver exactamente `Result<MovieSummary>` sin
casteos ni parámetros de tipo explícitos. Si cambias el tipo en la declaración del record, el
compilador te obliga a arreglar todos los consumidores.

**`_sender.Send(...)`** es donde ocurre el salto. Por dentro, el `Mediator` hace, en orden:

1. Lee el tipo real del objeto recibido: `GetMovieSummaryQuery`.
2. Construye por reflexión el tipo `IRequestHandler<GetMovieSummaryQuery, Result<MovieSummary>>`.
3. Se lo pide al contenedor de DI, que devuelve un `GetMovieSummaryQueryHandler` recién
   construido **con `IMovieRepository` ya inyectado** — y ese repositorio arrastra el
   `MovieDbContext` del scope de esta petición HTTP. Si no hubiera handler registrado, aquí
   truena con `InvalidOperationException` **en runtime, no en compilación**: ese es el precio
   del patrón.
4. Pide `IEnumerable<IPipelineBehavior<GetMovieSummaryQuery, Result<MovieSummary>>>` → obtiene
   `[LoggingBehavior<…>]`.
5. Los anida en cebolla, de dentro hacia fuera, y ejecuta la capa externa.

### Paso 3 — `LoggingBehavior.Handle`

```csharp
logger.LogInformation("Mediator → {Request}", requestName);
var response = await next(cancellationToken);
logger.LogInformation("Mediator ← {Request} en {Elapsed} ms", requestName, sw.ElapsedMilliseconds);
```

`RequestHandlerDelegate<TResponse> next` es el parámetro más importante y el menos evidente.
**Es "el resto del pipeline empaquetado en un delegado".** Con tres behaviors, el `next` del
primero sería el segundo; el del segundo, el tercero; y el del tercero, el handler real. Aquí,
con uno solo, `next` **es** el handler.

Esa estructura es lo que hace el código genuinamente reutilizable:

- Lo que escribes **antes** de `next` corre antes del handler: validar, autorizar, abrir
  transacción.
- Lo que escribes **después** corre con el resultado ya en mano: log, commit, invalidar caché.
- El `try/catch` envuelve al handler completo, así que atrapa excepciones de **cualquier handler
  de la aplicación**. El `throw;` sin argumento re-lanza preservando el stack trace original;
  con `throw ex;` se perdería.

Este behavior está escrito **una vez** y aplica a todos los mensajes de todos los módulos,
presentes y futuros. Es lo que un mediator artesanal no da gratis, y la razón principal para
usar MediatR en lugar de escribir el patrón a mano.

### Paso 4 — `GetMovieSummaryQueryHandler.Handle`

```csharp
var movie = await movieRepository.GetMovieAsync(request.MovieId, cancellationToken);

if (movie is null)
    return Result<MovieSummary>.Failure(MoviesContractErrors.MovieNotFound);

return Result<MovieSummary>.Success(new MovieSummary(movie.Id, movie.Title, movie.RentalPrice));
```

Estamos **dentro** de `CP.Portal.Movies.Module.dll`, en territorio de Movies, con acceso libre a
sus repositorios y entidades.

- **Reutiliza `IMovieRepository`**, que ya existía. Un handler de integración no reimplementa
  nada: es una fachada delgada sobre lo que el módulo ya sabe hacer.
- **El fallo viaja como valor de retorno, no como excepción.** Al cruzar el límite entre módulos,
  "no existe" es un resultado normal y esperado, no una anomalía. `MoviesContractErrors` es
  `public` a propósito: si Users va a poder reaccionar al error, el error es parte del contrato
  tanto como el mensaje.
- **La traducción a `MovieSummary` es la línea crítica.** Entra una entidad `Movie` (con
  `MovieGenres`, `Casts`, navegaciones de EF, `internal`), sale un
  `MovieSummary(Id, Title, RentalPrice)`. **La entidad nunca cruza el límite.** Si mañana
  renombras `RentalPrice` o le agregas quince propiedades, Users no se enterará ni se romperá:
  solo hay que mantener este mapeo. Sin este DTO tendrías el acoplamiento de vuelta, nada más
  que disfrazado.

### Paso 5 — el regreso

El `Result<MovieSummary>` sube por la cebolla: sale del handler → lo recibe el `await next(ct)`
del behavior → el behavior loguea el tiempo y lo devuelve → `Mediator.Send` lo devuelve → aterriza
en `CartMovieService`.

```csharp
if (!movie.IsSuccess)
    return Result<Guid>.Failure(movie.Error);

var cartMovie = new CartMovie(userId, movie.Value.Id);
```

- Users **propaga** el error de Movies sin reinterpretarlo. Como `Error` lleva su propio
  `HttpCode` (404), `SendApiResponseAsync` lo traduce solo, y el cliente recibe
  `404 MovieNotFound` en lugar de un 400 genérico o una excepción.
- Se usa `movie.Value.Id` y no `movieId` a propósito: el `Id` que se guarda es el que **Movies
  confirmó que existe**, no el que llegó por la URL. Es el mismo valor, pero la intención queda
  escrita en el código.

---

## 5. La otra mitad: notificaciones

La cadena anterior es *pregunta → respuesta*, con un handler obligatorio. Las notificaciones
funcionan al revés: **publica el dueño del dato y escucha quien quiera**.

Publica Movies, en `MovieService.UpdateMoviePrice`:

```csharp
await _publisher.Publish(new MoviePriceChangedNotification(movie.Id, previousPrice, price), ct);
```

Escucha Users, en `Application/Integrations/`:

```csharp
internal sealed class MoviePriceChangedNotificationHandler(ILogger<…> logger)
    : INotificationHandler<MoviePriceChangedNotification>
```

Cuatro diferencias que conviene tener claras:

| | `Send` (request) | `Publish` (notification) |
|---|---|---|
| Interfaz del mensaje | `IRequest<T>` | `INotification` |
| Handlers | Exactamente **1**. Cero → excepción | **0..N**. Cero → no pasa nada |
| Retorno | `T` | `void` |
| Pasa por `IPipelineBehavior` | Sí | **No** |

Esa última fila explica algo que se ve en el log: aparecen líneas `Mediator → GetMovieSummaryQuery`
para las consultas, pero **ninguna línea `Mediator →` para la notificación**, aunque su handler sí
corre. Las notificaciones no pasan por `IPipelineBehavior`; tienen su propio mecanismo
(`INotificationPublisher`).

> [!WARNING]
> **`Publish` no es fire-and-forget.** El publisher por defecto (`ForeachAwaitPublisher`) recorre
> los handlers uno por uno, **esperando cada uno, en el mismo hilo y la misma pila de llamadas**.
> Consecuencias reales:
> - Si un handler tarda 2 segundos, la petición HTTP tarda 2 segundos más.
> - Si un handler **lanza una excepción, esta sube y revienta la operación de Movies**.

Por eso en `UpdateMoviePrice` la publicación va **después** del `SaveChangesAsync`: si un
suscriptor falla, el precio ya quedó guardado. Cuando el desacople temporal importe de verdad,
esto es lo que se cambia por una cola (integration events + patrón Outbox).

---

## 6. Resumen de artefactos

| Artefacto | Ensamblado | Acceso | Para qué |
|---|---|---|---|
| `GetMovieSummaryQuery` | `Movies.Module.Contracts` | `public` | El mensaje. Lo único que Users conoce. Declara el tipo de retorno |
| `MovieSummary` | `Movies.Module.Contracts` | `public` | Aísla a los consumidores de la entidad `Movie` |
| `MoviesContractErrors` | `Movies.Module.Contracts` | `public` | Los fallos también son contrato |
| `MoviePriceChangedNotification` | `Movies.Module.Contracts` | `public` | Un hecho anunciado, sin destinatario conocido |
| `GetMovieSummaryQueryHandler` | `CP.Portal.Movies.Module` | `internal` | Fachada pública del módulo. Traduce entidad → DTO |
| `MoviePriceChangedNotificationHandler` | `Users.Module` | `internal` | Reacción de Users a un hecho de Movies |
| `LoggingBehavior` | `Core.Contracts` | `internal` | Middleware para todos los mensajes |
| `AddModuleMediator` | `Core.Contracts` | `public` | Cada módulo se registra solo; el host no participa |
| `ISender` / `IPublisher` | MediatR | — | El intermediario: preguntar / anunciar |

**El truco del desacople, en una frase:** `Users.Module.dll` contiene una referencia al tipo
`GetMovieSummaryQuery`, que vive en el ensamblado de *contratos*. **No contiene ninguna
referencia a `GetMovieSummaryQueryHandler`.** Ese enlace lo hace el contenedor de DI en runtime,
buscando por el tipo del mensaje. Si borraras `CP.Portal.Movies.Module.dll` del disco,
`Users.Module.dll` seguiría compilando perfectamente; solo fallaría al arrancar la app. Ese
"seguiría compilando" es la propiedad que permite partir el monolito en servicios más adelante.

---

## 7. La licencia de MediatR

**MediatR es software comercial a partir de la v13** (Lucky Penny Software). La 12.5.0 fue la
última bajo Apache 2.0. Este proyecto usa **14.2.0** con una licencia **Community edition**.

La clave se lee de `appsettings.json` → `MediatR:LicenseKey` y se asigna a `cfg.LicenseKey` en
`AddModuleMediator`. Al arrancar, MediatR confirma en el log:

```
info: LuckyPennySoftware.MediatR.License[0]
      You have a valid license key for the Lucky Penny software Bundle Community edition.
```

Alternativas si no se quiere la clave en el repositorio — el código funciona sin cambios en los
tres casos, porque MediatR resuelve la licencia por orden de precedencia:

1. `cfg.LicenseKey` (lo que hace este proyecto, vía `appsettings.json`)
2. `dotnet user-secrets`, que alimenta la misma ruta de configuración
3. Variables de entorno `MEDIATR_LICENSE_KEY` o `LUCKYPENNY_LICENSE_KEY`, que MediatR lee por su
   cuenta sin pasar por nuestro código

Dos notas:

- Los paquetes son dos y hacen cosas distintas: **`MediatR.Contracts`** (2.0.1) trae solo las
  interfaces de mensaje y es la única dependencia de los proyectos `*.Contracts`;
  **`MediatR`** (14.2.0) trae el motor y va en los proyectos de implementación.
- La licencia es un **bundle** de Lucky Penny, y el proyecto también usa **AutoMapper**, que es
  de la misma empresa y actualmente avisa en el arranque que no tiene licencia. La misma clave
  probablemente lo cubre; está pendiente aplicarla ahí.

---

## 8. Receta: agregar un mensaje nuevo

Para que Users pregunte algo nuevo a Movies:

1. **Contrato** en `Movies.Module.Contracts/Queries/` — un `record public` que implemente
   `IRequest<Result<T>>`. Si devuelve datos nuevos, agrega su DTO en `Dtos/`. Si puede fallar de
   una forma nueva, agrega el error en `Errors/MoviesContractErrors.cs`.
2. **Handler** en `CP.Portal.Movies.Module/Application/Integrations/` — `internal sealed`,
   implementando `IRequestHandler<TQuery, Result<T>>`. Reutiliza los repositorios existentes y
   traduce las entidades al DTO del contrato.
3. **Consumir** desde Users con `_sender.Send(new TQuery(...), ct)`.

No hay paso 4: **no hay que registrar nada**. El escaneo por ensamblado de
`RegisterServicesFromAssembly` descubre el handler nuevo solo.

Para el sentido inverso (Movies necesita algo de Users) hay que crear primero el proyecto
`Users.Module.Contracts` siguiendo el mismo patrón. No existe todavía porque no hay ningún
mensaje en esa dirección, y un proyecto vacío es andamiaje sin valor.

Para una notificación, igual pero con `INotification` / `INotificationHandler<>`, y el handler va
en el `Application/Integrations/` del módulo que **escucha**, no del que publica.

---

## 9. Decisiones tomadas y pendientes

### Por qué no hay `ValidationBehavior`

Es el ejemplo canónico de behavior en los tutoriales, y aquí se omitió a propósito:
FastEndpoints + FluentValidation ya validan en el borde HTTP, y los mensajes de integración
actuales son un `Guid`. Además, para que devolviera `Result<T>.Failure` en lugar de lanzar una
excepción haría falta una fábrica por reflexión — y mezclar los dos modelos de error habría sido
peor que no tenerlo. Se agregará cuando haya un mensaje que lo justifique.

### Ideas de behaviors para más adelante

Todos irían en `Core.Contracts/Messaging/Behaviors/` y se registran con el mismo
`TryAddEnumerable`. **El orden de registro es el orden de ejecución.**

- `TransactionBehavior` — abre transacción, commit al final
- `PerformanceBehavior` — advierte de handlers lentos
- `CachingBehavior` — para queries idempotentes

### Ejercicios para practicar el patrón

- `GET /api/CartMovie` que devuelva títulos y precios del carrito. Requiere una query nueva en
  el contrato (`GetMovieSummariesQuery` con varios ids) y recorre el patrón completo.
- Un `TransactionBehavior`, para escribir un behavior desde cero.

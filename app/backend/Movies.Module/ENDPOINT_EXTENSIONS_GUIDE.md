# Guía de Uso: EndpointExtensions

## 📋 Descripción

Los **EndpointExtensions** proporcionan métodos de extensión sobre `IEndpoint` de FastEndpoints para enviar respuestas HTTP estandarizadas en todo el módulo de Movies.

## 📁 Ubicación

```
Utilities/Extensions/EndpointExtensions.cs
```

## 🔧 Métodos Disponibles

### 1. `SendStandardSuccessAsync<T>` - Objeto único

Envía una respuesta exitosa con **un solo objeto**.

```csharp
public static Task SendStandardSuccessAsync<T>(
    this IEndpoint endpoint,
    T data,
    string message = "Operación exitosa",
    int statusCode = 200,
    CancellationToken ct = default)
```

**Ejemplo de uso:**

```csharp
internal class GetMovieByIdEndpoint : Endpoint<GetMovieRequest, MovieDto>
{
    public override async Task HandleAsync(GetMovieRequest req, CancellationToken ct)
    {
        var result = await _movieService.GetMovieByIdAsync(req.Id, ct);
        
        if (result.IsSuccess)
        {
            await this.SendStandardSuccessAsync(
                result.Value,
                "Película encontrada",
                statusCode: 200,
                ct: ct
            );
        }
    }
}
```

**Respuesta JSON:**

```json
{
  "data": [
    {
      "id": "01234567-89ab-cdef-0123-456789abcdef",
      "title": "Inception",
      "description": "A mind-bending thriller"
    }
  ],
  "message": "Película encontrada",
  "error": null
}
```

---

### 2. `SendStandardSuccessAsync<T>` - Colección

Envía una respuesta exitosa con **una colección de objetos**.

```csharp
public static Task SendStandardSuccessAsync<T>(
    this IEndpoint endpoint,
    IEnumerable<T> data,
    string message = "Operación exitosa",
    int statusCode = 200,
    CancellationToken ct = default)
```

**Ejemplo de uso:**

```csharp
internal class GetListMoviesEndpoint : EndpointWithoutRequest<IEnumerable<MovieDto>>
{
    public override async Task HandleAsync(CancellationToken ct = default)
    {
        var result = await _movieService.ListMovieAsync(ct);

        if (result.IsSuccess)
        {
            await this.SendStandardSuccessAsync(
                result.Value.Movies,
                "Películas obtenidas exitosamente",
                statusCode: 200,
                ct: ct
            );
        }
    }
}
```

**Respuesta JSON:**

```json
{
  "data": [
    {
      "id": "01234567-89ab-cdef-0123-456789abcdef",
      "title": "Inception",
      "description": "A mind-bending thriller"
    },
    {
      "id": "fedcba98-7654-3210-fedc-ba9876543210",
      "title": "Interstellar",
      "description": "Space exploration epic"
    }
  ],
  "message": "Películas obtenidas exitosamente",
  "error": null
}
```

---

### 3. `SendStandardFailureAsync<T>`

Envía una respuesta de **error estandarizada**.

```csharp
public static Task SendStandardFailureAsync<T>(
    this IEndpoint endpoint,
    string error,
    string message,
    int statusCode = 400,
    CancellationToken ct = default)
```

**Ejemplo de uso:**

```csharp
internal class GetMovieByIdEndpoint : Endpoint<GetMovieRequest, MovieDto>
{
    public override async Task HandleAsync(GetMovieRequest req, CancellationToken ct)
    {
        var result = await _movieService.GetMovieByIdAsync(req.Id, ct);
        
        if (!result.IsSuccess)
        {
            await this.SendStandardFailureAsync<MovieDto>(
                error: result.Error.Code,
                message: result.Error.Message,
                statusCode: 404,
                ct: ct
            );
        }
    }
}
```

**Respuesta JSON:**

```json
{
  "data": null,
  "message": "Película no encontrada",
  "error": "NotFound"
}
```

---

### 4. `SendStandardResponseAsync<T>` - Manejo automático de Result

Maneja automáticamente **éxito o fallo** basándose en un objeto `Result<T>`.

```csharp
public static Task SendStandardResponseAsync<T>(
    this IEndpoint endpoint,
    Result<T> result,
    string successMessage = "Operación exitosa",
    int successStatusCode = 200,
    int errorStatusCode = 400,
    CancellationToken ct = default)
```

**Ejemplo de uso (Recomendado):**

```csharp
internal class CreateMovieEndpoint : Endpoint<AddMovieRequest, string>
{
    public override async Task HandleAsync(AddMovieRequest req, CancellationToken ct)
    {
        var result = await _movieService.CreateMovieAsync(req, ct);
        
        // Manejo automático de éxito/error basado en Result<T>
        await this.SendStandardResponseAsync(
            result,
            successMessage: "Película creada exitosamente",
            successStatusCode: 201,
            errorStatusCode: 400,
            ct: ct
        );
    }
}
```

**Respuesta JSON (Éxito - 201):**

```json
{
  "data": ["01234567-89ab-cdef-0123-456789abcdef"],
  "message": "Película creada exitosamente",
  "error": null
}
```

**Respuesta JSON (Error - 400):**

```json
{
  "data": null,
  "message": "El título es obligatorio.",
  "error": "ValidationError"
}
```

---

## 🎯 Patrones Recomendados

### Patrón 1: Manejo manual de éxito/error

```csharp
public override async Task HandleAsync(TRequest req, CancellationToken ct)
{
    var result = await _service.DoSomethingAsync(req, ct);
    
    if (result.IsSuccess)
    {
        await this.SendStandardSuccessAsync(
            result.Value,
            "Operación exitosa",
            ct: ct
        );
    }
    else
    {
        await this.SendStandardFailureAsync<TResponse>(
            result.Error.Code,
            result.Error.Message,
            statusCode: 400,
            ct: ct
        );
    }
}
```

### Patrón 2: Manejo automático con `SendStandardResponseAsync` (Recomendado)

```csharp
public override async Task HandleAsync(TRequest req, CancellationToken ct)
{
    var result = await _service.DoSomethingAsync(req, ct);
    
    await this.SendStandardResponseAsync(
        result,
        successMessage: "Operación exitosa",
        successStatusCode: 200,
        errorStatusCode: 400,
        ct: ct
    );
}
```

---

## 📊 Estructura de Respuesta Estandarizada

Todas las respuestas siguen este formato:

```typescript
{
  "data": T[] | null,      // Array de datos (null en caso de error)
  "message": string,       // Mensaje descriptivo
  "error": string | null   // Código de error (null en caso de éxito)
}
```

---

## ✅ Ventajas

✅ **Consistencia**: Todas las APIs devuelven el mismo formato de respuesta  
✅ **Mantenibilidad**: Cambios centralizados en un solo lugar  
✅ **Type Safety**: IntelliSense completo y validación en tiempo de compilación  
✅ **Sintaxis limpia**: Se siente como parte nativa de FastEndpoints  
✅ **Reutilizable**: Disponible automáticamente en todos los endpoints  
✅ **Flexible**: Múltiples sobrecargas para diferentes casos de uso  

---

## 🔄 Migración de Endpoints Existentes

**Antes:**

```csharp
public override async Task HandleAsync(CancellationToken ct)
{
    var movies = await _movieService.ListMovieAsync(ct);
    await Send.OkAsync(movies.Value.Movies, ct);
}
```

**Después:**

```csharp
public override async Task HandleAsync(CancellationToken ct)
{
    var result = await _movieService.ListMovieAsync(ct);
    
    await this.SendStandardResponseAsync(
        result,
        successMessage: "Películas obtenidas exitosamente",
        ct: ct
    );
}
```

---

## 📚 Referencias

- **Archivo principal:** `Utilities/Extensions/EndpointExtensions.cs`
- **DTO de respuesta:** `Utilities/Abstractions/ResponseDto.cs`
- **Objeto Result:** `Utilities/Abstractions/Result.cs`
- **Ejemplo de uso:** `Application/Endpoints/MovieEndpoints/GetListMoviesAsync/GetListMoviesEndpoint.cs`

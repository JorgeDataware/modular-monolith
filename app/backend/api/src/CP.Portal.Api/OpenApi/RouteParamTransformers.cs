using System.Reflection;
using System.Text.RegularExpressions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CP.Portal.Api.OpenApi;

/// <summary>
/// FastEndpoints enlaza las propiedades marcadas con <see cref="RouteParamAttribute"/> desde la URL,
/// pero el generador de OpenAPI no conoce ese atributo y las publica dentro del cuerpo de la petición.
/// Este transformador las elimina del esquema del cuerpo.
/// </summary>
internal sealed class RouteParamSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken ct)
    {
        foreach (var property in context.JsonTypeInfo.Properties)
        {
            if (property.AttributeProvider?.IsDefined(typeof(RouteParamAttribute), inherit: true) is not true)
                continue;

            schema.Properties?.Remove(property.Name);
            schema.Required?.Remove(property.Name);
        }

        return Task.CompletedTask;
    }
}

/// <summary>
/// Declara como parámetros de ruta los segmentos de la plantilla del endpoint. FastEndpoints registra
/// los endpoints con un delegado genérico, así que el generador de OpenAPI no puede deducirlos por sí solo.
/// </summary>
internal sealed partial class RouteParamOperationTransformer : IOpenApiOperationTransformer
{
    [GeneratedRegex(@"\{(?<name>[^:?}]+)")]
    private static partial Regex RouteTokens { get; }

    public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken ct)
    {
        var requestType = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<IAcceptsMetadata>()
            .FirstOrDefault()?
            .RequestType;

        if (requestType is null)
            return;

        foreach (Match token in RouteTokens.Matches(context.Description.RelativePath ?? string.Empty))
        {
            var name = token.Groups["name"].Value;

            if (operation.Parameters?.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)) is true)
                continue;

            var property = requestType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property is null)
                continue;

            operation.Parameters ??= [];
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Path,
                Required = true,
                Schema = await context.GetOrCreateSchemaAsync(property.PropertyType, cancellationToken: ct)
            });
        }
    }
}

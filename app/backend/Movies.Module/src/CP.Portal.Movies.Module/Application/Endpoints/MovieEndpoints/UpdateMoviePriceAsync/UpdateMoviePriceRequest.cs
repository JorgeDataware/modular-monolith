using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.UpdateMoviePriceAsync;

internal record UpdateMoviePriceRequest
(
    [property: RouteParam]
    Guid Id,
    decimal RentalPrice
);

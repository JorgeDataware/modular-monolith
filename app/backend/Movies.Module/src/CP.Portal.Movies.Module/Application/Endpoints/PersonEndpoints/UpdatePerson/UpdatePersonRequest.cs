using FastEndpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.UpdatePerson;

internal record UpdatePersonRequest
(
    [property: RouteParam]
    Guid Id,
    string FirstName,
    string LastName,
    string Bio
);

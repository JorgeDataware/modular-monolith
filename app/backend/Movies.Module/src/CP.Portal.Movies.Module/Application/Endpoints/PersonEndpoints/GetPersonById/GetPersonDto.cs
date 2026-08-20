using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.GetPersonById;

internal record GetPersonDto
(
    Guid Id,
    string FirstName,
    string LastName,
    string Bio
);

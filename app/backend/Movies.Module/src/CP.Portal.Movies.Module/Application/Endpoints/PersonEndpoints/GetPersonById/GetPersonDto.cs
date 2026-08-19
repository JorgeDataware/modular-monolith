using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.GetPersonById;

internal record GetPersonDto
(
    Guid Id,
    string FisrtName,
    string LastName,
    string Bio
);

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;

internal record AddPersonRequest
(
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string Bio
);

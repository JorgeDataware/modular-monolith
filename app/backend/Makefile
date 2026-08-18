.PHONY: movie-migration-add

MIGRATION ?= no

movie-migration-add:
	dotnet ef migrations add $(MIGRATION) -c MovieDbContext -p "Movies.Module\src\CP.Portal.Movies.Module" -s "Api\src\CP.Portal.Api" --output-dir "Domain\Migrations"

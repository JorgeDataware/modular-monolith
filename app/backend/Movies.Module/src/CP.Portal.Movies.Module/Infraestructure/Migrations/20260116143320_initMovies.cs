using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CP.Portal.Movies.Module.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class initMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "movies");

            migrationBuilder.CreateTable(
                name: "genres",
                schema: "movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                schema: "movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseYear = table.Column<DateOnly>(type: "date", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentalPrice = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                schema: "movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "movie_genres",
                schema: "movies",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_genres", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_movie_genres_genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "movies",
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movie_genres_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "movies",
                        principalTable: "movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "casters",
                schema: "movies",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Character = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_casters", x => new { x.MovieId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_casters_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "movies",
                        principalTable: "movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_casters_persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "movies",
                        principalTable: "persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crewers",
                schema: "movies",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crewers", x => new { x.MovieId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_crewers_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "movies",
                        principalTable: "movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_crewers_persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "movies",
                        principalTable: "persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_casters_MovieId",
                schema: "movies",
                table: "casters",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_casters_PersonId",
                schema: "movies",
                table: "casters",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_crewers_MovieId",
                schema: "movies",
                table: "crewers",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_crewers_PersonId",
                schema: "movies",
                table: "crewers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_movie_genres_GenreId",
                schema: "movies",
                table: "movie_genres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_movie_genres_MovieId",
                schema: "movies",
                table: "movie_genres",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "casters",
                schema: "movies");

            migrationBuilder.DropTable(
                name: "crewers",
                schema: "movies");

            migrationBuilder.DropTable(
                name: "movie_genres",
                schema: "movies");

            migrationBuilder.DropTable(
                name: "persons",
                schema: "movies");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "movies");

            migrationBuilder.DropTable(
                name: "movies",
                schema: "movies");
        }
    }
}

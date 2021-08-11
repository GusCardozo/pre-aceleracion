using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Challenge.Aceleracion.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Disney");

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "Disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "Disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                schema: "Disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Qualification = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenresId",
                        column: x => x.GenresId,
                        principalSchema: "Disney",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterMovie",
                schema: "Disney",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterMovie", x => new { x.CharactersId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_CharacterMovie_Characters_CharactersId",
                        column: x => x.CharactersId,
                        principalSchema: "Disney",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalSchema: "Disney",
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Disney",
                table: "Genres",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "Testimage", "Comedia" },
                    { 2, "Testimage", "Fantasía" },
                    { 3, "Testimage", "Musical" },
                    { 4, "Testimage", "Acción/Aventura" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterMovie_MoviesId",
                schema: "Disney",
                table: "CharacterMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenresId",
                schema: "Disney",
                table: "Movies",
                column: "GenresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterMovie",
                schema: "Disney");

            migrationBuilder.DropTable(
                name: "Characters",
                schema: "Disney");

            migrationBuilder.DropTable(
                name: "Movies",
                schema: "Disney");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "Disney");
        }
    }
}

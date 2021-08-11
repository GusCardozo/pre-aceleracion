using Microsoft.EntityFrameworkCore.Migrations;

namespace Challenge.Aceleracion.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_GenresId",
                schema: "Disney",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "GenresId",
                schema: "Disney",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CharacterMovies",
                schema: "Disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterMovies", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_GenresId",
                schema: "Disney",
                table: "Movies",
                column: "GenresId",
                principalSchema: "Disney",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_GenresId",
                schema: "Disney",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "CharacterMovies",
                schema: "Disney");

            migrationBuilder.AlterColumn<int>(
                name: "GenresId",
                schema: "Disney",
                table: "Movies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_GenresId",
                schema: "Disney",
                table: "Movies",
                column: "GenresId",
                principalSchema: "Disney",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BulkyBook.DataAccess.Migrations
{
    public partial class AddPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoverPhotoId",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CoverPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverPhoto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CoverPhotoId",
                table: "Product",
                column: "CoverPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_CoverPhoto_CoverPhotoId",
                table: "Product",
                column: "CoverPhotoId",
                principalTable: "CoverPhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_CoverPhoto_CoverPhotoId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "CoverPhoto");

            migrationBuilder.DropIndex(
                name: "IX_Product_CoverPhotoId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CoverPhotoId",
                table: "Product");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoeStore.Migrations.ShoeStoreDb
{
    public partial class ShoeStoreDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shoe",
                columns: table => new
                {
                    shoeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    shoeName = table.Column<string>(maxLength: 50, nullable: true),
                    shoeImage = table.Column<byte[]>(nullable: true),
                    price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoe", x => x.shoeId);
                });

            migrationBuilder.CreateTable(
                name: "UserCart",
                columns: table => new
                {
                    cartId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    shoeId = table.Column<int>(nullable: false),
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCart", x => new { x.shoeId, x.cartId });
                    table.ForeignKey(
                        name: "FK__UserCart",
                        column: x => x.shoeId,
                        principalTable: "Shoe",
                        principalColumn: "shoeId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCart");

            migrationBuilder.DropTable(
                name: "Shoe");
        }
    }
}

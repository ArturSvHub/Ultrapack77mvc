using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpakDataAccessLibrary.Migrations
{
    public partial class AddImageToCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Categories",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Categories");
        }
    }
}

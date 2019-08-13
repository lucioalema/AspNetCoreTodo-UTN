using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreTodo.Migrations
{
    public partial class UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("d1417003-6349-4f54-b458-05b1135f2d31"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("d220bf10-820e-4c28-be81-383a7b886b49"));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Items");

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DueAt", "IsDone", "Title" },
                values: new object[] { new Guid("d1417003-6349-4f54-b458-05b1135f2d31"), new DateTimeOffset(new DateTime(2019, 8, 6, 21, 49, 39, 696, DateTimeKind.Unspecified).AddTicks(127), new TimeSpan(0, -3, 0, 0, 0)), false, "Curso ASP.NET Core" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DueAt", "IsDone", "Title" },
                values: new object[] { new Guid("d220bf10-820e-4c28-be81-383a7b886b49"), new DateTimeOffset(new DateTime(2019, 8, 6, 21, 49, 39, 704, DateTimeKind.Unspecified).AddTicks(7198), new TimeSpan(0, -3, 0, 0, 0)), false, "Curso React" });
        }
    }
}

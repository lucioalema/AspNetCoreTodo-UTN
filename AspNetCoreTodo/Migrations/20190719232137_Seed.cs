using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreTodo.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("bcaf58d3-eced-41e5-9539-1b1f1d3c7dff"));

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DueAt", "IsDone", "Title" },
                values: new object[] { new Guid("808c1504-7899-45c0-9369-5dc12a662429"), new DateTimeOffset(new DateTime(2019, 7, 20, 20, 21, 36, 942, DateTimeKind.Unspecified).AddTicks(6868), new TimeSpan(0, -3, 0, 0, 0)), false, "Curso ASP.NET Core" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DueAt", "IsDone", "Title" },
                values: new object[] { new Guid("98ca59e2-4c74-4c57-b848-a8f77ee23a5c"), new DateTimeOffset(new DateTime(2019, 7, 20, 20, 21, 36, 946, DateTimeKind.Unspecified).AddTicks(1648), new TimeSpan(0, -3, 0, 0, 0)), false, "Curso React" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("808c1504-7899-45c0-9369-5dc12a662429"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("98ca59e2-4c74-4c57-b848-a8f77ee23a5c"));

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DueAt", "IsDone", "Title" },
                values: new object[] { new Guid("bcaf58d3-eced-41e5-9539-1b1f1d3c7dff"), new DateTimeOffset(new DateTime(2019, 7, 20, 20, 18, 18, 76, DateTimeKind.Unspecified).AddTicks(5296), new TimeSpan(0, -3, 0, 0, 0)), false, "Curso ASP.NET Core UTN" });
        }
    }
}

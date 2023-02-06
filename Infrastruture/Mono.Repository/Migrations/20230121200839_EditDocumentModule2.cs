using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mono.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditDocumentModule2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PdfConvertedPath",
                table: "Files",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "Files",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("0aa8656e-4a18-4071-90ab-dd2e1a985e03"), "Report", "Tờ trình" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("43e47d69-3519-4ca1-8620-d6daa6bcade6"), "Dispatch", "Công văn" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("8d5d75ea-152a-4da5-a7b4-e9975862b98c"), "Decision", "Quyết định" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("2cf86642-f5f7-44ae-8a7d-8fe29cbc1040"), "Resolution", "Nghị quyết" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("783a5f39-07f8-49c0-beb5-60fff43edfcf"), "Command", "Chỉ thị" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PdfConvertedPath",
                table: "Files",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "Files",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("d1a0adfe-41ea-487f-8536-3719564b0d3b"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("fa99c43a-db2a-4359-b986-2eed7a28453c"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("fe68e0d3-6220-4468-b331-cae3b435c90c"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("3a12091d-aa25-4c5e-8036-6026f80285fc"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("cf18f3ab-f4b3-449c-8a01-146af89f6ac1"), null, null });
        }
    }
}

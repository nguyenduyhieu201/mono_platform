using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mono.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditDocumentModule1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuedDeparment",
                table: "Documents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signer",
                table: "Documents",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleSearch",
                table: "Documents",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("d1a0adfe-41ea-487f-8536-3719564b0d3b"), "Report", "Tờ trình" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("fa99c43a-db2a-4359-b986-2eed7a28453c"), "Dispatch", "Công văn" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("fe68e0d3-6220-4468-b331-cae3b435c90c"), "Decision", "Quyết định" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("3a12091d-aa25-4c5e-8036-6026f80285fc"), "Resolution", "Nghị quyết" });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("cf18f3ab-f4b3-449c-8a01-146af89f6ac1"), "Command", "Chỉ thị" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedDeparment",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Signer",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TitleSearch",
                table: "Documents");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("7febdf2f-2bfc-4389-97a9-a45a78a7e70e"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("c4bc5aef-0bfa-46f0-bc1f-779bb986bda1"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("8b6a96d4-1e82-449c-925a-fde17a5602ac"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("076c7c61-2ad8-443c-acf4-089e0294a91e"), null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Guid", "Title_en", "Title_vi" },
                values: new object[] { new Guid("90297518-c7ad-4d31-bff3-e0b2faa64af6"), null, null });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewlMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                table: "Books",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Books_RequestId",
                table: "Books",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Requests_RequestId",
                table: "Books",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Requests_RequestId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_RequestId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Books");
        }
    }
}

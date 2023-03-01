using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCollectingIdeas.Migrations
{
    public partial class idea_12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_AspNetUsers_IdentityUserId",
                table: "Ideas");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_AspNetUsers_IdentityUserId",
                table: "Ideas",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_AspNetUsers_IdentityUserId",
                table: "Ideas");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_AspNetUsers_IdentityUserId",
                table: "Ideas",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

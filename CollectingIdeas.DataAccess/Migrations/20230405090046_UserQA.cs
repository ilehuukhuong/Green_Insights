using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCollectingIdeas.Migrations
{
    public partial class UserQA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isQA",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isQA",
                table: "AspNetUsers");
        }
    }
}

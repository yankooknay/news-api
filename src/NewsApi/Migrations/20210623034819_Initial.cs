using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    By = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Time = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsItem");
        }
    }
}

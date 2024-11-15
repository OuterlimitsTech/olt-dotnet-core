using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Migrations.SqlContext
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "TestEntities",
                schema: "dbo",
                columns: table => new
                {
                    TestEntitiesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10, 5")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestEntities", x => x.TestEntitiesId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestEntities",
                schema: "dbo");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace school_project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    StepGoal = table.Column<int>(type: "integer", nullable: false),
                    CalorieGoal = table.Column<int>(type: "integer", nullable: false),
                    Steps = table.Column<int>(type: "integer", nullable: false),
                    Calories = table.Column<int>(type: "integer", nullable: false),
                    ActiveMinutes = table.Column<int>(type: "integer", nullable: false),
                    Protein = table.Column<int>(type: "integer", nullable: false),
                    Carbs = table.Column<int>(type: "integer", nullable: false),
                    Fat = table.Column<int>(type: "integer", nullable: false),
                    WaterIntake = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

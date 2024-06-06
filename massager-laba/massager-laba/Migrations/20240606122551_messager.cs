using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace massagerlaba.Migrations
{
    /// <inheritdoc />
    public partial class messager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessagerModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUserWhere = table.Column<Guid>(type: "uuid", nullable: false),
                    LastLetter = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdUserFrom = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCheked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagerModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessagerModels");
        }
    }
}

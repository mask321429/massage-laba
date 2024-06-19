using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace massagerlaba.Migrations
{
    /// <inheritdoc />
    public partial class addTypeInInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeMessage",
                table: "MessageInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeMessage",
                table: "MessageInfos");
        }
    }
}

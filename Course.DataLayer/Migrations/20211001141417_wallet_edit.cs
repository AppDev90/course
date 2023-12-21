using Microsoft.EntityFrameworkCore.Migrations;

namespace Course.DataLayer.Migrations
{
    public partial class wallet_edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPay",
                table: "Wallets",
                newName: "IsDone");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Wallets",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "Wallets",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Wallets");

            migrationBuilder.RenameColumn(
                name: "IsDone",
                table: "Wallets",
                newName: "IsPay");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Wallets",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);
        }
    }
}

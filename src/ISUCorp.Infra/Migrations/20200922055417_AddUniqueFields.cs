using Microsoft.EntityFrameworkCore.Migrations;

namespace ISUCorp.Infra.Migrations
{
    public partial class AddUniqueFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Places_Name",
                table: "Places",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Name",
                table: "Contacts",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Places_Name",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_Name",
                table: "Contacts");
        }
    }
}

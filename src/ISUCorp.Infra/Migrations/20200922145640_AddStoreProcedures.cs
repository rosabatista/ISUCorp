using Microsoft.EntityFrameworkCore.Migrations;

namespace ISUCorp.Infra.Migrations
{
    public partial class AddStoreProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spSearchContactsByName = @"CREATE PROCEDURE [dbo].[spSearchContactsByName]
	                                          @term nvarchar(255)
                                          AS
                                          BEGIN
	                                          SELECT *
	                                          FROM Contacts
	                                          WHERE Name LIKE '%' + @term + '%'
                                              ORDER BY AddedAt
                                          END";

            var spSearchPlacesByName = @"CREATE PROCEDURE [dbo].[spSearchPlacesByName]
	                                          @term nvarchar(255)
                                          AS
                                          BEGIN
	                                          SELECT *
	                                          FROM Places
	                                          WHERE Name LIKE '%' + @term + '%'
                                              ORDER BY AddedAt
                                          END";

            migrationBuilder.Sql(spSearchContactsByName);
            migrationBuilder.Sql(spSearchPlacesByName);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var spSearchContactsByName = @"DROP PROCEDURE [dbo].[spSearchContactsByName]";
            var spSearchPlacesByName = @"DROP PROCEDURE [dbo].[spSearchPlacesByName]";

            migrationBuilder.Sql(spSearchContactsByName);
            migrationBuilder.Sql(spSearchPlacesByName);
        }
    }
}

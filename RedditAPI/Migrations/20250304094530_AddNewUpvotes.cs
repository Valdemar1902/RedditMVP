using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedditMVP.Migrations
{
    /// <inheritdoc />
    public partial class AddNewUpvotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Votes",
                table: "Posts",
                newName: "Upvotes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Upvotes",
                table: "Posts",
                newName: "Votes");
        }
    }
}

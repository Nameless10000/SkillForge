using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillForge.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductWatchlist_IsNotified_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductsWatchlist_ProductID",
                table: "ProductsWatchlist");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsWatchlist_ProductID_IsNotified",
                table: "ProductsWatchlist",
                columns: new[] { "ProductID", "IsNotified" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductsWatchlist_ProductID_IsNotified",
                table: "ProductsWatchlist");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsWatchlist_ProductID",
                table: "ProductsWatchlist",
                column: "ProductID");
        }
    }
}

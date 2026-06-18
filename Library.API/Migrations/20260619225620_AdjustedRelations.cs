using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Reservation_ReservationId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Client_ClientId",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Reservation",
                newName: "ClientModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ClientId",
                table: "Reservation",
                newName: "IX_Reservation_ClientModelId");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Book",
                newName: "ReservationModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_ReservationId",
                table: "Book",
                newName: "IX_Book_ReservationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Reservation_ReservationModelId",
                table: "Book",
                column: "ReservationModelId",
                principalTable: "Reservation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Client_ClientModelId",
                table: "Reservation",
                column: "ClientModelId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Reservation_ReservationModelId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Client_ClientModelId",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "ClientModelId",
                table: "Reservation",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ClientModelId",
                table: "Reservation",
                newName: "IX_Reservation_ClientId");

            migrationBuilder.RenameColumn(
                name: "ReservationModelId",
                table: "Book",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_ReservationModelId",
                table: "Book",
                newName: "IX_Book_ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Reservation_ReservationId",
                table: "Book",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Client_ClientId",
                table: "Reservation",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

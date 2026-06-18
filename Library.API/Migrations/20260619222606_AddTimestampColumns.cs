using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestampColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Reservation_ReservationModelId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_ReservationModelId",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "ReservationModelId",
                table: "Book",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reservation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Reservation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reservation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Client",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Client",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Client",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Book",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Book",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ClientId",
                table: "Reservation",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_ReservationId",
                table: "Book",
                column: "ReservationId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Reservation_ReservationId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Client_ClientId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ClientId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Book_ReservationId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Book",
                newName: "ReservationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_ReservationModelId",
                table: "Book",
                column: "ReservationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Reservation_ReservationModelId",
                table: "Book",
                column: "ReservationModelId",
                principalTable: "Reservation",
                principalColumn: "Id");
        }
    }
}

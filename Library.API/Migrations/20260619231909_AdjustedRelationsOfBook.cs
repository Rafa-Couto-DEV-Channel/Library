using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedRelationsOfBook : Migration
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

            migrationBuilder.DropColumn(
                name: "ReservationModelId",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Book",
                table: "Reservation",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Book",
                table: "Reservation");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationModelId",
                table: "Book",
                type: "TEXT",
                nullable: true);

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

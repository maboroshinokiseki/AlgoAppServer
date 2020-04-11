using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgoApp.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 1,
                column: "Order",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 2,
                column: "Order",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 3,
                column: "Order",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 4,
                column: "Order",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 5,
                column: "Order",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 6,
                column: "Order",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 7,
                column: "Order",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 8,
                column: "Order",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 9,
                column: "Order",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 10,
                column: "Order",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 11,
                column: "Order",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 664, DateTimeKind.Local).AddTicks(8377));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6742));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6770));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6774));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6782));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6784));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6786));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6789));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6791));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6794));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6796));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 15,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6798));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6801));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 17,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 9, 6, 30, 665, DateTimeKind.Local).AddTicks(6803));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 1,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 2,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 3,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 4,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 5,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 6,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 7,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 8,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 9,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 10,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 11,
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 813, DateTimeKind.Local).AddTicks(4023));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3304));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3332));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3336));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3339));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3341));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3344));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3347));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3350));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3353));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3355));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3357));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3360));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 15,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3362));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3364));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 17,
                column: "BirthDay",
                value: new DateTime(2000, 4, 5, 3, 15, 45, 814, DateTimeKind.Local).AddTicks(3367));
        }
    }
}

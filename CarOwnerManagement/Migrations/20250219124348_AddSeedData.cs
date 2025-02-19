using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarOwnerManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Description", "FuelType", "Name" },
                values: new object[,]
                {
                    { 1, "A high-performance luxury electric sedan with impressive acceleration, long range, and Autopilot capabilities.", 3, "Tesla Model S" },
                    { 2, "America’s best-selling full-size pickup truck, known for its durability, towing capacity, and versatility.", 1, "Ford F-150" },
                    { 3, "A powerful and reliable diesel pickup truck designed for heavy-duty work and off-road capability.", 2, "Chevrolet Silverado 1500" },
                    { 4, "A luxury electric sedan with a spacious interior, autonomous driving, and a state-of-the-art infotainment system.", 1, "Porsche 911" },
                    { 5, "A sporty and luxurious electric sedan combining BMW’s signature driving dynamics with zero emissions.", 3, "BMW i4" }
                });

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Neo Anderson" },
                    { 2, "Darth Wader" },
                    { 3, "Gandalf Grey" }
                });

            migrationBuilder.InsertData(
                table: "CarOwners",
                columns: new[] { "CarsId", "OwnersId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 2, 2 },
                    { 3, 1 },
                    { 4, 2 },
                    { 4, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarOwners",
                keyColumns: new[] { "CarsId", "OwnersId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "CarOwners",
                keyColumns: new[] { "CarsId", "OwnersId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "CarOwners",
                keyColumns: new[] { "CarsId", "OwnersId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "CarOwners",
                keyColumns: new[] { "CarsId", "OwnersId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "CarOwners",
                keyColumns: new[] { "CarsId", "OwnersId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

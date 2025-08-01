using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class InitialCreateAndSeed : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "dbo");

        migrationBuilder.CreateTable(
            name: "birds",
            schema: "dbo",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Wingspan = table.Column<long>(type: "bigint", nullable: false),
                name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_birds", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "cats",
            schema: "dbo",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                colour = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cats", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "dogs",
            schema: "dbo",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                colour1 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                colour2 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                pattern = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_dogs", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "users",
            schema: "dbo",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
            });

        migrationBuilder.InsertData(
            schema: "dbo",
            table: "birds",
            columns: ["id", "name", "Wingspan"],
            values: new object[,]
            {
                { new Guid("a1e1e1e1-1111-1111-1111-111111111111"), "Eagle", 200L },
                { new Guid("b2e2e2e2-2222-2222-2222-222222222222"), "Parrot", 25L },
                { new Guid("c3e3e3e3-3333-3333-3333-333333333333"), "Penguin", 30L }
            });

        migrationBuilder.InsertData(
            schema: "dbo",
            table: "cats",
            columns: ["id", "colour", "name"],
            values: new object[,]
            {
                { new Guid("a7e7e7e7-7777-7777-7777-777777777777"), "Black and White", "Toby" },
                { new Guid("f6e6e6e6-6666-6666-6666-666666666666"), "Tabby", "Whiskers" }
            });

        migrationBuilder.InsertData(
            schema: "dbo",
            table: "dogs",
            columns: ["id", "colour1", "colour2", "name", "pattern"],
            values: new object[,]
            {
                { new Guid("d4e4e4e4-4444-4444-4444-444444444444"), "Brown", null, "Rover", null },
                { new Guid("e5e5e5e5-5555-5555-5555-555555555555"), "White", "Black", "Spot", "Spots" }
            });

        migrationBuilder.InsertData(
            schema: "dbo",
            table: "users",
            columns: ["id", "email", "first_name", "last_name", "password_hash"],
            values: new object[] { new Guid("8000ba35-6cf7-4aef-90f7-097f8f6a3b13"), "a@b.com", "API", "User", "BE18F3BE7CD3799DF493F45B9118F3EE4A5BF512A96DC326907257D238B44628-29841CFA820416A7FEFB8CB02C56301B" });

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            schema: "dbo",
            table: "users",
            column: "email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "birds",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "cats",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "dogs",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "users",
            schema: "dbo");
    }
}

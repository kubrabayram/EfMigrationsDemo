using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfMigrationsDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogCreatedTimestamp : Migration
    {
        /// <inheritdoc />
protected override void Up(MigrationBuilder migrationBuilder)
{
    // 1) Geçici olarak nullable kolon ekle
    migrationBuilder.AddColumn<DateTime>(
        name: "CreatedTimestamp",
        table: "Blogs",
        type: "TEXT",
        nullable: true);

    // 2) Var olan satırları doldur (SQLite)
    migrationBuilder.Sql(
        "UPDATE Blogs SET CreatedTimestamp = CURRENT_TIMESTAMP WHERE CreatedTimestamp IS NULL;");

    // 3) NOT NULL yap + yeni kayıtlar için varsayılan
    migrationBuilder.AlterColumn<DateTime>(
        name: "CreatedTimestamp",
        table: "Blogs",
        type: "TEXT",
        nullable: false,
        defaultValueSql: "CURRENT_TIMESTAMP",
        oldClrType: typeof(DateTime),
        oldType: "TEXT",
        oldNullable: true);
}

        /// <inheritdoc />
protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "CreatedTimestamp",
        table: "Blogs");
}

    }
}

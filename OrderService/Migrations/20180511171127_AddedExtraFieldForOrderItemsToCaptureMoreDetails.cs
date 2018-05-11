using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OrderService.Migrations
{
    public partial class AddedExtraFieldForOrderItemsToCaptureMoreDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCategory",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductMediaFile",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductSku",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShopperReview",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCategory",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductMediaFile",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductSku",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ShopperReview",
                table: "OrderItems");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBookShop.Migrations
{
    public partial class RemoveRequiredAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerAddress_CustomerAddressId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Opinion_Customer_CustomerId",
                table: "Opinion");

            migrationBuilder.DropForeignKey(
                name: "FK_Opinion_Products_ProductId",
                table: "Opinion");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryAddress_DeliveryAddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payment_PaymentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Customer_CustomerId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Opinion",
                table: "Opinion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryAddress",
                table: "DeliveryAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerAddress",
                table: "CustomerAddress");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Opinion",
                newName: "Opinions");

            migrationBuilder.RenameTable(
                name: "DeliveryAddress",
                newName: "DeliveryAddresses");

            migrationBuilder.RenameTable(
                name: "CustomerAddress",
                newName: "CustomerAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_CustomerId",
                table: "Payments",
                newName: "IX_Payments_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Opinion_ProductId",
                table: "Opinions",
                newName: "IX_Opinions_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Opinion_CustomerId",
                table: "Opinions",
                newName: "IX_Opinions_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Opinions",
                table: "Opinions",
                column: "OpinionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryAddresses",
                table: "DeliveryAddresses",
                column: "DeliveryAddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerAddresses",
                table: "CustomerAddresses",
                column: "CustomerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerAddresses_CustomerAddressId",
                table: "Customer",
                column: "CustomerAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "CustomerAddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_Customer_CustomerId",
                table: "Opinions",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_Products_ProductId",
                table: "Opinions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryAddresses_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId",
                principalTable: "DeliveryAddresses",
                principalColumn: "DeliveryAddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Customer_CustomerId",
                table: "Payments",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerAddresses_CustomerAddressId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_Customer_CustomerId",
                table: "Opinions");

            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_Products_ProductId",
                table: "Opinions");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryAddresses_DeliveryAddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Customer_CustomerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Opinions",
                table: "Opinions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryAddresses",
                table: "DeliveryAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerAddresses",
                table: "CustomerAddresses");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Opinions",
                newName: "Opinion");

            migrationBuilder.RenameTable(
                name: "DeliveryAddresses",
                newName: "DeliveryAddress");

            migrationBuilder.RenameTable(
                name: "CustomerAddresses",
                newName: "CustomerAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_CustomerId",
                table: "Payment",
                newName: "IX_Payment_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Opinions_ProductId",
                table: "Opinion",
                newName: "IX_Opinion_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Opinions_CustomerId",
                table: "Opinion",
                newName: "IX_Opinion_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Opinion",
                table: "Opinion",
                column: "OpinionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryAddress",
                table: "DeliveryAddress",
                column: "DeliveryAddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerAddress",
                table: "CustomerAddress",
                column: "CustomerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerAddress_CustomerAddressId",
                table: "Customer",
                column: "CustomerAddressId",
                principalTable: "CustomerAddress",
                principalColumn: "CustomerAddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinion_Customer_CustomerId",
                table: "Opinion",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinion_Products_ProductId",
                table: "Opinion",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryAddress_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId",
                principalTable: "DeliveryAddress",
                principalColumn: "DeliveryAddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payment_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Customer_CustomerId",
                table: "Payment",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

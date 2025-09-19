using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestmentApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "INVESTMENTS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TYPE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    BASE_VALUE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EXPECTED_YIELD_PERCENT = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    RISK_LEVEL = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVESTMENTS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    DATA_NASCIMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ROLE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_INVESTMENTS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    USER_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    INVESTMENT_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    AMOUNT_INVESTED = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UNITS = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PURCHASE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CURRENT_VALUE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_INVESTMENTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USER_INVESTMENTS_INVESTMENTS_INVESTMENT_ID",
                        column: x => x.INVESTMENT_ID,
                        principalTable: "INVESTMENTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USER_INVESTMENTS_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USER_INVESTMENTS_INVESTMENT_ID",
                table: "USER_INVESTMENTS",
                column: "INVESTMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_INVESTMENTS_USER_ID",
                table: "USER_INVESTMENTS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_CPF",
                table: "USERS",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_EMAIL",
                table: "USERS",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER_INVESTMENTS");

            migrationBuilder.DropTable(
                name: "INVESTMENTS");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}

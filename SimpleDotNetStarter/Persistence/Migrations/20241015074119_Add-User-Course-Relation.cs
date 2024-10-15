using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleDotNetStarter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCourseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                schema: "users",
                table: "Courses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatorId",
                schema: "users",
                table: "Courses",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_CreatorId",
                schema: "users",
                table: "Courses",
                column: "CreatorId",
                principalSchema: "users",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_CreatorId",
                schema: "users",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CreatorId",
                schema: "users",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                schema: "users",
                table: "Courses");
        }
    }
}

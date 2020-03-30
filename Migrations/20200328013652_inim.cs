using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgoApp.Migrations
{
    public partial class inim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(nullable: true),
                    Analysis = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ChapterId = table.Column<int>(nullable: false),
                    Difficulty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeacherId = table.Column<int>(nullable: false),
                    ClassName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassRooms_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FillingAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FillingAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FillingAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Input = table.Column<string>(nullable: true),
                    Output = table.Column<string>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultTests_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    Correct = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Correct = table.Column<bool>(nullable: false),
                    MyAnswers = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsToClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassRoomId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsToClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentsToClasses_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsToClasses_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 1, "第一章", 0 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 2, "第二章", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NickName", "Password", "Role", "Username" },
                values: new object[] { 1, null, "root", 0, "root" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NickName", "Password", "Role", "Username" },
                values: new object[] { 2, "老師", "t", 1, "t" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NickName", "Password", "Role", "Username" },
                values: new object[] { 3, "老師2", "t2", 1, "t2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NickName", "Password", "Role", "Username" },
                values: new object[] { 4, "學生", "s", 2, "s" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NickName", "Password", "Role", "Username" },
                values: new object[] { 5, "學生2", "s2", 2, "s2" });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "ClassName", "TeacherId" },
                values: new object[] { 1, "t的Class", 2 });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "ClassName", "TeacherId" },
                values: new object[] { 2, "t2的Class", 3 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 1, "無", 1, "選擇正確答案", 0, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 2, "無", 1, "選擇正確答案", 0, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 3, "無", 2, "選擇正確答案", 0, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 4, "無", 2, "選擇正確答案", 0, 0 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 1, "錯誤", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 15, "錯誤", false, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 14, "錯誤", false, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 13, "錯誤", false, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 12, "正確", true, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 11, "錯誤", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 10, "錯誤", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 16, "正確", true, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 9, "錯誤", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 7, "錯誤", false, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 6, "錯誤", false, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 5, "錯誤", false, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 4, "正確", true, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 3, "錯誤", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 2, "錯誤", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 8, "正確", true, 2 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 1, 1, 4 });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_TeacherId",
                table: "ClassRooms",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_FillingAnswers_QuestionId",
                table: "FillingAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ChapterId",
                table: "Questions",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTests_QuestionId",
                table: "ResultTests",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectionOptions_QuestionId",
                table: "SelectionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsToClasses_ClassRoomId",
                table: "StudentsToClasses",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsToClasses_StudentId",
                table: "StudentsToClasses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserId",
                table: "UserAnswers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FillingAnswers");

            migrationBuilder.DropTable(
                name: "ResultTests");

            migrationBuilder.DropTable(
                name: "SelectionOptions");

            migrationBuilder.DropTable(
                name: "StudentsToClasses");

            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Chapters");
        }
    }
}

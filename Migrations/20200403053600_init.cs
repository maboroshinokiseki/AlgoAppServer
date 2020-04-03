using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgoApp.Migrations
{
    public partial class init : Migration
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
                    Role = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    BirthDay = table.Column<DateTime>(nullable: false)
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
                columns: new[] { "Id", "BirthDay", "Gender", "NickName", "Password", "Points", "Role", "Username" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, "root", 0, 0, "root" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "NickName", "Password", "Points", "Role", "Username" },
                values: new object[] { 2, new DateTime(2000, 4, 3, 13, 36, 0, 280, DateTimeKind.Local).AddTicks(5069), 1, "赵老师", "123", 0, 1, "teacherzhao" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "NickName", "Password", "Points", "Role", "Username" },
                values: new object[] { 3, new DateTime(2000, 4, 3, 13, 36, 0, 281, DateTimeKind.Local).AddTicks(4353), 2, "钱老师", "123", 0, 1, "teacherqian" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "NickName", "Password", "Points", "Role", "Username" },
                values: new object[] { 4, new DateTime(2000, 4, 3, 13, 36, 0, 281, DateTimeKind.Local).AddTicks(4406), 1, "孙同学", "123", 0, 2, "studentsun" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "NickName", "Password", "Points", "Role", "Username" },
                values: new object[] { 5, new DateTime(2000, 4, 3, 13, 36, 0, 281, DateTimeKind.Local).AddTicks(4429), 2, "李同学", "123", 0, 2, "studentli" });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "ClassName", "TeacherId" },
                values: new object[] { 1, "赵老师的班级", 2 });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "ClassName", "TeacherId" },
                values: new object[] { 2, "钱老师的班级", 3 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 1, "无", 1, "采用邻接表存储的图的广度优先遍历算法类似于二叉树的（）。", 0, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 2, "判断链表有没有环，可以用快慢指针来实现，两指针的移动速度不一样。如果相遇，则表示有环，否则表示无环。", 1, "如果使用比较高效的算法判断单链表有没有环的算法中，至少需要几个指针？", 1, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 3, "有向图是n，无向图是n-1。", 2, "要连通具有n个顶点的有向图,至少需要（）条边？", 2, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 4, "本题考点是有向图中顶点度的概念。有向图的某个顶点v，把以v为终点的边的数目，称为v的入度；以v为始点的边的数目，称为v的出度；v的度则定义为该顶点的入度和出度之和。因此，本题参考答案是C。", 2, "有向图的一个顶点的度为该顶点的（）。", 3, 0 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 1, "先序遍历", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 15, "入度与出度之和", true, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 14, "出度", false, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 13, "入度", false, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 12, "2n", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 11, "n+1", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 10, "n", true, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 16, "(入度＋出度)/2", false, 4 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 9, "n-1", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 7, "2个", true, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 6, "1个", false, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 5, "不需要", false, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 4, "按层遍历", true, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 3, "后序遍历", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 2, "中序遍历", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 8, "3个", false, 2 });

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

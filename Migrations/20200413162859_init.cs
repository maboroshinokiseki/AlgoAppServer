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
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Nickname = table.Column<string>(nullable: true),
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
                name: "DailyPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPoints_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyPractices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPractices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPractices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageType = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Read = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookmarks_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookmarks_Users_UserId",
                        column: x => x.UserId,
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
                values: new object[] { 1, "绪论", 1 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 11, "外部排序", 11 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 10, "内部排序", 10 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 8, "动态存储管理", 8 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 7, "图", 7 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 9, "查找", 9 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 5, "数组和广义表", 5 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 4, "串", 4 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 3, "栈和队列", 3 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 2, "线性表", 2 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[] { 6, "树和二叉树", 6 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 10, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9664), 2, "冯同学", "123", 128, 2, "student10" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 15, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9677), 2, "沈同学", "123", 0, 2, "student11" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 14, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9675), 2, "蒋同学", "123", 0, 2, "student10" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 13, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9672), 2, "卫同学", "123", 1024, 2, "student13" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 12, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9670), 2, "褚同学", "123", 512, 2, "student12" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 11, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9667), 2, "陈同学", "123", 256, 2, "student11" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 9, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9661), 2, "王同学", "123", 64, 2, "student09" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 3, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9112), 2, "钱老师", "123", 0, 1, "teacherqian" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 7, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9656), 2, "吴同学", "123", 16, 2, "student07" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 6, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9653), 2, "周同学", "123", 8, 2, "student06" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 5, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9625), 2, "李同学", "123", 4, 2, "studentli" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 4, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9141), 1, "孙同学", "123", 2, 2, "studentsun" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 16, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9680), 2, "韩同学", "123", 0, 2, "student12" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 2, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(156), 1, "赵老师", "123", 0, 1, "teacherzhao" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, "root", 0, 0, "root" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 8, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9659), 2, "郑同学", "123", 32, 2, "student08" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDay", "Gender", "Nickname", "Password", "Points", "Role", "Username" },
                values: new object[] { 17, new DateTime(2000, 4, 14, 0, 28, 58, 596, DateTimeKind.Local).AddTicks(9682), 2, "杨同学", "123", 0, 2, "student13" });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "ClassName", "TeacherId" },
                values: new object[] { 1, "赵老师的班级_计科", 2 });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "ClassName", "TeacherId" },
                values: new object[] { 2, "赵老师的班级_电信", 2 });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "ClassName", "TeacherId" },
                values: new object[] { 3, "钱老师的班级", 3 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 1, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 1, 4 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 2, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 2, 5 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 3, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 4, 6 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 4, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 8, 7 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 5, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 16, 8 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 6, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 32, 9 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 7, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 64, 10 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 8, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 128, 11 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 9, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 256, 12 });

            migrationBuilder.InsertData(
                table: "DailyPoints",
                columns: new[] { "Id", "Date", "Points", "UserId" },
                values: new object[] { 10, new DateTime(2020, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), 512, 13 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 1, "无", 1, "算法的时间复杂度是指（）。", 0, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 2, "无", 1, "算法的空间复杂度是指（）。", 0, 0 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Analysis", "ChapterId", "Content", "Difficulty", "Type" },
                values: new object[] { 3, "无", 1, "以下哪个不是算法的重要特性（）。", 0, 0 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 1, "开发算法所花费的时间", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 12, "易用性", true, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 11, "可行性", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 10, "确定性", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 8, "以上都不对", true, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 7, "程序执行所需内存大小", false, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 9, "有穷性", false, 3 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 5, "算法代码的大小", false, 2 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 4, "以上都不对", true, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 3, "程序执行所需时间", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 2, "学习算法所需要的时间", false, 1 });

            migrationBuilder.InsertData(
                table: "SelectionOptions",
                columns: new[] { "Id", "Content", "Correct", "QuestionId" },
                values: new object[] { 6, "算法所能处理数据的大小", false, 2 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 8, 1, 11 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 12, 2, 15 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 11, 2, 14 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 10, 1, 13 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 9, 1, 12 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 7, 1, 10 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 1, 1, 4 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 5, 1, 8 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 4, 1, 7 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 3, 1, 6 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 2, 1, 5 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 13, 2, 16 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 6, 1, 9 });

            migrationBuilder.InsertData(
                table: "StudentsToClasses",
                columns: new[] { "Id", "ClassRoomId", "StudentId" },
                values: new object[] { 14, 2, 17 });

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_QuestionId",
                table: "Bookmarks",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UserId",
                table: "Bookmarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_TeacherId",
                table: "ClassRooms",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPoints_UserId",
                table: "DailyPoints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPractices_UserId",
                table: "DailyPractices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FillingAnswers_QuestionId",
                table: "FillingAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

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
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "DailyPoints");

            migrationBuilder.DropTable(
                name: "DailyPractices");

            migrationBuilder.DropTable(
                name: "FillingAnswers");

            migrationBuilder.DropTable(
                name: "Messages");

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

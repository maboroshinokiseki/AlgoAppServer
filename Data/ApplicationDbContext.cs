using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace AlgoApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<FillingAnswer> FillingAnswers { get; set; }
        public DbSet<SelectionOption> SelectionOptions { get; set; }
        public DbSet<ResultTest> ResultTests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<StudentToClass> StudentsToClasses { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<DailyPoints> DailyPoints { get; set; }
        public DbSet<DailyPractice> DailyPractices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var valueComparer = new ValueComparer<List<string>>(true);
            modelBuilder.Entity<UserAnswer>()
                .Property(a => a.MyAnswers)
                .HasConversion(a => JsonSerializer.Serialize(a, null), a => JsonSerializer.Deserialize<List<string>>(a, null))
                .Metadata.SetValueComparer(valueComparer);
            var defaultPassword = Utilities.HashPassword("123");
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "root", Nickname = "Root", Password = Utilities.HashPassword("root"), Role = UserRole.Admin, Gender = Gender.Secrecy },
                new User { Id = 2, Username = "teacherzhao", Password = defaultPassword, Nickname = "赵老师", Role = UserRole.Teacher, Gender = Gender.Male, BirthDay = DateTime.Now.AddYears(-20) },
                new User { Id = 3, Username = "teacherqian", Password = defaultPassword, Nickname = "钱老师", Role = UserRole.Teacher, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20) },
                new User { Id = 4, Username = "studentsun", Password = defaultPassword, Nickname = "孙同学", Role = UserRole.Student, Gender = Gender.Male, BirthDay = DateTime.Now.AddYears(-20), Points = 2 },
                new User { Id = 5, Username = "studentli", Password = defaultPassword, Nickname = "李同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 4 },
                new User { Id = 6, Username = "student06", Password = defaultPassword, Nickname = "周同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 8 },
                new User { Id = 7, Username = "student07", Password = defaultPassword, Nickname = "吴同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 16 },
                new User { Id = 8, Username = "student08", Password = defaultPassword, Nickname = "郑同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 32 },
                new User { Id = 9, Username = "student09", Password = defaultPassword, Nickname = "王同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 64 },
                new User { Id = 10, Username = "student10", Password = defaultPassword, Nickname = "冯同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 128 },
                new User { Id = 11, Username = "student11", Password = defaultPassword, Nickname = "陈同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 256 },
                new User { Id = 12, Username = "student12", Password = defaultPassword, Nickname = "褚同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 512 },
                new User { Id = 13, Username = "student13", Password = defaultPassword, Nickname = "卫同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 1024 },
                new User { Id = 14, Username = "student10", Password = defaultPassword, Nickname = "蒋同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 0 },
                new User { Id = 15, Username = "student11", Password = defaultPassword, Nickname = "沈同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 0 },
                new User { Id = 16, Username = "student12", Password = defaultPassword, Nickname = "韩同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 0 },
                new User { Id = 17, Username = "student13", Password = defaultPassword, Nickname = "杨同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20), Points = 0 }
                );
            modelBuilder.Entity<Chapter>().HasData(
                new Chapter { Id = 1, Name = "绪论", Order = 1 },
                new Chapter { Id = 2, Name = "线性表", Order = 2 },
                new Chapter { Id = 3, Name = "栈和队列", Order = 3 },
                new Chapter { Id = 4, Name = "串", Order = 4 },
                new Chapter { Id = 5, Name = "数组和广义表", Order = 5 },
                new Chapter { Id = 6, Name = "树和二叉树", Order = 6 },
                new Chapter { Id = 7, Name = "图", Order = 7 },
                new Chapter { Id = 8, Name = "动态存储管理", Order = 8 },
                new Chapter { Id = 9, Name = "查找", Order = 9 },
                new Chapter { Id = 10, Name = "内部排序", Order = 10 },
                new Chapter { Id = 11, Name = "外部排序", Order = 11 }
                );
            modelBuilder.Entity<Question>().HasData(new Question { Id = 1, ChapterId = 1, Content = "算法的时间复杂度是指（）。", Analysis = "无", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<SelectionOption>().HasData(
                new SelectionOption { Id = 1, QuestionId = 1, Correct = false, Content = "开发算法所花费的时间" },
                new SelectionOption { Id = 2, QuestionId = 1, Correct = false, Content = "学习算法所需要的时间" },
                new SelectionOption { Id = 3, QuestionId = 1, Correct = false, Content = "程序执行所需时间" },
                new SelectionOption { Id = 4, QuestionId = 1, Correct = true, Content = "以上都不对" }
                );
            modelBuilder.Entity<Question>().HasData(new Question { Id = 2, ChapterId = 1, Content = "算法的空间复杂度是指（）。", Analysis = "无", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<SelectionOption>().HasData(
                new SelectionOption { Id = 5, QuestionId = 2, Correct = false, Content = "算法代码的大小" },
                new SelectionOption { Id = 6, QuestionId = 2, Correct = false, Content = "算法所能处理数据的大小" },
                new SelectionOption { Id = 7, QuestionId = 2, Correct = false, Content = "程序执行所需内存大小" },
                new SelectionOption { Id = 8, QuestionId = 2, Correct = true, Content = "以上都不对" }
                );
            modelBuilder.Entity<Question>().HasData(new Question { Id = 3, ChapterId = 1, Content = "以下哪个不是算法的重要特性（）。", Analysis = "无", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<SelectionOption>().HasData(
                new SelectionOption { Id = 9, QuestionId = 3, Correct = false, Content = "有穷性" },
                new SelectionOption { Id = 10, QuestionId = 3, Correct = false, Content = "确定性" },
                new SelectionOption { Id = 11, QuestionId = 3, Correct = false, Content = "可行性" },
                new SelectionOption { Id = 12, QuestionId = 3, Correct = true, Content = "易用性" }
                );
            //modelBuilder.Entity<Question>().HasData(new Question { Id = 1, ChapterId = 1, Content = "采用邻接表存储的图的广度优先遍历算法类似于二叉树的（）。", Analysis = "无", Type = QuestionType.SingleSelection, Difficulty = 0 });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 1, QuestionId = 1, Correct = false, Content = "先序遍历" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 2, QuestionId = 1, Correct = false, Content = "中序遍历" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 3, QuestionId = 1, Correct = false, Content = "后序遍历" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 4, QuestionId = 1, Correct = true, Content = "按层遍历" });
            //modelBuilder.Entity<Question>().HasData(new Question { Id = 2, ChapterId = 1, Content = "如果使用比较高效的算法判断单链表有没有环的算法中，至少需要几个指针？", Analysis = "判断链表有没有环，可以用快慢指针来实现，两指针的移动速度不一样。如果相遇，则表示有环，否则表示无环。", Type = QuestionType.SingleSelection, Difficulty = 1 });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 5, QuestionId = 2, Correct = false, Content = "不需要" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 6, QuestionId = 2, Correct = false, Content = "1个" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 7, QuestionId = 2, Correct = true, Content = "2个" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 8, QuestionId = 2, Correct = false, Content = "3个" });
            //modelBuilder.Entity<Question>().HasData(new Question { Id = 3, ChapterId = 2, Content = "要连通具有n个顶点的有向图,至少需要（）条边？", Analysis = "有向图是n，无向图是n-1。", Type = QuestionType.SingleSelection, Difficulty = 2 });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 9, QuestionId = 3, Correct = false, Content = "n-1" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 10, QuestionId = 3, Correct = true, Content = "n" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 11, QuestionId = 3, Correct = false, Content = "n+1" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 12, QuestionId = 3, Correct = false, Content = "2n" });
            //modelBuilder.Entity<Question>().HasData(new Question { Id = 4, ChapterId = 2, Content = "有向图的一个顶点的度为该顶点的（）。", Analysis = "本题考点是有向图中顶点度的概念。有向图的某个顶点v，把以v为终点的边的数目，称为v的入度；以v为始点的边的数目，称为v的出度；v的度则定义为该顶点的入度和出度之和。因此，本题答案是 入度与出度之和。", Type = QuestionType.SingleSelection, Difficulty = 3 });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 13, QuestionId = 4, Correct = false, Content = "入度" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 14, QuestionId = 4, Correct = false, Content = "出度" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 15, QuestionId = 4, Correct = true, Content = "入度与出度之和" });
            //modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 16, QuestionId = 4, Correct = false, Content = "(入度＋出度)/2" });
            modelBuilder.Entity<Classroom>().HasData(
                new Classroom { Id = 1, TeacherId = 2, ClassName = "赵老师的班级_计科" },
                new Classroom { Id = 2, TeacherId = 2, ClassName = "赵老师的班级_电信" },
                new Classroom { Id = 3, TeacherId = 3, ClassName = "钱老师的班级" }
                );
            modelBuilder.Entity<StudentToClass>().HasData(
                new StudentToClass { Id = 1, ClassroomId = 1, StudentId = 4 },
                new StudentToClass { Id = 2, ClassroomId = 1, StudentId = 5 },
                new StudentToClass { Id = 3, ClassroomId = 1, StudentId = 6 },
                new StudentToClass { Id = 4, ClassroomId = 1, StudentId = 7 },
                new StudentToClass { Id = 5, ClassroomId = 1, StudentId = 8 },
                new StudentToClass { Id = 6, ClassroomId = 1, StudentId = 9 },
                new StudentToClass { Id = 7, ClassroomId = 1, StudentId = 10 },
                new StudentToClass { Id = 8, ClassroomId = 1, StudentId = 11 },
                new StudentToClass { Id = 9, ClassroomId = 1, StudentId = 12 },
                new StudentToClass { Id = 10, ClassroomId = 1, StudentId = 13 },
                new StudentToClass { Id = 11, ClassroomId = 2, StudentId = 14 },
                new StudentToClass { Id = 12, ClassroomId = 2, StudentId = 15 },
                new StudentToClass { Id = 13, ClassroomId = 2, StudentId = 16 },
                new StudentToClass { Id = 14, ClassroomId = 2, StudentId = 17 }
                );

            modelBuilder.Entity<DailyPoints>().HasData(
                new DailyPoints { Id = 1, Date = DateTime.Today.AddDays(-1), UserId = 4, Points = 1 },
                new DailyPoints { Id = 2, Date = DateTime.Today.AddDays(-1), UserId = 5, Points = 2 },
                new DailyPoints { Id = 3, Date = DateTime.Today.AddDays(-1), UserId = 6, Points = 4 },
                new DailyPoints { Id = 4, Date = DateTime.Today.AddDays(-1), UserId = 7, Points = 8 },
                new DailyPoints { Id = 5, Date = DateTime.Today.AddDays(-1), UserId = 8, Points = 16 },
                new DailyPoints { Id = 6, Date = DateTime.Today.AddDays(-1), UserId = 9, Points = 32 },
                new DailyPoints { Id = 7, Date = DateTime.Today.AddDays(-1), UserId = 10, Points = 64 },
                new DailyPoints { Id = 8, Date = DateTime.Today.AddDays(-1), UserId = 11, Points = 128 },
                new DailyPoints { Id = 9, Date = DateTime.Today.AddDays(-1), UserId = 12, Points = 256 },
                new DailyPoints { Id = 10, Date = DateTime.Today.AddDays(-1), UserId = 13, Points = 512 }
                );
        }
    }
}

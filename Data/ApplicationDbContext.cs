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
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<StudentToClass> StudentsToClasses { get; set; }
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
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "root", Password = "root", Role = UserRole.Admin, Gender = Gender.Secrecy });
            modelBuilder.Entity<User>().HasData(new User { Id = 2, Username = "teacherzhao", Password = "123", NickName = "赵老师", Role = UserRole.Teacher, Gender = Gender.Male, BirthDay = DateTime.Now.AddYears(-20) });
            modelBuilder.Entity<User>().HasData(new User { Id = 3, Username = "teacherqian", Password = "123", NickName = "钱老师", Role = UserRole.Teacher, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20) });
            modelBuilder.Entity<User>().HasData(new User { Id = 4, Username = "studentsun", Password = "123", NickName = "孙同学", Role = UserRole.Student, Gender = Gender.Male, BirthDay = DateTime.Now.AddYears(-20) });
            modelBuilder.Entity<User>().HasData(new User { Id = 5, Username = "studentli", Password = "123", NickName = "李同学", Role = UserRole.Student, Gender = Gender.Female, BirthDay = DateTime.Now.AddYears(-20) });
            modelBuilder.Entity<Chapter>().HasData(new Chapter { Id = 1, Name = "第一章", Order = 0 });
            modelBuilder.Entity<Chapter>().HasData(new Chapter { Id = 2, Name = "第二章", Order = 1 });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 1, ChapterId = 1, Content = "采用邻接表存储的图的广度优先遍历算法类似于二叉树的（）。", Analysis = "无", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 1, QuestionId = 1, Correct = false, Content = "先序遍历" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 2, QuestionId = 1, Correct = false, Content = "中序遍历" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 3, QuestionId = 1, Correct = false, Content = "后序遍历" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 4, QuestionId = 1, Correct = true, Content = "按层遍历" });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 2, ChapterId = 1, Content = "如果使用比较高效的算法判断单链表有没有环的算法中，至少需要几个指针？", Analysis = "判断链表有没有环，可以用快慢指针来实现，两指针的移动速度不一样。如果相遇，则表示有环，否则表示无环。", Type = QuestionType.SingleSelection, Difficulty = 1 });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 5, QuestionId = 2, Correct = false, Content = "不需要" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 6, QuestionId = 2, Correct = false, Content = "1个" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 7, QuestionId = 2, Correct = true, Content = "2个" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 8, QuestionId = 2, Correct = false, Content = "3个" });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 3, ChapterId = 2, Content = "要连通具有n个顶点的有向图,至少需要（）条边？", Analysis = "有向图是n，无向图是n-1。", Type = QuestionType.SingleSelection, Difficulty = 2 });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 9, QuestionId = 3, Correct = false, Content = "n-1" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 10, QuestionId = 3, Correct = true, Content = "n" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 11, QuestionId = 3, Correct = false, Content = "n+1" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 12, QuestionId = 3, Correct = false, Content = "2n" });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 4, ChapterId = 2, Content = "有向图的一个顶点的度为该顶点的（）。", Analysis = "本题考点是有向图中顶点度的概念。有向图的某个顶点v，把以v为终点的边的数目，称为v的入度；以v为始点的边的数目，称为v的出度；v的度则定义为该顶点的入度和出度之和。因此，本题答案是 入度与出度之和。", Type = QuestionType.SingleSelection, Difficulty = 3 });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 13, QuestionId = 4, Correct = false, Content = "入度" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 14, QuestionId = 4, Correct = false, Content = "出度" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 15, QuestionId = 4, Correct = true, Content = "入度与出度之和" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 16, QuestionId = 4, Correct = false, Content = "(入度＋出度)/2" });
            modelBuilder.Entity<ClassRoom>().HasData(new ClassRoom { Id = 1, TeacherId = 2, ClassName = "赵老师的班级" });
            modelBuilder.Entity<ClassRoom>().HasData(new ClassRoom { Id = 2, TeacherId = 3, ClassName = "钱老师的班级" });
            modelBuilder.Entity<StudentToClass>().HasData(new StudentToClass { Id = 1, ClassRoomId = 1, StudentId = 4 });
        }
    }
}

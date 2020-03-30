using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "root", Password = "root", Role = UserRole.Admin });
            modelBuilder.Entity<User>().HasData(new User { Id = 2, Username = "t", Password = "t",NickName="老師", Role = UserRole.Teacher });
            modelBuilder.Entity<User>().HasData(new User { Id = 3, Username = "t2", Password = "t2", NickName = "老師2", Role = UserRole.Teacher });
            modelBuilder.Entity<User>().HasData(new User { Id = 4, Username = "s", Password = "s",NickName="學生", Role = UserRole.Student });
            modelBuilder.Entity<User>().HasData(new User { Id = 5, Username = "s2", Password = "s2", NickName = "學生2", Role = UserRole.Student });
            modelBuilder.Entity<Chapter>().HasData(new Chapter { Id = 1, Name = "第一章", Order = 0 });
            modelBuilder.Entity<Chapter>().HasData(new Chapter { Id = 2, Name = "第二章", Order = 1 });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 1, ChapterId = 1, Content = "選擇正確答案", Analysis = "無", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 2, ChapterId = 1, Content = "選擇正確答案", Analysis = "無", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 3, ChapterId = 2, Content = "選擇正確答案", Analysis = "無", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<Question>().HasData(new Question { Id = 4, ChapterId = 2, Content = "選擇正確答案", Analysis = "無", Type = QuestionType.SingleSelection, Difficulty = 0 });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 1, QuestionId = 1, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 2, QuestionId = 1, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 3, QuestionId = 1, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 4, QuestionId = 1, Correct = true, Content = "正確" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 5, QuestionId = 2, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 6, QuestionId = 2, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 7, QuestionId = 2, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 8, QuestionId = 2, Correct = true, Content = "正確" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 9, QuestionId = 3, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 10, QuestionId = 3, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 11, QuestionId = 3, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 12, QuestionId = 3, Correct = true, Content = "正確" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 13, QuestionId = 4, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 14, QuestionId = 4, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 15, QuestionId = 4, Correct = false, Content = "錯誤" });
            modelBuilder.Entity<SelectionOption>().HasData(new SelectionOption { Id = 16, QuestionId = 4, Correct = true, Content = "正確" });
            modelBuilder.Entity<ClassRoom>().HasData(new ClassRoom { Id = 1, TeacherId = 2, ClassName = "t的Class" });
            modelBuilder.Entity<ClassRoom>().HasData(new ClassRoom { Id = 2, TeacherId = 3, ClassName = "t2的Class" });
            modelBuilder.Entity<StudentToClass>().HasData(new StudentToClass { Id = 1, ClassRoomId = 1, StudentId = 4 });
        }
    }
}

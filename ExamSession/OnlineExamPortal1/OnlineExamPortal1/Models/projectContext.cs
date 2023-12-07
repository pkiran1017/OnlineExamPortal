using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlineExamPortal1.Models
{
    public partial class projectContext : DbContext
    {
        public projectContext()
        {
        }

        public projectContext(DbContextOptions<projectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<Topic> Topics { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=VWVITHLPUB5272\\SQLEXPRESS;Database=project;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("Exam");

                entity.Property(e => e.ExamId).HasColumnName("ExamID");

                entity.Property(e => e.ExamEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExamStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.TopicId).HasColumnName("TopicID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK__Exam__TopicID__5165187F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Exam__UserID__5070F446");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.CorrectOption)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DifficultyLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Option1)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Option2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Option3)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Option4)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Option5)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Question1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Question");

                entity.Property(e => e.TopicId).HasColumnName("TopicID");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK__Questions__Topic__4D94879B");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Result");

                entity.Property(e => e.ResultId).HasColumnName("ResultID");

                entity.Property(e => e.CorrectOption)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExamId).HasColumnName("ExamID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.Result1).HasColumnName("Result");

                entity.Property(e => e.SelectedOption)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK__Result__ExamID__5441852A");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__Result__Question__5535A963");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.Property(e => e.TopicId).HasColumnName("TopicID");

                entity.Property(e => e.TopicName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

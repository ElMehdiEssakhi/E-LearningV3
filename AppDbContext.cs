using E_LearningV3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace E_LearningV3
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
            // Optional: custom configuration
        }
        // ⭐️ Add your new DbSet properties here ⭐️
        public DbSet<Professor> Professors { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Chapter> Chapters { get; set; } = null!;
        public DbSet<Content> Contents { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<ExamFinal> ExamFinales { get; set; } = null!;
        public DbSet<Score> Scores { get; set; } = null!;
        public DbSet<Certificate> Certificates { get; set; } = null!;
        //public DbSet<StudentChapterProgress> StudentChapterProgress { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // 1. Student -> Certificate: Change to Restrict
            builder.Entity<Certificate>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Certificates)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            // 2. Course -> Certificate: Change to Restrict
            builder.Entity<Certificate>()
                .HasOne(c => c.Course)
                .WithMany(cr => cr.Certificates)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            // 3. Student -> Enrollment: Change to Restrict (Another frequent conflict source)
            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            // 4. Student -> Score: Change to Restrict
            builder.Entity<Score>()
                .HasOne(s => s.Student)
                .WithMany(st => st.Scores)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

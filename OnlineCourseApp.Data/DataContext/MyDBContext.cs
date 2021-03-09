using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.Models.Account;
using OnlineCourseApp.Data.Models.Announcement;

namespace OnlineCourseApp.Data.EF
{
    public partial class MyDBContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
             base.OnModelCreating(builder);
            builder.Entity<DocumentShare>()
                .HasKey(a => new { a.ID, a.CourseID, a.DocumentID });

            builder.Entity<DocumentShare>().Property(d => d.ID).UseIdentityColumn();
        }
        public DbSet<Region> Region { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseType> CourseType { get; set; }
        public DbSet<CourseSection> CourseSection { get; set; }
        public DbSet<UserLog> UserLog { get; set; }
        public DbSet<Announcements> Announcement { get; set; }
        public DbSet<AnnouncementFilter> AnnouncementFilter { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<CourseParticipants> CourseParticipants { get; set; }
        public DbSet<Exam> Exam { get; set; }
        public DbSet<CourseGroup> CourseGroup { get; set; }
        public DbSet<CourseGroupStudent> CourseGroupStudent { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentShare> DocumentShare { get; set; }
        public DbSet<QuestionCategory> QuestionCategory { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Choice> Choice { get; set; }
        public DbSet<QuestionDuration> QuestionDuration { get; set; }
        public DbSet<ExamAnsweredQuestion> ExamAnsweredQuestion { get; set; }
    }
}

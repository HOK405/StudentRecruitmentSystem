using StudentRecruitment.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentRecruitment.DAL
{
    public class ApidDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApidDbContext(DbContextOptions<ApidDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SemesterInfo> SemesterInfos { get; set; }
        public DbSet<StudentEmployer> StudentEmployers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Employer>().ToTable("Employers");
            builder.Entity<Admin>().ToTable("Admins");

            builder.Entity<Student>(b =>
            {
                b.Property(s => s.Description).IsRequired(false);

                b.HasMany(e => e.SemesterInfos)
                 .WithOne(s => s.Student)
                 .HasForeignKey(si => si.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Employer>(b =>
            {
                b.HasMany<StudentEmployer>()
                 .WithOne(se => se.Employer)
                 .HasForeignKey(se => se.EmployerId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Subject>()
                .HasKey(s => s.Id);
            builder.Entity<Subject>()
                .Property(s => s.Id)
                .ValueGeneratedNever();
            builder.Entity<Subject>()
                .HasMany(s => s.SemesterInfos)
                .WithOne(si => si.Subject)
                .HasForeignKey(si => si.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SemesterInfo>()
                .HasKey(si => new { si.StudentId, si.SubjectId, si.Semester });

            builder.Entity<StudentEmployer>()
                .HasKey(se => new { se.StudentId, se.EmployerId });
            builder.Entity<StudentEmployer>()
                .HasOne(se => se.Student)
                .WithMany(s => s.StudentEmployers)
                .HasForeignKey(se => se.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentEmployer>()
                .HasOne(se => se.Employer)
                .WithMany()
                .HasForeignKey(se => se.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
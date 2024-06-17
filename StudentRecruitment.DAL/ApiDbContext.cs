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

            // Configure tables
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Employer>().ToTable("Employers");
            builder.Entity<Admin>().ToTable("Admins");

            // Configure Student entity
            builder.Entity<Student>(b =>
            {
                b.Property(s => s.Description).IsRequired(false);

                b.HasMany(e => e.SemesterInfos)
                 .WithOne(s => s.Student)
                 .HasForeignKey(si => si.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(s => s.StudentEmployers)
                 .WithOne(se => se.Student)
                 .HasForeignKey(se => se.StudentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Employer entity
            builder.Entity<Employer>(b =>
            {
                b.HasMany(e => e.StudentEmployers)
                 .WithOne(se => se.Employer)
                 .HasForeignKey(se => se.EmployerId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Subject entity
            builder.Entity<Subject>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Id).ValueGeneratedNever();

                b.HasMany(s => s.SemesterInfos)
                 .WithOne(si => si.Subject)
                 .HasForeignKey(si => si.SubjectId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SemesterInfo entity
            builder.Entity<SemesterInfo>(b =>
            {
                b.HasKey(si => new { si.StudentId, si.SubjectId, si.Semester });
            });

            // Configure StudentEmployer entity
            builder.Entity<StudentEmployer>(b =>
            {
                b.HasKey(se => new { se.StudentId, se.EmployerId });

                b.HasOne(se => se.Student)
                 .WithMany(s => s.StudentEmployers)
                 .HasForeignKey(se => se.StudentId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(se => se.Employer)
                 .WithMany(e => e.StudentEmployers) 
                 .HasForeignKey(se => se.EmployerId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
using Microsoft.AspNetCore.Identity;
using StudentRecruitment.BLL.Services;
using StudentRecruitment.DAL;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.DAL.Repositories;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.Utilities;

namespace StudentRecruitment.Api.Extensions
{
    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddIdentity<Student, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApidDbContext>()
            .AddDefaultTokenProviders();

            // Repositories registration
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            // Utils classes
            services.AddScoped<CredentialsGenerator>();

            // BLL services registration
            services.AddScoped<SubjectService>();
            services.AddScoped<StudentService>();

            return services;
        }
    }
}
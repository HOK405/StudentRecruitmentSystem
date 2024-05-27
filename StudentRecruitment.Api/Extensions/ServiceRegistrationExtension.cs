using StudentRecruitment.BLL.Services;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.DAL.Repositories;
using StudentRecruitment.Shared.Utilities;

namespace StudentRecruitment.Api.Extensions
{
    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Repositories registration
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IEmployerRepository, EmployerRepository>();

            // Utils classes
            services.AddScoped<CredentialsGenerator>();

            // BLL services registration
            services.AddScoped<AuthorizationService>();
            services.AddScoped<JwtTokenService>();

            services.AddScoped<SubjectService>();
            services.AddScoped<StudentService>();
            services.AddScoped<AdminService>();
            services.AddScoped<EmployerService>();

            return services;
        }
    }
}
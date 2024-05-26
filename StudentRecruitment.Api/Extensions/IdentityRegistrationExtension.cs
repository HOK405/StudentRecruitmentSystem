using Microsoft.AspNetCore.Identity;
using StudentRecruitment.DAL;
using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.Api.Extensions
{
    public static class IdentityRegistrationExtension
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
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

            return services;
        }
    }
}

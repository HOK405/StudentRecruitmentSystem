using Microsoft.AspNetCore.Identity;
using StudentRecruitment.BLL.DTOs.Output;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.BLL.Services
{
    public class EmployerService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;

        public EmployerService(UserManager<IdentityUser<int>> userManager)
        {
            _userManager = userManager;
        }

        public async Task<EmployerModel> CreateEmployerAsync(CreateEmployerDto creationDto)
        {
            var employer = new Employer
            {
                UserName = creationDto.Username,
                Email = creationDto.Email,
                CompanyName = creationDto.CompanyName,
                Location = creationDto.Location,
                Phone = creationDto.Phone
            };

            var result = await _userManager.CreateAsync(employer, creationDto.Password);

            if (result.Succeeded)
            {
                return new EmployerModel
                {
                    Id = employer.Id,
                    Username = employer.UserName,
                    Email = employer.Email,
                    CompanyName = employer.CompanyName,
                    Location = employer.Location,
                    Phone = employer.Phone
                };
            }

            throw new Exception("Failed to create employer: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
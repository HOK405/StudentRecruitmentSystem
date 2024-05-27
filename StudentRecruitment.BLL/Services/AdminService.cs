using Microsoft.AspNetCore.Identity;
using StudentRecruitment.BLL.DTOs.Output;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.BLL.Services
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public AdminService(IAdminRepository adminRepository, UserManager<IdentityUser<int>> userManager)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
        }

        public async Task<AdminModel> CreateAdminAsync(CreateAdminDto createAdminDto)
        {
            var admin = new Admin
            {
                UserName = createAdminDto.Username,
                Email = createAdminDto.Email
            };

            var result = await _userManager.CreateAsync(admin, createAdminDto.Password);

            if (result.Succeeded)
            {
                return new AdminModel
                {
                    Id = admin.Id,
                    Username = admin.UserName,
                    Email = admin.Email,
                };
            }

            throw new Exception("Failed to create admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
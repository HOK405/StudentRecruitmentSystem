using Microsoft.AspNetCore.Identity;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.BLL.Services
{
    public class AuthorizationService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly IAdminRepository _adminRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IStudentRepository _studentRepository;

        public AuthorizationService(UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager,
                                    IAdminRepository adminRepository, IEmployerRepository employerRepository, IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _adminRepository = adminRepository;
            _employerRepository = employerRepository;
            _studentRepository = studentRepository;
        }

        public async Task<(IdentityUser<int> user, string role)> AuthorizeUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    var admin = await _adminRepository.GetAdminByIdAsync(user.Id);
                    if (admin != null)
                    {
                        return (user, "Admin");
                    }

                    var employer = await _employerRepository.GetEmployerByIdAsync(user.Id);
                    if (employer != null)
                    {
                        return (user, "Employer");
                    }

                    var student = await _studentRepository.GetStudentByIdAsync(user.Id);
                    if (student != null)
                    {
                        return (user, "Student");
                    }
                }
            }
            return (null, "Unknown");
        }
    }

}

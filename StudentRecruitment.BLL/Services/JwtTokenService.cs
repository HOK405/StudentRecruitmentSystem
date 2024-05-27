using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentRecruitment.BLL.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly IAdminRepository _adminRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly byte[] _key;

        public JwtTokenService(
            IConfiguration configuration,
            UserManager<IdentityUser<int>> userManager,
            SignInManager<IdentityUser<int>> signInManager,
            IAdminRepository adminRepository,
            IEmployerRepository employerRepository,
            IStudentRepository studentRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _adminRepository = adminRepository;
            _employerRepository = employerRepository;
            _studentRepository = studentRepository;
            _key = Encoding.ASCII.GetBytes("MyMegaHyperSuperSecretKey123456789012345112312");
        }

        public async Task<string> GenerateAccessTokenAsync(string username, string password)
        {
            var (user, role) = await AuthorizeUserAsync(username, password);

            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            return await GenerateTokenAsync(user, role);
        }

        private async Task<(IdentityUser<int> user, string role)> AuthorizeUserAsync(string username, string password)
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

        private Task<string> GenerateTokenAsync(IdentityUser<int> user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
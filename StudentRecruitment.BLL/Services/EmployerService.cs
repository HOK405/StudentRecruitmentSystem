using Microsoft.AspNetCore.Identity;
using StudentRecruitment.BLL.DTOs.Output;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.BLL.Services
{
    public class EmployerService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IEmployerRepository _employerRepository;

        public EmployerService(UserManager<IdentityUser<int>> userManager, IEmployerRepository employerRepository)
        {
            _userManager = userManager;
            _employerRepository = employerRepository;
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

        public async Task LikeStudentAsync(int employerId, int studentId)
        {
            await _employerRepository.LikeStudentAsync(employerId, studentId);
        }

        public async Task<List<StudentModel>> GetLikedStudentsAsync(int employerId)
        {
            var likedStudents = await _employerRepository.GetLikedStudentsAsync(employerId);
            return likedStudents.Select(s => new StudentModel
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname,
                Patronimic = s.Patronimic,
                BirthDate = s.BirthDate
            }).ToList();
        }

        public async Task DislikeStudentAsync(int employerId, int studentId)
        {
            await _employerRepository.DislikeStudentAsync(employerId, studentId);
        }
    }
}
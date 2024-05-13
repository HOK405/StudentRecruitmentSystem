using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StudentRecruitment.BLL.DTOs;
using StudentRecruitment.BLL.Utilities;
using StudentRecruitment.DAL;
using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.BLL.Services
{
    public class StudentService
    {
        private readonly UserManager<Student> _userManager;
        private readonly ApidDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly CredentialsGenerator _credentialsGenerator;

        public StudentService(
            UserManager<Student> userManager, 
            ApidDbContext context, 
            IConfiguration configuration, 
            CredentialsGenerator credentialsGenerator)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _credentialsGenerator = credentialsGenerator;
        }

        public async Task ImportStudentsFromFileAsync()
        {
            var filePath = _configuration["FilePaths:StudentsJson"];
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path for students data is not defined.");
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var students = JsonConvert.DeserializeObject<List<StudentImportDto>>(jsonData);  
            var credentials = new List<CredentialDto>();

            foreach (var studentDto in students)
            {
                var student = new Student
                {
                    UserName = await _credentialsGenerator.GenerateUsername(studentDto),
                    Email = studentDto.Email,
                    PhoneNumber = studentDto.Phone,
                    BirthDate = DateTime.Parse(studentDto.BirthDate),
                    Name = studentDto.Name,
                    Surname = studentDto.Surname,
                    Patronimic = studentDto.Patronimic,
                    IsPublicProfile = false
                };

                var password = _credentialsGenerator.GeneratePassword();
                var result = await _userManager.CreateAsync(student, password);

                if (result.Succeeded)
                {
                    credentials.Add(new CredentialDto { Username = student.UserName, Password = password });

                    foreach (var semester in studentDto.Semesters) 
                    {
                        foreach (var subject in semester.Subjects) 
                        {
                            _context.SemesterInfos.Add(new SemesterInfo
                            {
                                StudentId = student.Id,
                                SubjectId = subject.Id,
                                Grade = subject.Grade,
                                Semester = semester.SemesterNumber
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }

            await File.WriteAllTextAsync("credentials.json", JsonConvert.SerializeObject(credentials));
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.DTOs;
using StudentRecruitment.Shared.Utilities;

namespace StudentRecruitment.DAL.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApidDbContext _dbContext;
        private readonly CredentialsGenerator _credentialsGenerator;

        public StudentRepository(ApidDbContext dbContext, CredentialsGenerator credentialsGenerator)
        {
            _dbContext = dbContext;
            _credentialsGenerator = credentialsGenerator;
        }
        public async Task<List<CredentialDto>> ImportStudentsAsync(
            List<StudentImportDto> students,
            UserManager<Student> userManager)
        {
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
                var result = await userManager.CreateAsync(student, password);

                if (result.Succeeded)
                {
                    credentials.Add(new CredentialDto { Username = student.UserName, Password = password });

                    foreach (var semester in studentDto.Semesters)
                    {
                        foreach (var subject in semester.Subjects)
                        {
                            _dbContext.SemesterInfos.Add(new SemesterInfo
                            {
                                StudentId = student.Id,
                                SubjectId = subject.Id,
                                Grade = subject.Grade,
                                Semester = semester.SemesterNumber
                            });
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            return credentials;
        }

        // Алгоритм зваженого оціюнвання
        public async Task<List<Student>> GetBestSuitedStudentsAsync(Dictionary<int, int> subjectRatings)
        {
            var studentScores = await _dbContext.SemesterInfos
                .Where(si => subjectRatings.Keys.Contains(si.SubjectId))
                .Select(si => new
                {
                    si.StudentId,
                    si.SubjectId,
                    si.Grade
                })
                .ToListAsync();

            var groupedScores = studentScores
                .GroupBy(si => si.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    TotalScore = g.Sum(si => si.Grade * subjectRatings[si.SubjectId])
                })
                .ToList();

            var studentIds = groupedScores.Select(s => s.StudentId).ToList();

            var students = await _dbContext.Students
                .Where(s => studentIds.Contains(s.Id) && s.IsPublicProfile) 
                .ToListAsync();

            var sortedStudents = students
                .Join(groupedScores, student => student.Id, score => score.StudentId, (student, score) => new
                {
                    Student = student,
                    score.TotalScore
                })
                .OrderByDescending(ss => ss.TotalScore)
                .Select(ss => ss.Student)
                .ToList();

            return sortedStudents;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _dbContext.Students.FindAsync(studentId);
        }

        public async Task DeleteStudentAsync(Student student)
        {
            _dbContext.Students.Remove(student);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
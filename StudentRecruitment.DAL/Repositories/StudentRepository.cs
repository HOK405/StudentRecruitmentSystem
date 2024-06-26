﻿using Microsoft.AspNetCore.Identity;
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
            UserManager<IdentityUser<int>> userManager)
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
                    IsPublicProfile = true
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

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _dbContext.Students.FindAsync(studentId);
        }

        // Алгоритм зваженого оціюнвання
        public async Task<List<Student>> GetBestSuitedStudentsAsync(Dictionary<int, int> subjectRatings)
        {
            // Get all relevant semester info
            var studentScores = await _dbContext.SemesterInfos
                .Where(si => subjectRatings.Keys.Contains(si.SubjectId))
                .ToListAsync();

            // Group by student and subject, then calculate average grade per subject per student
            var groupedScores = studentScores
                .GroupBy(si => new { si.StudentId, si.SubjectId })
                .Select(g => new
                {
                    g.Key.StudentId,
                    g.Key.SubjectId,
                    AverageGrade = g.Average(si => si.Grade)
                })
                .ToList();

            // Calculate the total weighted score for each student
            var totalScores = groupedScores
                .GroupBy(gs => gs.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    TotalScore = g.Sum(gs => gs.AverageGrade * subjectRatings[gs.SubjectId])
                })
                .ToList();

            var studentIds = totalScores.Select(ts => ts.StudentId).ToList();

            var students = await _dbContext.Students
                .Where(s => studentIds.Contains(s.Id) && s.IsPublicProfile)
                .ToListAsync();

            var sortedStudents = students
                .Join(totalScores, student => student.Id, score => score.StudentId, (student, score) => new
                {
                    Student = student,
                    score.TotalScore
                })
                .OrderByDescending(ss => ss.TotalScore)
                .Select(ss => ss.Student)
                .ToList();

            return sortedStudents;
        }


        public async Task<Student> GetStudentWithGradesAsync(int studentId)
        {
            return await _dbContext.Students
                .Include(s => s.SemesterInfos)
                    .ThenInclude(si => si.Subject)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task DeleteAllStudentsAsync()
        {
            // Delete related data from other tables using raw SQL
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM StudentEmployers WHERE StudentId IN (SELECT Id FROM Students)");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM SemesterInfos WHERE StudentId IN (SELECT Id FROM Students)");

            // Delete students from the AspNetUsers table using raw SQL
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUsers WHERE Id IN (SELECT Id FROM Students)");

            // Delete students from the Students table
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Students");

            await _dbContext.SaveChangesAsync();
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
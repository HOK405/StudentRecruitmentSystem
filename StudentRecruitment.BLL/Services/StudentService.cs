﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StudentRecruitment.BLL.DTOs.Output;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.BLL.Services
{
    public class StudentService
    {
        private readonly UserManager<Student> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IStudentRepository _studentRepository;

        public StudentService(
            UserManager<Student> userManager,
            IConfiguration configuration,
            IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _studentRepository = studentRepository;
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

            var credentials = await _studentRepository.ImportStudentsAsync(students, _userManager);

            await File.WriteAllTextAsync("credentials.json", JsonConvert.SerializeObject(credentials));
        }

        public async Task<PagedData<StudentModel>> GetBestSuitedStudentsAsync(List<SubjectRatingDto> subjectRatingDtos, int pageNumber = 1, int pageSize = 30)
        {
            var subjectRatings = subjectRatingDtos.ToDictionary(sr => sr.SubjectId, sr => sr.Rating);

            var students = await _studentRepository.GetBestSuitedStudentsAsync(subjectRatings);

            var totalStudents = students.Count();
            var pagedStudents = students
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var studentDtos = pagedStudents.Select(student => new StudentModel
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                Patronimic = student.Patronimic,
                BirthDate = student.BirthDate
            }).ToList();

            return new PagedData<StudentModel>
            {
                Results = studentDtos,
                TotalCount = totalStudents,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<StudentWithGradesOutputModel> GetStudentWithGradesAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentWithGradesAsync(studentId);
            if (student == null) return null;

            var outputModel = new StudentWithGradesOutputModel
            {
                Name = student.Name,
                Surname = student.Surname,
                Patronimic = student.Patronimic,
                Description = student.Description,
                BirthDate = student.BirthDate,
                SemesterGrades = student.SemesterInfos.Select(si => new SemesterGrade
                {
                    Semester = si.Semester,
                    Grade = si.Grade,
                    SubjectName = si.Subject.Name
                }).ToList()
            };

            return outputModel;
        }
    }
}
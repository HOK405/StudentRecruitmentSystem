using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StudentRecruitment.BLL.DTOs.Output;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.BLL.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IConfiguration _configuration;

        public SubjectService(ISubjectRepository subjectRepository, IConfiguration configuration)
        {
            _subjectRepository = subjectRepository;
            _configuration = configuration;
        }

        public async Task ImportSubjectsFromFileAsync()
        {
            var filePath = _configuration["FilePaths:SubjectsJson"];

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path for subjects data is not defined.");
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var subjects = JsonConvert.DeserializeObject<List<Subject>>(jsonData);

            if (subjects != null)
            {
                await _subjectRepository.AddSubjectsAsync(subjects);
                await _subjectRepository.SaveChangesAsync();
            }
        }

        public async Task<List<SubjectOutputModel>> SearchSubjectsByNameAsync(string name)
        {
            var subjects = await _subjectRepository.GetSubjectsByNameAsync(name);
            return subjects.Select(s => new SubjectOutputModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();
        }

        public async Task<PagedData<SubjectOutputModel>> GetPagedSubjectsAsync(int pageNumber, int pageSize)
        {
            var query = _subjectRepository.GetSubjects();
            var totalCount = await query.CountAsync();
            var results = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(s => new SubjectOutputModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description
                }).ToListAsync();

            return new PagedData<SubjectOutputModel>
            {
                Results = results,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> DeleteAllSubjectsAsync()
        {
            await _subjectRepository.DeleteAllSubjectsAsync();
            return true;
        }
    }
}
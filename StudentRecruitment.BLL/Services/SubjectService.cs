using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    }
}
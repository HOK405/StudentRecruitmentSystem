using System.Text.Json.Serialization;

namespace StudentRecruitment.Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<SemesterInfo> SemesterInfos { get; set; }
    }
}
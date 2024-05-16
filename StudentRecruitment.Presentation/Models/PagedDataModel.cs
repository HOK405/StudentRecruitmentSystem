namespace StudentRecruitment.Presentation.Models
{
    public class PagedDataModel<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

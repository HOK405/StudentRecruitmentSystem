namespace StudentRecruitment.BLL.DTOs.Output
{
    public class PagedData<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

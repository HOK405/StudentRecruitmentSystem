﻿namespace StudentRecruitment.BLL.DTOs
{
    public class SemesterDto
    {
        public int SemesterNumber { get; set; }
        public List<SubjectDto> Subjects { get; set; }
    }
}
namespace ForumAPI.DTOs
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<SubjectDto>? Subjects { get; set; }
    }
}
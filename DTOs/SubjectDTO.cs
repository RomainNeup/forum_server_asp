namespace ForumAPI.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Text { get; set; } = null!;
        public List<MessageDto>? Messages { get; set; }
    }
}

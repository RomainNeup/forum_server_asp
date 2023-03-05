namespace ForumAPI.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public UserDto Owner { get; set; } = null!;
    }
}
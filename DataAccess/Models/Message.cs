using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Repositories.Models
{
    public class Message : BaseOwnedModel
    {

        [Required]
        public string Text { get; set; } = null!;

        public int SubjectId { get; set; }

    }
}
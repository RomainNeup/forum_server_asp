using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Repositories.Models
{
    public class Subject : BaseOwnedModel
    {

        [Required]
        public string Name { get; set; } = null!;

        public string Text { get; set; } = null!;

        public int SectionId { get; set; }

    }
}

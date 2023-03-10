using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Repositories.Models
{
    public class Section : BaseOwnedModel
    {

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int CategoryId { get; set; }

    }
}

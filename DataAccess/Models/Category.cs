using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Repositories.Models
{
    public class Category: BaseModel
    {

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

    }
}

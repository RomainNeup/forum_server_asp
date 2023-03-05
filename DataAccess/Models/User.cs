using Microsoft.AspNetCore.Identity;

namespace ForumAPI.Repositories.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Role { get; set; } = 0;
    }
}
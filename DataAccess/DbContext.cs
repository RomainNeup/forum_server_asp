using Microsoft.EntityFrameworkCore;
using ForumAPI.Repositories.Models;

namespace ForumAPI.Data
{
    public class ForumAPIContext : DbContext
    {
        public ForumAPIContext(DbContextOptions<ForumAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Section> Sections { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired();
        }

    }
}
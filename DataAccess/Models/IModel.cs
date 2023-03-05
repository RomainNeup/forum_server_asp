namespace ForumAPI.Repositories.Models
{
    public interface IModel
    {
        int Id { get; set; }
    }

    public abstract class BaseModel : IModel
    {
        public int Id { get; set; }
    }
}

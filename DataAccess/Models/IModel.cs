namespace ForumAPI.Repositories.Models
{
    public interface IModel
    {
        int Id { get; set; }
    }

    public interface IOwnedModel : IModel
    {
        int OwnerId { get; set; }
    }

    public abstract class BaseModel : IModel
    {
        public int Id { get; set; }
    }

    public abstract class BaseOwnedModel : BaseModel, IOwnedModel
    {
        public int OwnerId { get; set; }
    }
}

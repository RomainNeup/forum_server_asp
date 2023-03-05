using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Services.Interfaces;
using System.Security.Claims;

namespace ForumAPI.Services
{
    public class MessagesService : IServiceBase<Message>
    {
        private readonly IRepositoryBase<Message> _messagesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessagesService(IRepositoryBase<Message> messagesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _messagesRepository = messagesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _messagesRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Message>> GetByParentIdAsync(int subjectId)
        {
            return await _messagesRepository.GetByParentIdAsync(subjectId);
        }

        public async Task<Message> GetByIdAsync(int messageId)
        {
            var existingMessage = await _messagesRepository.GetByIdAsync(messageId);
            if (existingMessage == null)
            {
                throw new ArgumentException($"Message with ID {messageId} not found.");
            }

            return existingMessage;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                message.OwnerId = int.Parse(userId);
            }
            return await _messagesRepository.CreateAsync(message);
        }

        public async Task<Message> UpdateAsync(Message message)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingMessage = await _messagesRepository.GetByIdAsync(message.Id);

            if (existingMessage == null)
            {
                throw new ArgumentException($"Message with ID {message.Id} not found.");
            }
            if (userId != existingMessage.OwnerId.ToString())
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");
            }

            existingMessage.Text = message.Text;
            existingMessage.SubjectId = message.SubjectId;

            return await _messagesRepository.UpdateAsync(existingMessage);
        }

        public async Task<bool> DeleteAsync(int messageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingMessage = await _messagesRepository.GetByIdAsync(messageId);

            if (existingMessage == null)
            {
                throw new ArgumentException($"Message with ID {messageId} not found.");
            }
            if (userId != existingMessage.OwnerId.ToString())
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");
            }

            return await _messagesRepository.DeleteAsync(messageId);
        }
    }
}

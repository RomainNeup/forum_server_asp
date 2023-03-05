using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.Services
{
    public class MessagesService : IServiceBase<Message>
    {
        private readonly IRepositoryBase<Message> _messagesRepository;

        public MessagesService(IRepositoryBase<Message> messagesRepository)
        {
            _messagesRepository = messagesRepository;
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
            return await _messagesRepository.CreateAsync(message);
        }

        public async Task<Message> UpdateAsync(Message message)
        {
            var existingMessage = await _messagesRepository.GetByIdAsync(message.Id);
            if (existingMessage == null)
            {
                throw new ArgumentException($"Message with ID {message.Id} not found.");
            }

            existingMessage.Text = message.Text;
            existingMessage.SubjectId = message.SubjectId;

            return await _messagesRepository.UpdateAsync(existingMessage);
        }

        public async Task<bool> DeleteAsync(int messageId)
        {
            if (await _messagesRepository.GetByIdAsync(messageId) == null) {
                throw new ArgumentException($"Message with ID {messageId} not found.");
            }
            return await _messagesRepository.DeleteAsync(messageId);
        }
    }
}

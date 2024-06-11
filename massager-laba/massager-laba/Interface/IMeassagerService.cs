using massager_laba.Data.DTO;

namespace massager_laba.Interface;

public interface IMeassagerService
{
    Task<List<MessagerDTO>> GetMyMessager(Guid id);
    Task SendMessage(Guid fromUserId, Guid toUserId, string content);
    Task SaveMessage(Guid fromUserId, Guid toUserId, string content, DateTime timestamp);
}
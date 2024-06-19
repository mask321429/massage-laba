using massager_laba.Data.DTO;
using massager_laba.Data.Enum;

namespace massager_laba.Interface;

public interface IMeassagerService
{
    Task<List<MessagerDTO>> GetMyMessager(Guid id);
    Task SendMessage(Guid fromUserId, Guid toUserId, string content, TypeMessage typeMessage);

    Task<List<MessageHistoryDTO>> GetHistoryMeassage(Guid idFromUser, Guid idToUser, int? count);
    //   Task SaveMessage(Guid fromUserId, Guid toUserId, string content, DateTime timestamp);
}
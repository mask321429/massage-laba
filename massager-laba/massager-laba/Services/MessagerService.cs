using massager_laba.Data;
using massager_laba.Data.DTO;
using massager_laba.Data.Model;
using massager_laba.Interface;
using Microsoft.EntityFrameworkCore;

namespace massager_laba.Services;

public class MessagerService : IMeassagerService
{
    private readonly DBContext _dbContext;
    private readonly WebSocketConnectionManager _connectionManager;

    public MessagerService(DBContext dbContext, WebSocketConnectionManager connectionManager)
    {
        _dbContext = dbContext;
        _connectionManager = connectionManager;
    }


    public async Task<List<MessagerDTO>> GetMyMessager(Guid id)
    {
        var messages = await _dbContext.MessagerModels
            .Where(x => x.IdUserFrom == id)
            .ToListAsync();

        if (messages == null || messages.Count == 0)
        {
            throw new KeyNotFoundException("Переписки не найдены");
        }
        
        var userIds = messages.Select(m => m.IdUserWhere).Distinct().ToList();

        var users = await _dbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
        
        var result = new List<MessagerDTO>();

        foreach (var message in messages)
        {
            var user = users.FirstOrDefault(u => u.Id == message.IdUserWhere);
            if (user != null)
            {
                result.Add(new MessagerDTO
                {
                    NameUser = user.Login,
                    IdUserWhere = message.IdUserWhere,
                    DateTimeLastLetter = message.LastLetter,
                    UrlAvatar = user.AvatarUrl,
                    IsCheked = message.IsCheked
                });
            }
        }

        return result;
    }
    
    public async Task SendMessage(Guid fromUserId, Guid toUserId, string content)
    {
        var socket = _connectionManager.GetSocketById(toUserId.ToString());
        if (socket != null && socket.State == System.Net.WebSockets.WebSocketState.Open)
        {
            var messageObject = new
            {
                FromUserId = fromUserId,
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            var messageBytes = System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(messageObject));
            await socket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }
        else
        {
        }
        
        await SaveMessage(fromUserId, toUserId, content, DateTime.UtcNow);
    }

    public async Task SaveMessage(Guid fromUserId, Guid toUserId, string content, DateTime timestamp)
    {
        var message = new MessageInfo
        {
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Content = content,
            Timestamp = timestamp
        };

        await _dbContext.MessageInfos.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }


}
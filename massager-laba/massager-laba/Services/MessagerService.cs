using massager_laba.Data;
using massager_laba.Data.DTO;
using massager_laba.Interface;
using Microsoft.EntityFrameworkCore;

namespace massager_laba.Services;

public class MessagerService : IMeassagerService
{
    private readonly DBContext _dbContext;

    public MessagerService(DBContext dbContext)
    {
        _dbContext = dbContext;
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


}
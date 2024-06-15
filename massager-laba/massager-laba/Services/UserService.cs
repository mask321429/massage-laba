using massager_laba.Data;
using massager_laba.Data.DTO;
using massager_laba.Data.Model;
using massager_laba.Interface;
using Microsoft.EntityFrameworkCore;

namespace massager_laba.Services;

public class UserService : IUserService
{
    private readonly DBContext _dbContext;

    public UserService(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProfileDTO>> GetProfile(Guid? id, string? name)
    {
        IQueryable<UserModel> query = _dbContext.Users;
        
        if (id != null)
        {
            query = query.Where(u => u.Id == id);
        }
        
        List<UserModel> users = await query.ToListAsync();
        
        if (!string.IsNullOrEmpty(name))
        {
            users = users.Where(u => u.Login.ToLower().Contains(name.ToLower())).ToList();
        }
        
        var profiles = users.Select(user => new ProfileDTO
        {
            Login = user.Login,
            DateBirth = user.BirthDate,
            Avatar = user.AvatarUrl
        }).ToList();

        return profiles;
    }


    
    
    public async Task UpdateProfile(Guid id, UpdateProfileDTO updateProfileDto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        if (!string.IsNullOrEmpty(user.AvatarUrl) && File.Exists(user.AvatarUrl))
        {
            File.Delete(user.AvatarUrl);
        }
        
        var avatarUrl = SaveAvatarToLocalDisk(updateProfileDto.Avatar, updateProfileDto.Login);

        user.Login = updateProfileDto.Login;
        user.BirthDate = DateTime.SpecifyKind(updateProfileDto.BirthDate, DateTimeKind.Utc);
        user.AvatarUrl = avatarUrl;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    private string SaveAvatarToLocalDisk(IFormFile avatar, string login)
    {
        var filePath = Path.Combine("/Users/kiselevmaksim/Pictures/photo", $"{login}_{Path.GetRandomFileName()}.jpg");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            avatar.CopyTo(stream);
        }

        return filePath;
    }

}

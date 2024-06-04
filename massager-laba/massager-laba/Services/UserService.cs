using massager_laba.Data;
using massager_laba.Data.DTO;
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

    public async Task<ProfileDTO> GetProfile(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var profile = new ProfileDTO
        {
            Login = user.Login,
            DateBirth = user.BirthDate,
            Avatar = user.AvatarUrl
        };

        return profile;
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

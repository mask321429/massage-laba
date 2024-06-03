using Amazon.S3;
using Amazon.S3.Transfer;
using massager_laba.Data;
using massager_laba.Data.DTO;
using massager_laba.Data.Model;
using massager_laba.Interface;

namespace massager_laba.Services;

public class AuthService : IAuthService
{
    private readonly DBContext _dbContext;
    
    public AuthService(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RegisterUser(RegistrationDTO model)
    {
     
        var avatarUrl = SaveAvatarToLocalDisk(model.Avatar, model.Login);
        
        var user = new UserModel
        {
            Id = Guid.NewGuid(),
            Login = model.Login,
            Password = model.Password, 
            BirthDate = DateTime.SpecifyKind(model.BirthDate, DateTimeKind.Utc), 
            AvatarUrl = avatarUrl
        };

        _dbContext.Users.Add(user);
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
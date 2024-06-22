using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Amazon.S3;
using Amazon.S3.Transfer;
using massager_laba.Data;
using massager_laba.Data.DTO;
using massager_laba.Data.Model;
using massager_laba.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace massager_laba.Services;

public class AuthService : IAuthService
{
    private readonly DBContext _dbContext;
    private readonly string _baseUrl;
    public AuthService(DBContext dbContext, IConfiguration baseUrl)
    {
        _dbContext = dbContext;
        _baseUrl = baseUrl["BaseUrl"];
    }

    public async Task<TokenDTO> RegisterUser(RegistrationDTO model)
    {
        var avatarUrl = string.Empty;
        
        await CheckLoginIdentity(model.Login);

        if (model.Avatar != null)
        {
            avatarUrl = SaveAvatarToLocalDisk(model.Avatar, model.Login);
        }
        
        byte[] saltBytes;
        RandomNumberGenerator.Fill(saltBytes = new byte[16]);

        using var deriveBytes = new Rfc2898DeriveBytes(model.Password, saltBytes, 100000);

        byte[] passwordHashBytes = deriveBytes.GetBytes(20);

        byte[] combinedBytes = new byte[36];
        Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, 16);
        Buffer.BlockCopy(passwordHashBytes, 0, combinedBytes, 16, 20);

        string savedPasswordHash = Convert.ToBase64String(combinedBytes);
        
        var user = new UserModel
        {
            Id = Guid.NewGuid(),
            Login = model.Login,
            Password = savedPasswordHash, 
            BirthDate = DateTime.SpecifyKind(model.BirthDate, DateTimeKind.Utc), 
            AvatarUrl = avatarUrl
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        
        var ForSuccessfulLogin = new LoginDTO
        {
            Login = model.Login,
            Password = model.Password
        };

        return await Login(ForSuccessfulLogin);
    }
    
    public async Task<TokenDTO> Login(LoginDTO ForSuccessfulLogin)
    {
        var identity = await GetIdentity(ForSuccessfulLogin.Login, ForSuccessfulLogin.Password);
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: TokenConfigurations.Issuer,
            audience: TokenConfigurations.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(TokenConfigurations.Lifetime),
            signingCredentials: new SigningCredentials(TokenConfigurations.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new TokenDTO()
        {
            Token = encodeJwt
        }; 
    }
    
    
    private async Task<ClaimsIdentity> GetIdentity(string login, string password)
    {
        var userEntity = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Login == login);

        if (userEntity == null)
        {
            throw new BadHttpRequestException("Login не найден");
        }

        if (!ValidatePasswordHash(userEntity.Password, password))
        {
            throw new BadHttpRequestException("Пароль не верный");
        }

        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, userEntity.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity
        (
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType
        );

        return claimsIdentity;
    }

    
    private static bool ValidatePasswordHash(string savedPasswordHash, string userEnteredPassword)
    {
        var hashBytes = Convert.FromBase64String(savedPasswordHash);
        var storedSalt = new byte[16];
        Array.Copy(hashBytes, 0, storedSalt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(userEnteredPassword, storedSalt, 100000);
        var computedHash = pbkdf2.GetBytes(20);

        for (var i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != computedHash[i])
            {
                return false;
            }
        }

        return true;
    }
    private string SaveAvatarToLocalDisk(IFormFile avatar, string login)
    {
        var filePath = Path.Combine($"{_baseUrl}", $"{login}_{Path.GetRandomFileName()}.jpg");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            avatar.CopyTo(stream);
        }

        return filePath;
    }
    
    private async Task CheckLoginIdentity(string email)
    {
        var existingEmail = await _dbContext
            .Users
            .AnyAsync(x => x.Login == email);

        if (existingEmail)
        {
            throw new BadHttpRequestException("Данный login уже занят");
        }
    }

}
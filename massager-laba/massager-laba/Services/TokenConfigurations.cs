using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace massager_laba.Services;

internal class TokenConfigurations
{
    
    public const string Issuer = "Messager"; 
    public const string Audience = "Messagers"; 
    private const string Key = "mYGh8lG8d6W7wC1cK2fR3sT4aP5eN9dE0dK8iS6eY3";
    public const int Lifetime = 360; 
  
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }

}

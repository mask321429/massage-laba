using massager_laba.Data.DTO;

namespace massager_laba.Interface;

public interface IAuthService
{
    Task<TokenDTO> RegisterUser(RegistrationDTO registrationDto);
    Task<TokenDTO> Login(LoginDTO loginDto);
}
using massager_laba.Data.DTO;

namespace massager_laba.Interface;

public interface IAuthService
{
    Task RegisterUser(RegistrationDTO registrationDto);

}
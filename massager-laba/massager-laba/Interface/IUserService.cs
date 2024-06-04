using massager_laba.Data.DTO;

namespace massager_laba.Interface;

public interface IUserService
{
    Task<ProfileDTO>  GetProfile(Guid id);
    Task UpdateProfile(Guid id, UpdateProfileDTO updateProfileDto);

}
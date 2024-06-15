using massager_laba.Data.DTO;

namespace massager_laba.Interface;

public interface IUserService
{
    Task<List<ProfileDTO>>  GetProfile(Guid? id, string? name);
    Task UpdateProfile(Guid id, UpdateProfileDTO updateProfileDto);

}
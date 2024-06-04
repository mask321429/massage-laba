namespace massager_laba.Data.DTO;

public class UpdateProfileDTO
{
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public IFormFile Avatar { get; set; }

}
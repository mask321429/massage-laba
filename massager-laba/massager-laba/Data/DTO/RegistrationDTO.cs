namespace massager_laba.Data.DTO;

public class RegistrationDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime DataBirth { get; set; }
    public IFormFile Photo { get; set; }
}
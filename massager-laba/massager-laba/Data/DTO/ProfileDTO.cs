namespace massager_laba.Data.DTO;

public class ProfileDTO
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public DateTime DateBirth { get; set; }
    public string Avatar { get; set; }
}
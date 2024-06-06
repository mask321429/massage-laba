namespace massager_laba.Data.DTO;

public class MessagerDTO
{
    public string NameUser { get; set; }
    public Guid IdUserWhere { get; set; }
    public DateTime DateTimeLastLetter { get; set; }
    public string UrlAvatar { get; set; }
    public bool IsCheked { get; set; }
}
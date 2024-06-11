namespace massager_laba.Data.DTO;

public class SendMessageRequestDTO
{
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public string Content { get; set; }
}
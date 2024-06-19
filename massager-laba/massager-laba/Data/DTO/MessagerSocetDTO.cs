using massager_laba.Data.Enum;

namespace massager_laba.Data.DTO;

public class MessagerSocetDTO
{
    public string FromUserId { get; set; }
    public string ToUserId { get; set; }
    public string Content { get; set; }
    public TypeMessage TypeMessage { get; set; }
}
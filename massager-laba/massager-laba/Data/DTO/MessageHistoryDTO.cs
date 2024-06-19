using massager_laba.Data.Enum;

namespace massager_laba.Data.DTO;

public class MessageHistoryDTO
{
    public string Name { get; set; }
    public Guid MyId { get; set; }
    public Guid IdWhere { get; set; }
    public List<Message> Messages { get; set; }
}

public class Message
{
    public Guid WhoseMessage { get; set; }
    public DateTime DateTimeMessage { get; set; }
    public string Text { get; set; }
    public TypeMessage TypeMessage { get; set; }
}
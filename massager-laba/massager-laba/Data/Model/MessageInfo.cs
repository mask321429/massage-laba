using System.ComponentModel.DataAnnotations;
using massager_laba.Data.Enum;

namespace massager_laba.Data.Model;

public class MessageInfo
{
    [Key]
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public TypeMessage TypeMessage { get; set; }
}
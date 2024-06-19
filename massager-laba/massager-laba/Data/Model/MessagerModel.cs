using System.ComponentModel.DataAnnotations;
using massager_laba.Data.Enum;

namespace massager_laba.Data.Model;

public class MessagerModel
{
    [Key]
    public Guid Id { get; set; }
    public Guid IdUserWhere { get; set; }
    public DateTime LastLetter { get; set; }
    public Guid IdUserFrom { get; set; }
    public bool IsCheked { get; set; }
    public TypeMessage TypeMessage { get; set; }
}
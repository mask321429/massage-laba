using System.ComponentModel.DataAnnotations;

namespace massager_laba.Data.Model;

public class MessagerModel
{
    [Key]
    public Guid Id { get; set; }
    public Guid IdUserWhere { get; set; }
    public DateTime LastLetter { get; set; }
    public Guid IdUserFrom { get; set; }
    public bool IsCheked { get; set; }
}
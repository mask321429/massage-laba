using System.ComponentModel.DataAnnotations;

namespace massager_laba.Data.Model;

public class UserModel
{
    [Key]
    public Guid Id { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public DateTime BirthDate { get; set; }

    public string AvatarUrl { get; set; }
}
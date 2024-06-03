using System.ComponentModel.DataAnnotations;

namespace massager_laba.Data.DTO;

public class RegistrationDTO
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Login { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
    
    public DateTime BirthDate { get; set; }

    [Required]
    public IFormFile Avatar { get; set; }
}
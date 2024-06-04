using massager_laba.Data.DTO;
using massager_laba.Interface;
using Microsoft.AspNetCore.Mvc;

namespace massager_laba.Controllers.User;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("/profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        try
        {
            var result  =  await _userService.GetProfile(Guid.Parse(User.Identity.Name));
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileDTO updateProfileDto)
    {
        try
        {
            if (User.Identity.IsAuthenticated && Guid.TryParse(User.Identity.Name, out Guid userId))
            {
                await _userService.UpdateProfile(userId, updateProfileDto);
                return Ok();
            }
            return Unauthorized();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }
}
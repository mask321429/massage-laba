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
    public async Task<IActionResult> GetUserProfile(string? name)
    {
        if (!Guid.TryParse(User?.Identity?.Name, out Guid guidParse) && name == null)
        {
            return BadRequest("Invalid user ID or name not provided.");
        }

        try
        {
            var result = guidParse != Guid.Empty ? 
                await _userService.GetProfile(guidParse, name) : 
                await _userService.GetProfile(null, name);

            return Ok(result);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateProfileDTO updateProfileDto)
    {
        try
        {
            await _userService.UpdateProfile(Guid.Parse(User.Identity.Name), updateProfileDto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }
}
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
            var result = await _userService.GetProfile(Guid.Parse(User.Identity.Name));
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
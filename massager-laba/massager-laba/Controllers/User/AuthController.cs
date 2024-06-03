using massager_laba.Interface;
using Microsoft.AspNetCore.Mvc;

namespace massager_laba.Controllers.User;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IActionResult> Registration()
    {
        try
        {
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}
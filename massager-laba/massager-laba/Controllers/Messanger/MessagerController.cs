using massager_laba.Interface;
using Microsoft.AspNetCore.Mvc;

namespace massager_laba.Controllers.Messanger;

public class MessagerController : ControllerBase
{
    private readonly IMeassagerService _meassagerService;

    public MessagerController(IMeassagerService meassagerService)
    {
        _meassagerService = meassagerService;
    }

    public async Task<IActionResult> GetMessage()
    {
        try
        {
            var result = await _meassagerService.GetMyMessager(Guid.Parse(User.Identity.Name));
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return StatusCode(404, new { Error = e.Message });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
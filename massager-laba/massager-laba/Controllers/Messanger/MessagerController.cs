using massager_laba.Data.DTO;
using massager_laba.Interface;
using Microsoft.AspNetCore.Mvc;

namespace massager_laba.Controllers.Messanger;
[Route("api/[controller]")]
[ApiController]
public class MessagerController : ControllerBase
{
    private readonly IMeassagerService _messageService;

    public MessagerController(IMeassagerService meassagerService)
    {
        _messageService = meassagerService;
    }
    [HttpGet("people")]
    public async Task<IActionResult> GetMessage()
    {
        try
        {
            var result = await _messageService.GetMyMessager(Guid.Parse(User.Identity.Name));
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
    
    [HttpGet("history")]
    public async Task<IActionResult> GetMessageHistory(Guid idToUser, int? count)
    {
        try
        {
            var result = await _messageService.GetHistoryMeassage(Guid.Parse(User.Identity.Name),idToUser, count);
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
    
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDTO request)
    {
        if (request == null || request.FromUserId == Guid.Empty || request.ToUserId == Guid.Empty || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Invalid request payload");
        }

        await _messageService.SendMessage(request.FromUserId = Guid.Parse(User.Identity.Name), request.ToUserId, request.Content, request.TypeMessage);

        return Ok("Message sent");
    }
    
    
    [HttpPost("send/photo/")]
    public async Task<IActionResult> SendPhoto(IFormFile photo, Guid toUserId)
    {
        

        await _messageService.SendPhoto(photo, toUserId, Guid.Parse(User.Identity.Name));

        return Ok("Message sent");
    }
}
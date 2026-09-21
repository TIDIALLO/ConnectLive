using Microsoft.AspNetCore.Mvc;

namespace ConnectLive.Core.Controllers;
[ApiController]
[Route("[controller]")]
public class SessionController : ControllerBase
{
    [HttpGet]
    [Route("get-sessions")]
    public async Task<IActionResult> GetSessions()
    {
        return Ok("GetSessions");
    }
}

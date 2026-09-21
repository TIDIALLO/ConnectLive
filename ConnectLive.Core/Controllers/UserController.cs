using ConnectLive.Core.Api.Queries;
using ConnectLive.Portal.Shared.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static ConnectLive.Core.Api.Commands.UserCommands;
using static ConnectLive.Core.Api.Queries.UserQueries;

namespace ConnectLive.Core.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;


    public UserController(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<UserController>>();
        _mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [HttpGet]
    [Route("get-profile/{id}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var result = await _mediator.Send(new UserQueries.GetUserQuery(id));
        if (result == null) return NotFound($"User with id '{id}' cannot be found!");

        return Ok(result);
    }

    [HttpGet]
    [Route("get-profiles")]
    public async Task<IActionResult> GetProfiles()
    {
        var result = await _mediator.Send(new UserQueries.GetUsersQuery());
        return Ok(result);
    }

    [HttpPost]
    [Route("save-profile")]
    public async Task<IActionResult> SaveUser(SaveUserRequest request)
    {
        var saveRequest = new SaveUserCommand(request);

        var result = await _mediator.Send(saveRequest);
        return Ok(result);
    }
}

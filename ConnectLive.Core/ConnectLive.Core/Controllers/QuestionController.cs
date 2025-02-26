using ConnectLive.Portal.Shared;
using ConnectLive.Portal.Shared.Request.Questions;
using ConnectLive.Portal.Shared.Response.Questions;
using Microsoft.AspNetCore.Mvc;

namespace ConnectLive.Core.Controllers;
[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetQuestions(Guid sessionId)
    {
        var mock = new List<CreateQuestionResponse>();
        for(int  i = 0; i < 100; i++)
        {
            mock.Add(new CreateQuestionResponse
            {
                Question = $"Question {i}",
                Choice1 = $"Choice 1",
                Choice2 = $"Choice 2",
                Choice3 = $"Choice 3",
                Choice4 = $"Choice 4"
            });
        }
        return Ok(await Result<List<CreateQuestionResponse>>.SuccessAsync(mock));
    }

    [HttpPost]
    [Route("save")]
    public async Task<IActionResult> SaveQuestion(CreateQuestionRequest request)
    {
        return Ok(await Result<bool>.SuccessAsync(true, "Question saved successfully!"));
    }
}

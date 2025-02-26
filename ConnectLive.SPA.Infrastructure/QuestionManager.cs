using ConnectLive.Portal.Shared;
using ConnectLive.Portal.Shared.Request.Questions;
using ConnectLive.Portal.Shared.Response.Questions;
using ConnectLive.SPA.Infrastructure.Extensions;
using System.Net.Http.Json;

namespace ConnectLive.SPA.Infrastructure;

public interface IQuestionManager : IManager
{
    Task<IResult<bool>> SaveQuestionAsync(CreateQuestionRequest request);
    Task<IResult<List<CreateQuestionResponse>>> GetAllQuestionsAsync();
}

public class QuestionManager(HttpClient httpClient) : IQuestionManager
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IResult<List<CreateQuestionResponse>>> GetAllQuestionsAsync()
    {
        var response = await _httpClient.GetAsync("question/all");
        return await response.ToResult<List<CreateQuestionResponse>>();
    }

    public async Task<IResult<bool>> SaveQuestionAsync(CreateQuestionRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("question/save", request);
        return await response.ToResult<bool>();
    }
}


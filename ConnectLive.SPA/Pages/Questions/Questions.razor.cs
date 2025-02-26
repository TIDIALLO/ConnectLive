using ConnectLive.Portal.Shared.Request.Questions;
using ConnectLive.Portal.Shared.Response.Questions;
using ConnectLive.SPA.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace ConnectLive.SPA.Pages.Questions;

public partial class Questions
{
    [Inject] private IQuestionManager QuestionManager { get; set; }
    private List<CreateQuestionResponse> _questions { get; set; } = new List<CreateQuestionResponse>();
    private bool _loading = false;
    protected override async Task OnInitializedAsync()
    {
        await GetAllQuestions();
    }

    private async Task GetAllQuestions()
    {
        try
        {
            _loading = true;
            await Task.Delay(4000);
            var result = await QuestionManager.GetAllQuestionsAsync();
            if (result.Successed)
            {
                _questions = result.Data;
            }
            else
            {
                result.Messages.ForEach(x => SnackBarAlert.Add(x, Severity.Error));
            }
        }
        catch (Exception ex)
        {
            SnackBarAlert.Add(ex.Message, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }
}

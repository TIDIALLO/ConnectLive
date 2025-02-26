using ConnectLive.Portal.Shared.Request.Questions;
using ConnectLive.SPA.Infrastructure;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ConnectLive.SPA.Pages.Questions;

public partial class CreateQuestion
{
    private CreateQuestionRequest _questionModel { get; set; } = new();
    [Inject] private IQuestionManager QuestionManager { get; set; }
    protected override void OnInitialized()
    {
        _questionModel.Question = "What is the process by which plants make their food using sunlight, carbon dioxide, and water?";
        _questionModel.Choice1 = "Respiration";
        _questionModel.Choice2 = "Photosynthesis";
        _questionModel.Choice3 = "Digestion";
        _questionModel.Choice4 = "Fermentation";
    }

    private void OnCancel()
    {
        _questionModel = new();
    }

    private async Task OnSave()
    {
        try
        {
            var result = await QuestionManager.SaveQuestionAsync(_questionModel);
            if (result.Successed)
            {
                SnackBarAlert.Add(result.Messages.FirstOrDefault(), Severity.Success);
                OnCancel();
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
    }
}

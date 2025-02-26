using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ConnectLive.SPA.Layout;

public partial class MainLayout
{
    #region Properties
    private readonly MudTheme _theme = new MudTheme()
    {
        Palette = new PaletteLight()
        {
            AppbarBackground = "#90CAF9"
        }
    };

    #endregion

    #region Methods
    private void GoHome() => NavigationManager.NavigateTo("/");
    private void GoToCreateUser() => NavigationManager.NavigateTo("create-user");
    private void GoToDeleteUser() => NavigationManager.NavigateTo("delete-user"); 
    private void GoToAllUsers() => NavigationManager.NavigateTo("users");

    private void GoToCreateGame() => NavigationManager.NavigateTo("create-game");
    private void GoToAllGames() => NavigationManager.NavigateTo("games");

    private void GoToCreateQuestion() => NavigationManager.NavigateTo("create-question");
    private void GoToAllQuestions() => NavigationManager.NavigateTo("questions");
    #endregion

}

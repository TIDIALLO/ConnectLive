using Microsoft.AspNetCore.Components;

namespace ConnectLive.SPA.Components;

public partial class HeaderComponent
{
    [Parameter] public string Title { get; set; }
    [Parameter] public string Description { get; set; }
}

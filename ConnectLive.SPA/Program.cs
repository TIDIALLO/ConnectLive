using ConnectLive.SPA;
using ConnectLive.SPA.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["GatewayUrl"]) });
builder.Services.AddMudServices();

builder.Services.AddTransient<IQuestionManager, QuestionManager>();


await builder.Build().RunAsync();

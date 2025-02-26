using ConnectLive.Portal.Shared;

namespace ConnectLive.SPA.Infrastructure.Extensions;
public static class ResultExtensions
{
    public static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Result<T>>(responseAsString);
    }

}

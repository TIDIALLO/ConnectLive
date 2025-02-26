using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connectlive.Proxy;
public class Proxy : IProxy
{
    private async Task<T> SendAsync<T>(HttpRequestMessage httpRequest, RequestCommand request)
    {
        T? result = default;

        //WARNING: Not good for PRODUCTION
        using var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
        };

        using var client = new HttpClient(httpClientHandler);
        var httpResponse = await client.SendAsync(httpRequest);

        var stringContent = await httpResponse.Content.ReadAsStringAsync()!;

        result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringContent);
        return result;
    }


    public async Task<T> Get<T>(RequestCommand request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, request.Uri);
        return await SendAsync<T>(httpRequest, request);
    }

    public Task<T> Delete<T>(RequestCommand request)
    {
        throw new NotImplementedException();
    }

    public Task<T> Post<T>(RequestCommand request)
    {
        throw new NotImplementedException();
    }

    public Task<T> Put<T>(RequestCommand request)
    {
        throw new NotImplementedException();
    }
}

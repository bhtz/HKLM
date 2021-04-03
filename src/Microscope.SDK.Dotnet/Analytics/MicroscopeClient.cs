using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microscope.SDK.Dotnet.Routes;
using Microscope.SDK.Dotnet.Models;
using System;

namespace Microscope.SDK.Dotnet
{
    public partial class MicroscopeClient
    {
        public async Task<string> PostAnalyticAsync(Analytic analytic)
        {
            var response = await this._httpClient.PostAsJsonAsync(AnalyticsEndpoint.Create,analytic);
            var clientId = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty;
            return clientId;
        }

        public async Task<bool> PutAnalyticAsync(Guid id, Analytic analytic)
        {
            var response = await this._httpClient.PutAsJsonAsync(AnalyticsEndpoint.Update(id), analytic);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Analytic>> GetAnalyticsAsync()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<Analytic>>(AnalyticsEndpoint.GetAll);
        }
    }
}
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microscope.SDK.Dotnet.Routes;
using System;

namespace Microscope.SDK.Dotnet
{
    public partial class MicroscopeClient
    {
        public async Task<string> PostRemoteConfigAsync(RemoteConfig remoteConfig)
        {
            var response = await this._httpClient.PostAsJsonAsync(RemoteConfigsEndpoint.Create, remoteConfig);

            var locationPathAndQuery = response.Headers.Location.PathAndQuery;
            var clientId = response.IsSuccessStatusCode ? locationPathAndQuery.Substring(locationPathAndQuery.LastIndexOf("/", StringComparison.Ordinal) + 1) : null;
            return clientId;
        }

        public async Task<bool> PutRemoteConfigAsync(Guid id, RemoteConfig remoteConfig)
        {
            var response = await this._httpClient.PutAsJsonAsync(RemoteConfigsEndpoint.Update(id), remoteConfig);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<RemoteConfig>> GetRemoteConfigsAsync()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<RemoteConfig>>(RemoteConfigsEndpoint.GetAll);
        }
    }

    public class RemoteConfig 
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Dimension { get; set; }
    }
}
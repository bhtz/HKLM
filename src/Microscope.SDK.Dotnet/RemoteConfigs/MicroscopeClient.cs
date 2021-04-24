using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microscope.SDK.Dotnet.Routes;
using System;
using Microscope.Application.Core.Commands.RemoteConfig;
using Microscope.Application.Core.Queries.RemoteConfig;

namespace Microscope.SDK.Dotnet
{
    public partial class MicroscopeClient
    {
        public async Task<string> PostRemoteConfigAsync(AddEditRemoteConfigCommand remoteConfig)
        {
            var response = await this._httpClient.PostAsJsonAsync(RemoteConfigsEndpoint.Create, remoteConfig);

            var locationPathAndQuery = response.Headers.Location.PathAndQuery;
            var clientId = response.IsSuccessStatusCode ? locationPathAndQuery.Substring(locationPathAndQuery.LastIndexOf("/", StringComparison.Ordinal) + 1) : null;
            return clientId;
        }

        public async Task<bool> PutRemoteConfigAsync(Guid id, AddEditRemoteConfigCommand remoteConfig)
        {
            var response = await this._httpClient.PutAsJsonAsync(RemoteConfigsEndpoint.Update(id), remoteConfig);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<RemoteConfigQueryResult>> GetRemoteConfigsAsync()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<RemoteConfigQueryResult>>(RemoteConfigsEndpoint.GetAll);
        }
    }
}
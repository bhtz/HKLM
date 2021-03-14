using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Microscope.Admin.Pages.Analytic
{
    public partial class Analytic : ComponentBase
    {
        #region injected properties

        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        #endregion

        #region properties
        public IEnumerable<Domain.Entities.Analytic> Analytics { get; set; } = new List<Domain.Entities.Analytic>();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await this.SetHttpHeaders();
            await this.GetAnalytic();
        }

        private async Task SetHttpHeaders()
        {
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            if (accessTokenResult.TryGetToken(out var token))
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }

        private async Task GetAnalytic()
        {
            var res = await this.Http.GetFromJsonAsync<IEnumerable<Microscope.Domain.Entities.Analytic>>("api/analytic");
            this.Analytics = res.ToList();
        }
    }
}
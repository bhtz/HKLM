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
using Microscope.Domain.Entities;
using MCSPAnalytic = Microscope.Domain.Entities.Analytic;

namespace Microscope.Admin.Pages.Analytic
{
    public partial class AnalyticPage : ComponentBase
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
        public IList<MCSPAnalytic> Analytics { get; set; } = new List<MCSPAnalytic>();
        public MCSPAnalytic SelectedItem { get; set; } = new MCSPAnalytic();
        public string SearchTerm { get; set; } = String.Empty;
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
            var res = await this.Http.GetFromJsonAsync<IEnumerable<MCSPAnalytic>>("api/analytic");
            this.Analytics = res.ToList();
        }

        private async Task Delete(MCSPAnalytic item)
        {
            var confirm = await this.ConfirmDialog("Are you sure ?");
            if (confirm)
            {
                var res = await this.Http.DeleteAsync("api/analytic/" + item.Id);
                if (res.IsSuccessStatusCode)
                {
                    this.Analytics.Remove(item);
                }
            }
        }

        private void OnSelectItem(MCSPAnalytic item)
        {
            this.SelectedItem = item;
        }

        private void OpenCreate()
        {
            this.SelectedItem = new MCSPAnalytic();
        }

        private async void HandleFormSubmit()
        {
            if(this.SelectedItem.Id == null)
            {
                await Http.PutAsJsonAsync("api/Analytic/" + this.SelectedItem.Id, this.SelectedItem);                
            }
            else
            {
                await Http.PostAsJsonAsync("api/Analytic", this.SelectedItem);
                this.Analytics.Add(this.SelectedItem);
            }

            await JsRuntime.InvokeVoidAsync("interop.toggleModal", "analyticModal");
            this.StateHasChanged();
        }

        private ValueTask<bool> ConfirmDialog(string message)
        {
            return this.JsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
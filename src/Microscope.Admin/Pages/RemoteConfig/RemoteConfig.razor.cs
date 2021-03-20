using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using MCSPRemoteConfig = Microscope.Domain.Entities.RemoteConfig;

namespace Microscope.Admin.Pages.RemoteConfig
{
    public partial class RemoteConfig : ComponentBase
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
        public IList<MCSPRemoteConfig> RemoteConfigs { get; set; } = new List<MCSPRemoteConfig>();
        public MCSPRemoteConfig SelectedItem { get; set; } = new MCSPRemoteConfig();
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
            var res = await this.Http.GetFromJsonAsync<IEnumerable<MCSPRemoteConfig>>("api/remoteconfig");
            this.RemoteConfigs = res.ToList();
        }

        private async Task Delete(MCSPRemoteConfig item)
        {
            var confirm = await this.ConfirmDialog("Are you sure ?");
            if (confirm)
            {
                var res = await this.Http.DeleteAsync("api/remoteconfig/" + item.Id);
                if (res.IsSuccessStatusCode)
                {
                    this.RemoteConfigs.Remove(item);
                }
            }
        }

        private void OnSelectItem(MCSPRemoteConfig item)
        {
            this.SelectedItem = item;
        }

        private void OpenCreate()
        {
            this.SelectedItem = new MCSPRemoteConfig();
        }

        private async void HandleFormSubmit()
        {
            if(this.SelectedItem.Id != Guid.Empty)
            {
                await Http.PutAsJsonAsync("api/remoteconfig/" + this.SelectedItem.Id, this.SelectedItem);                
            }
            else
            {
                await Http.PostAsJsonAsync("api/remoteconfig", this.SelectedItem);
                this.RemoteConfigs.Add(this.SelectedItem);
            }

            await JsRuntime.InvokeVoidAsync("interop.toggleModal", "remoteconfigModal");
            this.StateHasChanged();
        }

        private ValueTask<bool> ConfirmDialog(string message)
        {
            return this.JsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
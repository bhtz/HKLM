using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using MudBlazor;

namespace Microscope.Admin.Pages.RemoteConfig
{
    public partial class RemoteConfigFormDialog : ComponentBase
    {

        #region injected properties

        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        #endregion

        #region properties
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public RemoteConfigFormDTO RemoteConfig { get; set; } = new RemoteConfigFormDTO();

        public bool Success { get; set; }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await this.SetHttpHeaders();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await this.JSONEditor();
            }
        }

        private async Task JSONEditor()
        {
            await this.JsRuntime.InvokeVoidAsync("interop.jsonEditor", "jsoneditor", "dimension");
        }

        private async Task SetHttpHeaders()
        {
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            if (accessTokenResult.TryGetToken(out var token))
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }

        public async Task OnValidSubmit()
        {
            Success = true;
            StateHasChanged();

            if (this.RemoteConfig.Id != Guid.Empty)
            {
                var response = await Http.PutAsJsonAsync("api/remoteconfig/" + this.RemoteConfig.Id, this.RemoteConfig);
                if (response.IsSuccessStatusCode)
                {
                    Snackbar.Add("Remote Config updated", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(this.RemoteConfig));
                }
                else
                {
                    Snackbar.Add("Error", Severity.Error);
                    MudDialog.Close(DialogResult.Cancel());
                }
            }
            else
            {
                var response = await Http.PostAsJsonAsync("api/remoteconfig", this.RemoteConfig);

                if (response.IsSuccessStatusCode)
                {
                    RemoteConfigFormDTO inserted = await response.Content.ReadFromJsonAsync<RemoteConfigFormDTO>();
                    this.RemoteConfig.Id = inserted.Id;
                    Snackbar.Add("Remote Config added", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(this.RemoteConfig));
                }
                else
                {
                    Snackbar.Add("Error", Severity.Error);
                    MudDialog.Close(DialogResult.Cancel());
                }

            }
        }

        void Cancel() => MudDialog.Cancel();

        public class RemoteConfigFormDTO
        {
            public Guid Id { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "Key must be at least 2 characters long.", MinimumLength = 2)]
            public string Key { get; set; }

            [Required]
            public string Dimension { get; set; }

        }
    }
}
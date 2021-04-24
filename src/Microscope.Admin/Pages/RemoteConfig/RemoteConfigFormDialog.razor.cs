using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Microscope.Admin.Pages.RemoteConfig
{
    public partial class RemoteConfigFormDialog : ComponentBase
    {

        #region injected properties

        [Inject]
        private IJSRuntime JsRuntime { get; set; }


        #endregion

        #region properties
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public RemoteConfigFormViewModel RemoteConfig { get; set; } = new RemoteConfigFormViewModel();

        public bool Success { get; set; }

        #endregion

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

        public async Task OnValidSubmit()
        {
            Success = true;
            StateHasChanged();

            if (this.RemoteConfig.Id != Guid.Empty)
            {
                var response = await _httpClient.PutAsJsonAsync("api/remoteconfig/" + this.RemoteConfig.Id, this.RemoteConfig);
                if (response.IsSuccessStatusCode)
                {
                    _snackBar.Add("Remote Config updated", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(this.RemoteConfig));
                }
                else
                {
                    _snackBar.Add("Error", Severity.Error);
                    MudDialog.Close(DialogResult.Cancel());
                }
            }
            else
            {
                var response = await _httpClient.PostAsJsonAsync("api/remoteconfig", this.RemoteConfig);

                if (response.IsSuccessStatusCode)
                {
                    RemoteConfigFormViewModel inserted = await response.Content.ReadFromJsonAsync<RemoteConfigFormViewModel>();
                    this.RemoteConfig.Id = inserted.Id;
                    _snackBar.Add("Remote Config added", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(this.RemoteConfig));
                }
                else
                {
                    _snackBar.Add("Error", Severity.Error);
                    MudDialog.Close(DialogResult.Cancel());
                }

            }
        }

        void Cancel() => MudDialog.Cancel();

        public class RemoteConfigFormViewModel
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
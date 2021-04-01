using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Microscope.Admin.Pages.Analytic
{
    public partial class AnalyticFormDialog : ComponentBase
    {

        #region injected properties

        [Inject]
        private IJSRuntime JsRuntime { get; set; }


        #endregion

        #region properties
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public AnalyticFormDTO Analytic { get; set; } = new AnalyticFormDTO();

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

            if (this.Analytic.Id != Guid.Empty)
            {
                var response = await _httpClient.PutAsJsonAsync("api/Analytic/" + this.Analytic.Id, this.Analytic);
                if (response.IsSuccessStatusCode)
                {
                    _snackBar.Add("Analytic updated", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(this.Analytic));
                }
                else
                {
                    _snackBar.Add("Error", Severity.Error);
                    MudDialog.Close(DialogResult.Cancel());
                }
            }
            else
            {
                var response = await _httpClient.PostAsJsonAsync("api/Analytic", this.Analytic);

                if (response.IsSuccessStatusCode)
                {
                    AnalyticFormDTO inserted = await response.Content.ReadFromJsonAsync<AnalyticFormDTO>();
                    this.Analytic.Id = inserted.Id;
                    _snackBar.Add("Analytic added", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(this.Analytic));
                }
                else
                {
                    _snackBar.Add("Error", Severity.Error);
                    MudDialog.Close(DialogResult.Cancel());
                }

            }

        }

        void Cancel() => MudDialog.Cancel();

        public class AnalyticFormDTO
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
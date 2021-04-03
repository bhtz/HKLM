using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SdkAnalytic = Microscope.SDK.Dotnet.Models.Analytic;

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

            var analytic = new SdkAnalytic
            {
                Id = this.Analytic.Id,
                Key = this.Analytic.Key,
                Dimension = this.Analytic.Dimension
            };

            if (analytic.Id != Guid.Empty)
            {
                bool success = await _microscopeClient.PutAnalyticAsync(analytic.Id, analytic);
                if (success)
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
                string id = await _microscopeClient.PostAnalyticAsync(analytic);

                if (!string.IsNullOrEmpty(id))
                {
                    this.Analytic.Id = Guid.Parse(id);
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
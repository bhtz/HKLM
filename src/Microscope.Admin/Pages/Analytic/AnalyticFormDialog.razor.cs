using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;

namespace Microscope.Admin.Pages.Analytic
{
    public partial class AnalyticFormDialog : ComponentBase
    {

        #region injected properties

        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        private HttpClient Http { get; set; }

        #endregion

        #region properties
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public AnalyticFormDTO Analytic { get; set; } = new AnalyticFormDTO();

        public bool Success { get; set; }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await this.SetHttpHeaders();
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

             if (this.Analytic.Id != Guid.Empty)
            {
                await Http.PutAsJsonAsync("api/Analytic/" + this.Analytic.Id, this.Analytic);
            }
            else
            {
             var response =  await Http.PostAsJsonAsync("api/Analytic", this.Analytic);
             AnalyticFormDTO inserted = await response.Content.ReadFromJsonAsync<AnalyticFormDTO>();
             this.Analytic.Id = inserted.Id;
            }

            MudDialog.Close(DialogResult.Ok(this.Analytic));

        }

        void Cancel() => MudDialog.Cancel();

        public class AnalyticFormDTO
        {
            public Guid Id { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "Key must be at least 2 characters long.", MinimumLength = 2)]
            public string Key { get; set; }

            [Required]
            public string Dimension { get; set; } = "{}";

        }
    }
}
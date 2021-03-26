using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;

namespace Microscope.Admin.Pages.Storage
{
    public partial class ContainerFormDialog : ComponentBase
    {

        #region injected properties

        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        #endregion

        #region properties
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public StorageContainerDTO StorageContainer { get; set; } = new StorageContainerDTO();

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

            var response = await Http.PostAsJsonAsync<string>("api/storage", this.StorageContainer.Name);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Container created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(this.StorageContainer));
            }
            else
            {
                Snackbar.Add(response.ReasonPhrase, Severity.Error);
                MudDialog.Close(DialogResult.Cancel());
            }
        }

        void Cancel() => MudDialog.Cancel();

        public class StorageContainerDTO
        {
            private string _name;

            [Required]
            [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
            public string Name
            {
                get { return _name; }
                set { _name = value.ToLower(); }
            }

        }
    }
}
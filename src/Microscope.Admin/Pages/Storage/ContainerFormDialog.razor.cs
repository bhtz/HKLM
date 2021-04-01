using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Microscope.Admin.Pages.Storage
{
    public partial class ContainerFormDialog 
    {

        #region injected properties

        #endregion

        #region properties
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public StorageContainerDTO StorageContainer { get; set; } = new StorageContainerDTO();

        public bool Success { get; set; }

        #endregion

        public async Task OnValidSubmit()
        {
            Success = true;
            StateHasChanged();

            var response = await _httpClient.PostAsJsonAsync<string>("api/storage", this.StorageContainer.Name);
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add(localizer["Container Saved"], Severity.Success);
                MudDialog.Close(DialogResult.Ok(this.StorageContainer));
            }
            else
            {
                _snackBar.Add(response.ReasonPhrase, Severity.Error);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microscope.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using MudBlazor;
using static Microscope.Admin.Pages.Storage.ContainerFormDialog;

namespace Microscope.Admin.Pages.Storage
{
    public partial class Storage : ComponentBase
    {
        #region injected properties

        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }

        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }


        #endregion

        #region private properties

        public string SearchTerm { get; set; } = String.Empty;
        private string _selectedContainer;
        #endregion

        #region public properties

        public List<string> Blobs { get; set; } = new List<string> { };
        public List<string> Containers { get; set; } = new List<string> { };
        public bool IsLoading { get; set; } = false;
        public string SelectedContainer
        {
            get => _selectedContainer;
            set
            {
                _selectedContainer = value;
                this.GetBlobsFromSelectedContainer();
            }
        }

        #endregion

        #region methods

        protected override async Task OnInitializedAsync()
        {
            await this.SetHttpHeaders();
            await this.GetContainers();
        }

        private async Task GetContainers()
        {
            var containerResults = await Http.GetFromJsonAsync<IEnumerable<string>>("api/storage");
            this.Containers = containerResults.ToList();
            this.SelectedContainer = this.Containers.FirstOrDefault();
        }

        private bool FilterFunc(string element)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;
            if (element.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task OpenCreateContainerDialog()
        {

            var dialog = DialogService.Show<ContainerFormDialog>("New Container", new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                DisableBackdropClick = true
            });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var newItem = (StorageContainerDTO)result.Data;
                this.Containers.Add(newItem.Name);
                this.SelectedContainer = newItem.Name;
                this.StateHasChanged();
            }
        }

        private async void GetBlobsFromSelectedContainer()
        {

            if (!string.IsNullOrEmpty(this.SelectedContainer))
            {
                this.SearchTerm = string.Empty;
                var blobResults = await Http.GetFromJsonAsync<IEnumerable<string>>("api/storage/" + this.SelectedContainer);
                this.Blobs = blobResults.ToList();
                this.StateHasChanged();
            }
        }

        private async void Download(string blobName)
        {
            var res = await Http.GetAsync("api/storage/" + this.SelectedContainer + "/" + blobName);

            if (res.IsSuccessStatusCode)
            {
                var bytes = await res.Content.ReadAsByteArrayAsync();
                Console.WriteLine(bytes.Length);

                await this.JsRuntime.InvokeVoidAsync("interop.downloadFromByteArray",
                    new
                    {
                        ByteArray = bytes,
                        FileName = blobName,
                        ContentType = "application/octet-stream"
                    });
            }
        }

        private async Task DeleteBlob(string blobName)
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Are you sure ?");
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

            var dialog = DialogService.Show<ConfirmDialog>("Delete Blob", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var res = await Http.DeleteAsync($"api/storage/{this.SelectedContainer}/{blobName}");
                if (res.IsSuccessStatusCode)
                {
                    this.Blobs.Remove(blobName);
                    Snackbar.Add("Blob deleted", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Error deleted blob", Severity.Error);
                }
            }
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            IBrowserFile file = e.File;

            this.IsLoading = true;

            string imageType = file.ContentType;
            var fileContent = new StreamContent(file.OpenReadStream(50 * 1000 * 1024)); // LIMIT 50 MO
            var content = new MultipartFormDataContent();

            content.Add(content: fileContent, name: "file", fileName: file.Name);

            try
            {
                var res = await Http.PostAsync("api/storage/" + this.SelectedContainer, content);
                if (res.IsSuccessStatusCode)
                {
                    Snackbar.Add("File uploaded", Severity.Success);
                    this.GetBlobsFromSelectedContainer();
                }
                else
                {
                    Snackbar.Add("res.ReasonPhrase", Severity.Error);
                }
            }
            catch (System.Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);

            }

            this.IsLoading = false;
        }

        private async Task SetHttpHeaders()
        {
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            if (accessTokenResult.TryGetToken(out var token))
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }

        #endregion
    }
}
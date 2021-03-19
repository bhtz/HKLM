using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microscope.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

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
        private IJSRuntime JsRuntime { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        #endregion

        #region private properties
        private string _selectedContainer;
        #endregion

        #region public properties

        public List<string> Blobs { get; set; } = new List<string> { };
        public List<string> Containers { get; set; } = new List<string> { };
        public StorageContainer StorageContainer { get; set; } = new StorageContainer();
        public string ImageName { get; set; }
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

        private async void HandleContainerSubmit()
        {
            var res = await Http.PostAsJsonAsync<string>("api/storage", this.StorageContainer.Name);
            this.Containers.Add(this.StorageContainer.Name);
            this.StorageContainer.Name = string.Empty;
            base.StateHasChanged();
            this.ToastService.ShowSuccess("Container created !");
            this.CloseModal();
        }

        private async void GetBlobsFromSelectedContainer()
        {
            if (!string.IsNullOrEmpty(this.SelectedContainer))
            {
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
            var isConfirmed = await this.ConfirmDialog("Are you sure ?");
            if (isConfirmed)
            {
                var res = await Http.DeleteAsync($"api/storage/{this.SelectedContainer}/{blobName}");
                if (res.IsSuccessStatusCode)
                {
                    this.Blobs.Remove(blobName);
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
                    this.ToastService.ShowSuccess("File uploaded");
                    this.GetBlobsFromSelectedContainer();
                }
                else
                {
                    this.ToastService.ShowWarning(res.ReasonPhrase);
                }
            }
            catch (System.Exception ex)
            {
                this.ToastService.ShowError(ex.Message);
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

        private async void CloseModal()
        {
            await JsRuntime.InvokeVoidAsync("interop.toggleModal", "containerModal");
        }

        private ValueTask<bool> ConfirmDialog(string message)
        {
            return this.JsRuntime.InvokeAsync<bool>("confirm", message);
        }

        #endregion
    }
}
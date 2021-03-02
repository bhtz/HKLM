using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microscope.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace Microscope.Admin.Pages.Storage
{
    public partial class Storage : ComponentBase
    {
        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        IJSRuntime JsRuntime { get; set; }
        public List<string> Blobs { get; set; } = new List<string>{};
        public List<string> Containers { get; set; } = new List<string>{};
        public string SelectedContainer { get; set; }
        public StorageContainer StorageContainer { get; set; } = new StorageContainer();

        protected override async Task OnInitializedAsync()
        {
            await this.SetHttpHeaders();
            await this.GetContainers();
        }

        private async Task GetContainers()
        {
            var containerResults = await Http.GetFromJsonAsync<IEnumerable<string>>("/storage");
            this.Containers = containerResults.ToList();
        }

        private async Task GetBlobsFromContainer(ChangeEventArgs e)
        {
            this.SelectedContainer = e.Value.ToString();
            var blobResults = await Http.GetFromJsonAsync<IEnumerable<string>>("/storage/" + SelectedContainer);
            this.Blobs = blobResults.ToList();
        }

        private async void HandleContainerSubmit()
        {
            var res = await Http.PostAsJsonAsync<string>("/storage", this.StorageContainer.Name);
            this.Containers.Add(this.StorageContainer.Name);
            this.StorageContainer.Name = "";
            base.StateHasChanged();
            this.CloseModal();
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
    }
}
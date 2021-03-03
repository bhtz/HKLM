using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microscope.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
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

        #region View properties
        
        public List<string> Blobs { get; set; } = new List<string>{};
        public List<string> Containers { get; set; } = new List<string>{};
        public StorageContainer StorageContainer { get; set; } = new StorageContainer();
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
        
        #region private properties
        private string _selectedContainer;
        #endregion

        #region methods

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

        private async void HandleContainerSubmit()
        {
            var res = await Http.PostAsJsonAsync<string>("/storage", this.StorageContainer.Name);
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
                var blobResults = await Http.GetFromJsonAsync<IEnumerable<string>>("/storage/" + this.SelectedContainer);
                this.Blobs = blobResults.ToList();
                this.StateHasChanged();
            }
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
        
        #endregion
    }
}
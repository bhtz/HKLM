using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microscope.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using MudBlazor;
using static Microscope.Admin.Pages.RemoteConfig.RemoteConfigFormDialog;
using MCSPRemoteConfig = Microscope.Domain.Entities.RemoteConfig;

namespace Microscope.Admin.Pages.RemoteConfig
{
    public partial class RemoteConfig : ComponentBase
    {
        #region injected properties

        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }

        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        // [Inject]
        // private IToastService ToastService { get; set; }
        #endregion

        #region properties
        public IList<MCSPRemoteConfig> RemoteConfigs { get; set; } = new List<MCSPRemoteConfig>();
        public string SearchTerm { get; set; } = String.Empty;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await this.SetHttpHeaders();
            await this.GetRemoteConfigs();
        }

        private async Task SetHttpHeaders()
        {
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            if (accessTokenResult.TryGetToken(out var token))
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }

        private async Task GetRemoteConfigs()
        {
            var res = await this.Http.GetFromJsonAsync<IEnumerable<MCSPRemoteConfig>>("api/remoteconfig");
            this.RemoteConfigs = res.ToList();
        }

        private bool FilterFunc(MCSPRemoteConfig element)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;
            if (element.Key.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task OpenCreateDialog()
        {
            await Task.FromResult(0);
            // await this.JSONEditor();

            var dialog = DialogService.Show<RemoteConfigFormDialog>("New Remote Config", new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                DisableBackdropClick = true
            });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var newItem = (RemoteConfigFormDTO)result.Data;
                //In a real world scenario we would reload the data from the source
                MCSPRemoteConfig newRemoteConfig = new MCSPRemoteConfig
                {
                    Id = newItem.Id,
                    Key = newItem.Key,
                    Dimension = newItem.Dimension
                };

                this.RemoteConfigs.Add(newRemoteConfig);

            }
        }

        private async Task OnSelectItem(MCSPRemoteConfig item)
        {

            RemoteConfigFormDTO dto = new RemoteConfigFormDTO
            {
                Id = item.Id,
                Key = item.Key,
                Dimension = item.Dimension
            };

            var parameters = new DialogParameters { ["RemoteConfig"] = dto };

            //await this.JSONEditor();

            var dialog = DialogService.Show<RemoteConfigFormDialog>("Edit Remote Config", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                DisableBackdropClick = true
            });
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                //In a real world scenario we would reload the data 

                var editedItem = (RemoteConfigFormDTO)result.Data;

                var remoteConfigToUpdate = this.RemoteConfigs.FirstOrDefault(a => a.Id == editedItem.Id);
                if (remoteConfigToUpdate != null)
                {
                    remoteConfigToUpdate.Key = editedItem.Key;
                    remoteConfigToUpdate.Dimension = editedItem.Dimension;
                }

            }
        }
        private async Task Delete(MCSPRemoteConfig item)
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Are you sure ?");
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = DialogService.Show<ConfirmDialog>("Delete", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var res = await this.Http.DeleteAsync("api/remoteconfig/" + item.Id);
                if (res.IsSuccessStatusCode)
                {
                    this.RemoteConfigs.Remove(item);
                }
            }

        }

    }
}
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System;
using MCSPAnalytic = Microscope.Domain.Entities.Analytic;
using MudBlazor;
using static Microscope.Admin.Pages.Analytic.AnalyticFormDialog;
using Microscope.Admin.Shared;
using Microscope.Admin.Shared.Dialogs;

namespace Microscope.Admin.Pages.Analytic
{
    public partial class Analytic : ComponentBase
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

        #endregion

        #region properties
        public IList<MCSPAnalytic> Analytics { get; set; } = new List<MCSPAnalytic>();

        public string SearchTerm { get; set; } = String.Empty;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await this.SetHttpHeaders();
            await this.GetAnalytic();
        }

        private async Task SetHttpHeaders()
        {
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            if (accessTokenResult.TryGetToken(out var token))
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }

        private async Task GetAnalytic()
        {
            var res = await this.Http.GetFromJsonAsync<IEnumerable<MCSPAnalytic>>("api/analytic");
            this.Analytics = res.ToList();
        }

        private bool FilterFunc(MCSPAnalytic element)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;
            if (element.Key.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task OpenCreateDialog()
        {

            var dialog = DialogService.Show<AnalyticFormDialog>("Modal", new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                DisableBackdropClick = true
            });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var newItem = (AnalyticFormDTO)result.Data;
                //In a real world scenario we would reload the data from the source
                MCSPAnalytic newAnalytic = new MCSPAnalytic
                {
                    Id = newItem.Id,
                    Key = newItem.Key,
                    Dimension = newItem.Dimension
                };

                this.Analytics.Add(newAnalytic);
            }
        }

        private async Task OnSelectItem(MCSPAnalytic item)
        {

            AnalyticFormDTO dto = new AnalyticFormDTO
            {
                Id = item.Id,
                Key = item.Key,
                Dimension = item.Dimension
            };

            var parameters = new DialogParameters { ["Analytic"] = dto };

            var dialog = DialogService.Show<AnalyticFormDialog>("Modal", parameters, new DialogOptions
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

                var editedItem = (AnalyticFormDTO)result.Data;

                var analyticToUpdate = this.Analytics.FirstOrDefault(a => a.Id == editedItem.Id);
                if (analyticToUpdate != null)
                {
                    analyticToUpdate.Key = editedItem.Key;
                    analyticToUpdate.Dimension = editedItem.Dimension;
                }

            }
        }

        private async Task Delete(MCSPAnalytic item)
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Are you sure ?");
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

            var dialog = DialogService.Show<ConfirmDialog>("Delete", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var res = await this.Http.DeleteAsync("api/analytic/" + item.Id);

                if (res.IsSuccessStatusCode)
                {
                    this.Analytics.Remove(item);
                    Snackbar.Add("Remote Config deleted", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Error", Severity.Error);
                }
            }
        }

    }
}
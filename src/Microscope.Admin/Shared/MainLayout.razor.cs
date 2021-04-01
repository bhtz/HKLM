using System.Collections.Generic;
using System.Threading.Tasks;
using Microscope.Admin.Infrastructure.Settings;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Microscope.Admin.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        MudTheme currentTheme;

        public bool _drawerOpen = true;

        protected override async Task OnInitializedAsync()
        {
            currentTheme = await _preferenceManager.GetCurrentThemeAsync();
        }


        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        async Task DarkMode()
        {
            bool isDarkMode = await _preferenceManager.ToggleDarkModeAsync();
            if (isDarkMode)
            {
                currentTheme = MicroscopeTheme.DefaultTheme;
            }
            else
            {
                currentTheme = MicroscopeTheme.DarkTheme;
            }
        }


        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Personal", href: "#"),
            new BreadcrumbItem("Dashboard", href: "#"),
        };
    }
}

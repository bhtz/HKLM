using System.Collections.Generic;
using Microscope.Admin.Theme;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.ThemeManager;

namespace Microscope.Admin.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private ThemeManagerTheme _themeManager = new ThemeManagerTheme();

        public bool _drawerOpen = true;
        public bool _themeManagerOpen = false;

        private readonly MudTheme _defaultTheme = new MicroscopeTheme();
        private readonly MudTheme _darkTheme = new MicroscopeDarkTheme();

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        void OpenThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        void UpdateTheme(ThemeManagerTheme value)
        {
            _themeManager = value;
            StateHasChanged();
        }

        private void SwitchMode()
        {
            if (_themeManager.Theme == _defaultTheme)
            {
                _themeManager.Theme = _darkTheme;
            }
            else
            {
                _themeManager.Theme = _defaultTheme;
            }
        }

        protected override void OnInitialized()
        {
            _themeManager.Theme = _defaultTheme;
            _themeManager.DrawerClipMode = DrawerClipMode.Never;
            _themeManager.FontFamily = "Montserrat";
            _themeManager.DefaultBorderRadius = 3;
        }

        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Personal", href: "#"),
            new BreadcrumbItem("Dashboard", href: "#"),
        };
    }
}

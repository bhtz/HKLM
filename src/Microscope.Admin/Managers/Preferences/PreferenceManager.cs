using System.Threading.Tasks;
using Microscope.Admin.Infrastructure.Settings;
using MudBlazor;
using Blazored.LocalStorage;

namespace Microscope.Admin.Infrastructure.Managers.Preferences
{
public class PreferenceManager : IPreferenceManager
    {
        private readonly ILocalStorageService _localStorageService;

        public PreferenceManager(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<bool> ToggleDarkModeAsync()
        {
            var preference = await GetPreference();
            preference.IsDarkMode = !preference.IsDarkMode;
            await SetPreference(preference);
            return !preference.IsDarkMode;
        }

        public async Task ChangeLanguageAsync(string languageCode)
        {
            var preference = await GetPreference();
            preference.LanguageCode = languageCode;
            await SetPreference(preference);
        }

        public async Task<MudTheme> GetCurrentThemeAsync()
        {
            var preference = await GetPreference();
            if (preference.IsDarkMode) return MicroscopeTheme.DarkTheme;
            return MicroscopeTheme.DefaultTheme;
        }

        public async Task<Preference> GetPreference()
        {
            return await _localStorageService.GetItemAsync<Preference>("preference") ?? new Preference();
        }

        public async Task SetPreference(Preference preference)
        {
            await _localStorageService.SetItemAsync("preference", preference);
        }
    }
}
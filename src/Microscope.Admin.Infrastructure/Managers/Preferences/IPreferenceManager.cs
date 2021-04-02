using System.Threading.Tasks;
using Microscope.Admin.Infrastructure.Settings;
using MudBlazor;

namespace Microscope.Admin.Infrastructure.Managers.Preferences
{
   public interface IPreferenceManager
    {
        Task SetPreference(Preference preference);

        Task<Preference> GetPreference();

        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();

        Task ChangeLanguageAsync(string languageCode);
    }
}
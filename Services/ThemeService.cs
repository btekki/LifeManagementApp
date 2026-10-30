using CommunityToolkit.Mvvm.ComponentModel;

namespace LifeManagementApp.Services;

public interface IThemeService
{
    AppTheme CurrentTheme { get; }
    void SetTheme(AppTheme theme);
}

public partial class ThemeService : ObservableObject, IThemeService
{
    private const string ThemePreferenceKey = "app_theme_preference";

    [ObservableProperty]
    private AppTheme currentTheme;

    public ThemeService()
    {
        var savedTheme = Preferences.Get(ThemePreferenceKey, "system");

        if (savedTheme == "light")
        {
            currentTheme = AppTheme.Light;
        }
        else if (savedTheme == "dark")
        {
            currentTheme = AppTheme.Dark;
        }
        else
        {
            currentTheme = AppTheme.Unspecified;
        }

        ApplyTheme(currentTheme);
    }

    public void SetTheme(AppTheme theme)
    {
        try
        {
            CurrentTheme = theme;
            ApplyTheme(theme);

            // Save preference
            if (theme == AppTheme.Light)
                Preferences.Set(ThemePreferenceKey, "light");
            else if (theme == AppTheme.Dark)
                Preferences.Set(ThemePreferenceKey, "dark");
            else
                Preferences.Set(ThemePreferenceKey, "system");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to set theme: {ex.Message}");
        }
    }

    private void ApplyTheme(AppTheme theme)
    {
        if (Application.Current == null)
            return;

        // Set the application theme
        Application.Current.UserAppTheme = theme;

        // Reload the theme resources by clearing and re-adding the merged dictionaries
        try
        {
            var resources = Application.Current.Resources;
            var mergedDictionaries = resources?.MergedDictionaries;

            if (mergedDictionaries == null)
                return;

            // Clear existing theme dictionaries
            var toRemove = new List<ResourceDictionary>();
            foreach (var dict in mergedDictionaries)
            {
                if (dict.Source?.OriginalString.Contains("lightTheme") == true || 
                    dict.Source?.OriginalString.Contains("darkTheme") == true)
                {
                    toRemove.Add(dict);
                }
            }

            foreach (var dict in toRemove)
            {
                mergedDictionaries.Remove(dict);
            }

            // Add the appropriate theme dictionary based on the selected theme
            var themeFile = theme == AppTheme.Dark ? "darkTheme.xaml" : "lightTheme.xaml";
            var newThemes = new ResourceDictionary { Source = new Uri($"Resources/Themes/{themeFile}", UriKind.Relative) };
            mergedDictionaries.Add(newThemes);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to apply theme: {ex.Message}");
        }
    }
}

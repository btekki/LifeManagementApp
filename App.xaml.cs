using Microsoft.Extensions.DependencyInjection;

namespace LifeManagementApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            RestoreTheme();
        }

        private static void RestoreTheme()
        {
            var savedTheme = Preferences.Get("AppTheme", string.Empty);

            Current!.UserAppTheme = savedTheme switch
            {
                nameof(AppTheme.Light) => AppTheme.Light,
                nameof(AppTheme.Dark) => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
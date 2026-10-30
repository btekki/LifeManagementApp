namespace LifeManagementApp
{
    public partial class AppShell : Shell
    {
        private const string ThemePreferenceKey = "AppTheme";

        public Command LightModeCommand { get; }
        public Command DarkModeCommand { get; }

        public AppShell()
        {
            LightModeCommand = new Command(SetLightMode);
            DarkModeCommand = new Command(SetDarkMode);

            InitializeComponent();
            BindingContext = this;

            ToolbarItems[0].Command = LightModeCommand;
            ToolbarItems[1].Command = DarkModeCommand;
        }

        private static void SetLightMode()
        {
            Application.Current!.UserAppTheme = AppTheme.Light;
            Preferences.Set(ThemePreferenceKey, nameof(AppTheme.Light));
        }

        private static void SetDarkMode()
        {
            Application.Current!.UserAppTheme = AppTheme.Dark;
            Preferences.Set(ThemePreferenceKey, nameof(AppTheme.Dark));
        }
    }
}

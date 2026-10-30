using Microsoft.Extensions.DependencyInjection;
using LifeManagementApp.Services;

namespace LifeManagementApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Initialize theme from service
            var themeService = ServiceHelper.Current.GetService<IThemeService>();
            if (themeService != null)
            {
                themeService.SetTheme(themeService.CurrentTheme);
            }

            return window;
        }
    }

    internal static class ServiceHelper
    {
        public static IServiceProvider Current { get; set; } = null!;
    }
}

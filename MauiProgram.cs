using Microsoft.Extensions.Logging;
using LifeManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using LifeManagementApp.Services;
using LifeManagementApp.ViewModels;
using LifeManagementApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace LifeManagementApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var dbPath = GetDatabasePath();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            //Services here
            builder.Services.AddSingleton<IThemeService, ThemeService>();
            builder.Services.AddScoped<IWeatherService, OpenMeteoWeatherService>();
            builder.Services.AddHttpClient<OpenMeteoWeatherService>();
            builder.Services.AddScoped<ITodoRepository, TodoRepository>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<TodoListViewModel>();
            builder.Services.AddTransient<AddTodoViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<TodoListPage>();
            builder.Services.AddTransient<AddTodoPage>();
            builder.Services.AddTransient<AppShell>();

            InitializeDatabase(dbPath);

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureTheme();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            ServiceHelper.Current = app.Services;
            return app;
        }

        private static string GetDatabasePath()
        {
            string appDataPath = FileSystem.AppDataDirectory;
            string dbPath = Path.Combine(appDataPath, "lifemanagementapp.db");
            return dbPath;
        }

        private static void InitializeDatabase(string dbPath)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            using(var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }
        }

        private static MauiAppBuilder ConfigureTheme(this MauiAppBuilder builder)
        {
            // Ensure theme resources are loaded early during app initialization
            builder.ConfigureMauiHandlers(handlers =>
            {
                try
                {
                    // Theme initialization is handled in App.xaml.cs CreateWindow
                    // This ensures resources are available when pages are initialized
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to configure theme: {ex.Message}");
                }
            });
            return builder;
        }
    }
}

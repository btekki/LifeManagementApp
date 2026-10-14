using Microsoft.Extensions.Logging;
using LifeManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using LifeManagementApp.Services;
using LifeManagementApp.ViewModels;
using LifeManagementApp.Views;

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
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
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
    }
}

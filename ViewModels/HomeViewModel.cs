using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace LifeManagementApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IWeatherService _weatherService;
    private readonly ITodoRepository _todoRepository;

    private const double DefaultLatitude = 74.26;
    private const double DefaultLongitude = 19.02;

    [ObservableProperty]
    private ObservableCollection<TodoItem> topThreeTodos = new();

    [ObservableProperty]
    private WeatherInfo? currentWeather;

    [ObservableProperty]
    private string kiteSurfAlert = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string latitudeText;

    [ObservableProperty]
    private string longitudeText;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public HomeViewModel(
        IWeatherService weatherService,
        ITodoRepository todoRepository
    )
    {
        _weatherService = weatherService;
        _todoRepository = todoRepository;

        LatitudeText = DefaultLatitude.ToString(CultureInfo.InvariantCulture);
        LongitudeText = DefaultLongitude.ToString(CultureInfo.InvariantCulture);
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if(IsLoading)
            return;
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var weatherTask = _weatherService.GetCurrentWeatherAsync(DefaultLatitude, DefaultLongitude);
            
            var todosTask = _todoRepository.GetAllTodosAsync();

            await Task.WhenAll(weatherTask, todosTask);

            CurrentWeather = await weatherTask;
            UpdateKiteSurfAlert();


            var topTodos = (await todosTask)
                .Where(todo => !todo.IsDone)
                .OrderBy(todo => todo.Priority)
                .ThenBy(todo => todo.DueDate ?? DateTime.MaxValue)
                .Take(3);
                

            TopThreeTodos = new ObservableCollection<TodoItem>(topTodos);

            
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task FetchWeatherAsync()
    {
        if (IsLoading)
            return;

        if(!TryParseCoordinate(LatitudeText, out var latitude) || !TryParseCoordinate(LongitudeText, out var longitude))
        {
            ErrorMessage = "Anna kelvolliset koordinaatit";
                return;
        }

        if(latitude is < -90 or > 90)
        {
            ErrorMessage = "Leveysaste pitää olla väliltä -90 ja 90";
            return;
        }

        if(longitude is < -180 or > 180)
        {
            ErrorMessage = "Pituusasteen pitää olla -180 ja 180 välillä";
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            CurrentWeather = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);

            if(CurrentWeather is null)
            {
                ErrorMessage = "Säätietojen hakeminen epäonnistui";
                KiteSurfAlert = string.Empty;
                return;
            }
            UpdateKiteSurfAlert();

        }
        finally
        {
            IsLoading = false;
        }
    }

    private void UpdateKiteSurfAlert()
    {
        KiteSurfAlert = CurrentWeather?.WindSpeed > 5 ? "KiteSurf-keli" : string.Empty;
    }

    private static bool TryParseCoordinate(string? value, out double coordinate)
    {
        return double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out coordinate) || double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out coordinate);

    }
}
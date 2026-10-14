using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;
using System.Collections.ObjectModel;

namespace LifeManagementApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IWeatherService _weatherService;
    private readonly ITodoRepository _todoRepository;

    private const double latitude = 0.0;//tähän paskmaijan tai vastaavan hauskan paikan koordinaatit
    private const double longitude = 0.0;

    [ObservableProperty]
    private ObservableCollection<TodoItem> topThreeTodos = new();

    [ObservableProperty]
    private WeatherInfo? currentWeather;

    [ObservableProperty]
    private string kiteSurfAlert = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    public HomeViewModel(
        IWeatherService weatherService,
        ITodoRepository todoRepository
    )
    {
        _weatherService = weatherService;
        _todoRepository = todoRepository;
    }
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if(isLoading)
            return;
        isLoading = true;

        try
        {
            var weatherTask = _weatherService.GetCurrentWeatherAsync(latitude, longitude);
            var todosTask = _todoRepository.GetAllTodosAsync();

            await Task.WhenAll(weatherTask, todosTask);

            CurrentWeather = await weatherTask;

            var topTodos = (await todosTask)
                .OrderByDescending(todo => todo.Priority)
                .ThenBy(todo => todo.DueDate ?? DateTime.MaxValue)
                .Take(3);
                

            TopThreeTodos = new ObservableCollection<TodoItem>(topTodos);

            KiteSurfAlert = CurrentWeather?.WindSpeed > 5
                ? "Kitesurf-keli!"
                : string.Empty;
        }
        finally
        {
            isLoading = false;
        }
    }
}
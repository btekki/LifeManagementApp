using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

public partial class AddTodoViewModel : ObservableObject
{
    private readonly ITodoRepository _todoRepository;

    [ObservableProperty] 
    private string title = string.Empty;

    [ObservableProperty]
    private int priority = 3;

    [ObservableProperty]
    private DateTime dueDatePickerDate = DateTime.Today;

    [ObservableProperty]
    private bool hasDueDate;

    public DateTime? DueDate => HasDueDate ? DueDatePickerDate : null;

    private const int MinPriority = 1;
    private const int MaxPriority = 5;

    // Pidetään arvo rajoissa, jotta Stepperin sisäinen arvo ei karkaa rajojen yli
    partial void OnPriorityChanged(int value)
    {
        var clamped = Math.Clamp(value, MinPriority, MaxPriority);
        if (clamped != value)
            Priority = clamped;
    }

    partial void OnHasDueDateChanged(bool value) => OnPropertyChanged(nameof(DueDate));

    partial void OnDueDatePickerDateChanged(DateTime value) => OnPropertyChanged(nameof(DueDate));
    
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public AddTodoViewModel(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [RelayCommand]
    private async Task SaveTodoAsync()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "Otsikko on pakollinen";
            return;
        }

        IsLoading = true;

        try
        {
            var todo = new TodoItem
            {
                Title = Title.Trim(),
                Priority = Priority,
                DueDate = DueDate,
                IsDone = false,
                CreatedAt = DateTime.UtcNow
            };

            await _todoRepository.AddTodoAsync(todo);
            await Shell.Current.GoToAsync("//TodoList");

        }

        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task CancelAsync()=>Shell.Current.GoToAsync("//TodoList");
}
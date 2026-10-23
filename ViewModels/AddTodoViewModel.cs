using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

public partial class AddTodoViewModel : ObservableObject, IQueryAttributable
{
    private readonly ITodoRepository _todoRepository;
    private TodoItem? _editingTodo;

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

    public bool IsEditing => _editingTodo != null;

    public string SaveButtonText => IsEditing ? "Tallenna muutokset" : "Tallenna";

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("id", out var idValue)
            || !int.TryParse(idValue?.ToString(), out var id)
            || id <= 0)
            return;

        query.Remove("id");
        var todo = await _todoRepository.GetTodoByIdAsync(id);
        if (todo == null)
            return;

        _editingTodo = todo;
        Title = todo.Title;
        Priority = todo.Priority;
        HasDueDate = todo.DueDate.HasValue;
        DueDatePickerDate = todo.DueDate ?? DateTime.Today;
        ErrorMessage = string.Empty;
        NotifyEditStateChanged();
    }

    public void Reset()
    {
        _editingTodo = null;
        Title = string.Empty;
        Priority = 3;
        HasDueDate = false;
        DueDatePickerDate = DateTime.Today;
        ErrorMessage = string.Empty;
        NotifyEditStateChanged();
    }

    private void NotifyEditStateChanged()
    {
        OnPropertyChanged(nameof(IsEditing));
        OnPropertyChanged(nameof(SaveButtonText));
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
            if (_editingTodo != null)
            {
                _editingTodo.Title = Title.Trim();
                _editingTodo.Priority = Priority;
                _editingTodo.DueDate = DueDate;
                await _todoRepository.UpdateTodoAsync(_editingTodo);
            }
            else
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
            }

            Reset();
            await Shell.Current.GoToAsync("//TodoList");

        }

        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteTodoAsync()
    {
        if (_editingTodo == null)
            return;

        await _todoRepository.DeleteTodoAsync(_editingTodo.Id);
        Reset();
        await Shell.Current.GoToAsync("//TodoList");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        Reset();
        await Shell.Current.GoToAsync("//TodoList");
    }
}
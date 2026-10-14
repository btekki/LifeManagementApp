using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifemanagementApp.Services;
using System.Collections.ObjectModel;

namespace LifeManagementApp.ViewModels;

public partial class TodoListViewModel : ObservableObject
{
    private readonly ITodoRepository _todoRepository;
    private List<TodoItem> _todos = new();

    [ObservableProperty]
    private ObservableCollection<TodoItem> allTodos = new();

    [ObservableProperty]
    private int sortBy;

    [ObservableProperty]
    private bool isLoading;

    public TodoListViewModel(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;

    }

    [RelayCommand]
    private async Task LoadTodosAsync()
    {
        if(IsLoading)
            return;

        IsLoading = true;
        try
        {
            _todos = await _todoRepository.GetAllTodosAsync();
            ApplySort();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task SortByPriorityAsync()
    {
        SortBy = 0;
        ApplySort();
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task SortByDueDateAsync()
    {
        SortBy = 1;
        ApplySort();
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task ToggleTodoAsync(TodoItem todo)
    {
        todo.IsDone = !todo.IsDone;
        await _todoRepository.UpdateTodoAsync(todo);
    }

    [RelayCommand]
    private async Task DeleteTodoAsync(TodoItem todo)
    {
        await _todoRepository.DeleteTodoAsync(todo.Id);
        _todos.Remove(todo);
        ApplySort();
    }

    private void ApplySort()
    {
        IEnumerable<TodoItem> sortedTodos = SortBy switch
        {
            0 => _todos.OrderByDescending(todo => todo.Priority),
            1 => _todos
                .Where(todo =>
                    !todo.DueDate.HasValue || todo.DueDate.Value.Date >= DateTime.Today)
                .OrderBy(todo => todo.DueDate ?? DateTime.MaxValue),
            _ => _todos
            
        };
        AllTodos = new ObservableCollection<TodoItem>(sortedTodos);
    }
}
using LifeManagementApp.ViewModels;
using LifeManagementApp.Models;

namespace LifeManagementApp.Views;

public partial class TodoListPage : ContentPage
{
    public TodoListPage(TodoListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if(BindingContext is TodoListViewModel viewModel)
            await viewModel.LoadTodosCommand.ExecuteAsync(null);
    }

    private async void OnTodoCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(sender is not CheckBox checkBox || checkBox.BindingContext is not TodoItem todo || BindingContext is not TodoListViewModel viewModel || e.Value == todo.IsDone)
            return;
        await viewModel.ToggleTodoCommand.ExecuteAsync(todo);
    }

    private async void OnAddTodoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AddTodo");
    }
}
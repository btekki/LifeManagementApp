using LifeManagementApp.ViewModels;

namespace LifeManagementApp.Views;

public partial class AddTodoPage : ContentPage
{
    public AddTodoPage(AddTodoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
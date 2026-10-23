using LifeManagementApp.ViewModels;

namespace LifeManagementApp.Views;

public partial class AddTodoPage : ContentPage
{
    public AddTodoPage(AddTodoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // Tyhjennetään lomake poistuttaessa, ettei muokattavan tehtävän tiedot jää näkyviin
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is AddTodoViewModel viewModel)
            viewModel.Reset();
    }
}
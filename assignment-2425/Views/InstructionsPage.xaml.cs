using assignment_2425.ViewModels;

namespace assignment_2425.Views;

public partial class InstructionsPage : ContentPage
{
    public InstructionsPage(GameData gameData)
	{
		InitializeComponent();
        BindingContext = new InstructionsPageViewModel(gameData);
    }
}
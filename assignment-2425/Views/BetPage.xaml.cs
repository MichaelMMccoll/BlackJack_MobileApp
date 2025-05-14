using assignment_2425.ViewModels;
using assignment_2425.Models;

namespace assignment_2425.Views;

public partial class BetPage : ContentPage
{
    public BetPage(GameData gameData)
	{
		InitializeComponent();
        BindingContext = new BetPageViewModel(gameData);
    }

    //Removes the ability to press the phone back button
    protected override bool OnBackButtonPressed()
    {
        return true;
    }

}
using assignment_2425.ViewModels;
using Microsoft.Extensions.Configuration;
namespace assignment_2425.Views;

public partial class BackJackPage : ContentPage
{
    private readonly GameData _gameData;
    IConfiguration _configuration;
    public BackJackPage(GameData gameData,
                        IConfiguration configuration)
    {
        InitializeComponent();
        _gameData = gameData;
        _configuration = configuration;
        BindingContext = new BlackJackPageViewModel(_gameData,_configuration);
    }

    //Removes the ability to press the phone back button
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}

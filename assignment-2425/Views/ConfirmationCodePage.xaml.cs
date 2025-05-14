using assignment_2425.ViewModels;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;

namespace assignment_2425.Views;

public partial class ConfirmationCodePage : ContentPage
{
    private GameData _gameData;
    private IConfiguration _configuration;
    public ConfirmationCodePage(GameData gameData,
                                IConfiguration configuration)
	{
		InitializeComponent();
        _gameData = gameData;
        _configuration = configuration;
        BindingContext = new ConfirmationCodeViewModel(_gameData, _configuration);
    }
}
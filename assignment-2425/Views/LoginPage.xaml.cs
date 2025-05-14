using assignment_2425.ViewModels;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;

namespace assignment_2425.Views;

public partial class LoginPage : ContentPage
{
    private readonly GameData _gameData;
    private IConfiguration _configuration;


    public LoginPage(GameData gameData,
                     IConfiguration configuration)
	{
		InitializeComponent();
        _gameData = gameData;
        _configuration = configuration;
        BindingContext = new LoginPageViewModel(gameData, configuration);
        if (_gameData.Settings.HapticFeedback)
        {
            email.TextChanged += OnTextChanged;
            password.TextChanged += OnTextChanged;
        }
    }


    //Haptic Feedback
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
    }
}
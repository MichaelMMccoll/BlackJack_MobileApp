using assignment_2425.ViewModels;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;

namespace assignment_2425.Views;

public partial class SignUpPage : ContentPage
{
    private readonly GameData _gameData;
    public SignUpPage(GameData gameData,
                      IConfiguration configuration)
	{
		InitializeComponent();
        _gameData = gameData;

        BindingContext = new SignUpViewModel(gameData,configuration);
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
using assignment_2425.DataBase;
using assignment_2425.ViewModels;
using Microsoft.Extensions.Configuration;

namespace assignment_2425.Views;

public partial class SettingPage : ContentPage
{
    public SettingPage(GameData gameData,
                       BlackJackDatabase blackJackDatabase,
                       IConfiguration configuration)
    {
        InitializeComponent();
        BindingContext = new SettingsPageViewModel(gameData,configuration,blackJackDatabase);

        if (gameData.Settings.HapticFeedback)
        {
            NameBox.TextChanged += OnTextChanged;
        }
    }

    //Haptic Feedback
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
    }
}
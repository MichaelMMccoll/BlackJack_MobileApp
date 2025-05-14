using assignment_2425.Helpers;
using assignment_2425.Services;
using assignment_2425.Views;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;
using MvvmHelpers;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        private readonly GameData _gameData;
        private readonly IConfiguration _configuration;
        private readonly NavigationHelper _navigationHelper = new();
        private readonly PopUpHelper _popUpHelper = new();
        private readonly ConnectivityService _connectivityService = new();
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public ICommand LoginCommand { get; }
        public ICommand BackCommand { get; }
        public LoginPageViewModel(GameData gameData, IConfiguration configuration)
        {
            _gameData = gameData;
            _configuration = configuration;

            LoginCommand = new Command(async () => await LogInAsync());
            BackCommand = new Command(async () => await _navigationHelper.Navigate("Settings", _gameData.Settings.TextToSpeech));
        }

        //Log in Function 
        private async Task LogInAsync()
        {
            if (_connectivityService.CheckWifiStatus())
            {
                _popUpHelper.ShowPopUp("No internet connection Found", _gameData.Settings.TextToSpeech);
                return;

            }
                var authService = new CognitoService2(_configuration);

            //Ensure that email and password are not empty
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || !Email.Contains("@"))
            {
                _popUpHelper.ShowPopUp("Password/email are empty", _gameData.Settings.TextToSpeech);
                return;
            }

            var response = await authService.SignInAndGetUserIdAsync(Email, Password);

            //Added response to Player object
            if (!string.IsNullOrEmpty(response.idToken))
            {
                var dynamoDB = new CognitoAuthService(response.idToken, _configuration);
                var playerData = await dynamoDB.GetUserDataAsync(response.userId);

                _gameData.Blackjack.Player.Imageurl = playerData.ImageUrl;
                _gameData.Blackjack.Player.Username = playerData.UserName;
                _gameData.Blackjack.Player.Losses = playerData.Losses;
                _gameData.Blackjack.Player.Wins = playerData.Wins;
                _gameData.Blackjack.Player.Balance = playerData.Balance;
                _gameData.Blackjack.Player.Userid = response.userId;
                _gameData.Blackjack.Player.Idtoken = response.idToken;

                await Shell.Current.GoToAsync(nameof(MainPage));
            }
            else
            {
                _popUpHelper.ShowPopUp("Password or Email are incorrect", _gameData.Settings.TextToSpeech);
            }
        }
    }
}

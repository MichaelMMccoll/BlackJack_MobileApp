using assignment_2425.Helpers;
using assignment_2425.Services;
using Microsoft.Extensions.Configuration;
using MvvmHelpers;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly GameData _gameData;
        private readonly IConfiguration _configuration;
        private readonly CognitoService2 _authService;
        private readonly NavigationHelper _navigationHelper = new();
        private readonly PopUpHelper _popUpHelper = new();
        private readonly ConnectivityService _connectivityService = new();

        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public ICommand SignUpCommand { get; }
        public ICommand BackCommand { get; }

        public SignUpViewModel(GameData gameData, IConfiguration configuration)
        {
            _gameData = gameData;
            _configuration = configuration;
            _authService = new CognitoService2(configuration);

            SignUpCommand = new Command(async () => await SignUpAsync());
            BackCommand = new Command(async () => await _navigationHelper.Navigate("Settings", _gameData.Settings.TextToSpeech));
        }

        //signUp function
        private async Task SignUpAsync()
        {
            if (_connectivityService.CheckWifiStatus())
            {
                _popUpHelper.ShowPopUp("No internet connection Found", _gameData.Settings.TextToSpeech);
                return;
            }

            //Ensure email and password are not null
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                _popUpHelper.ShowPopUp("Email or Password are invalid", _gameData.Settings.TextToSpeech);
                return;
            }

            _gameData.Blackjack.Player.Email = Email;
            _gameData.Blackjack.Player.Password = Password;
            var response = await _authService.SignUpUserAsync(Email, Password);

            if (response)
            {
                await _navigationHelper.Navigate("Confirmation", _gameData.Settings.TextToSpeech);
            }
            else
            {
                _popUpHelper.ShowPopUp("Email has already been used", _gameData.Settings.TextToSpeech);
            }
        }
    }
}

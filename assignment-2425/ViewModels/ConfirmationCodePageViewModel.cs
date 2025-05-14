using assignment_2425.Helpers;
using assignment_2425.Services;
using Microsoft.Extensions.Configuration;
using MvvmHelpers;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public partial class ConfirmationCodeViewModel : BaseViewModel
    {
        private GameData _gameData;
        private IConfiguration _configuration;
        private readonly NavigationHelper _navigationHelper = new();
        private readonly PopUpHelper _popUpHelper = new();
        private readonly ConnectivityService _connectivityService = new();

        public string ConfirmationCode { get; set; } = "";
        public ICommand ConfirmationCommand { get; }
        public ICommand BackCommand { get; }

        public ConfirmationCodeViewModel(GameData gameData, IConfiguration configuration)
        {
            _gameData = gameData;
            _configuration = configuration;
            ConfirmationCommand = new Command(async () => await Confirmation());
            BackCommand = new Command(async () => await _navigationHelper.Navigate("Signup",_gameData.Settings.TextToSpeech));
        }

        //Sends confirmation code and signs user in.
        private async Task Confirmation()
        {
            var authService = new CognitoService2(_configuration);


            if (_connectivityService.CheckWifiStatus())
            {
                _popUpHelper.ShowPopUp("No internet connection Found", _gameData.Settings.TextToSpeech);
                return;
            }

            // Returns if user was able to sing in
            var isSignedUp = await authService.ConfirmSignUpAsync(_gameData.Blackjack.Player.Email, ConfirmationCode);

            if (isSignedUp)
            {
                var response = await authService.SignInAndGetUserIdAsync(_gameData.Blackjack.Player.Email, _gameData.Blackjack.Player.Password);
                _gameData.Blackjack.Player.Userid = response.userId;
                _gameData.Blackjack.Player.Idtoken = response.idToken;

                var dynamoDB = new CognitoAuthService(response.idToken, _configuration);
                var currentName = RandomString();

                await dynamoDB.SaveUserDataAsync(response.userId,
                                                 _gameData.Blackjack.Player.Losses,
                                                 _gameData.Blackjack.Player.Wins,
                                                 _gameData.Blackjack.Player.Balance,
                                                 currentName,
                                                 _gameData.Blackjack.Player.Imageurl);

                _gameData.Blackjack.Player.Username = currentName;
                await _navigationHelper.Navigate("Main", _gameData.Settings.TextToSpeech);
            }
            else
            {
                _popUpHelper.ShowPopUp("Invalid Confirmation Code", _gameData.Settings.TextToSpeech);
            }
        }

        //Creates a random string for users initial name 
        private string RandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
}

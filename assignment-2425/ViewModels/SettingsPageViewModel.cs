using assignment_2425.DataBase;
using assignment_2425.Helpers;
using assignment_2425.Models;
using assignment_2425.Services;
using Microsoft.Extensions.Configuration;
using MvvmHelpers;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        private readonly GameData _gameData;
        private readonly IConfiguration _configuration;
        private readonly BlackJackDatabase _blackJackDatabase;
        private string _imageBack;
        private string _imageSunMoon;
        private readonly NavigationHelper _navigationHelper = new();
        private readonly PopUpHelper _popUpHelper = new();

        public Player Player { get; set; }
        public Settings Settings { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        public bool IsValuesVisible { get; set; } = false;
        public string BackButtonIcon
        {
            get => _imageBack;
            set
            {
                if(_imageBack != value)
                {
                    _imageBack = value;
                    OnPropertyChanged(nameof(BackButtonIcon));
                }
            }
        }
        public string SunMoonIcon
        {
            get => _imageSunMoon;
            set
            {
                if (_imageSunMoon != value)
                {
                    _imageSunMoon = value;
                    OnPropertyChanged(nameof(SunMoonIcon));
                }
            }
        }
        public ICommand TakePictureCommand { get; }
        public ICommand ChangeColorModeCommand { get; }
        public ICommand ChangeFontSizeCommand { get; }

        public ICommand NavigateCommand { get; }
        public ICommand ChangeNameCommand { get; }
        public ICommand SaveLocalSettingsCommand { get; }

        public ICommand SignOutCommand { get; }

        public SettingsPageViewModel(GameData gameData,
                                     IConfiguration configuration,
                                     BlackJackDatabase blackJackDatabase)
        {
            _gameData = gameData;
            _configuration = configuration;
            _blackJackDatabase = blackJackDatabase;
            _imageBack = BackButtonIcon;
            _imageSunMoon = SunMoonIcon;
            Player = _gameData.Blackjack.Player;
            Settings = _gameData.Settings;
            setBackButton();

            if (string.IsNullOrEmpty(_gameData?.Blackjack?.Player?.Idtoken) && string.IsNullOrEmpty(_gameData?.Blackjack?.Player?.Userid))
            {
                IsLoggedIn = true;
                IsValuesVisible = false;

            }
            else
            {
                IsLoggedIn = false;
                IsValuesVisible = true;
            }

            TakePictureCommand = new Command(async () => await TakePhoto());
            ChangeColorModeCommand = new Command(ChangeColor);
            ChangeFontSizeCommand = new Command<string>(ChangeFontSize);
            NavigateCommand = new Command<string>(async (name) => await _navigationHelper.Navigate(name, _gameData.Settings.TextToSpeech));
            ChangeNameCommand = new Command<string>(async (name) => await NameChange(name));
            SaveLocalSettingsCommand = new Command(async () => await CheckBoxChange());
            SignOutCommand = new Command(SignOut);
 
        }


        //Save checkBox data
        private async Task CheckBoxChange()
        {
            await _blackJackDatabase.SaveItemAsync(_gameData.Settings);
        }

        //Change the name of the user
        private async Task NameChange(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var client = new CognitoAuthService(_gameData.Blackjack.Player.Idtoken, _configuration);

                await client.SaveUserDataAsync(_gameData.Blackjack.Player.Userid,
                                               _gameData.Blackjack.Player.Wins,
                                               _gameData.Blackjack.Player.Losses,
                                               _gameData.Blackjack.Player.Balance,
                                               name,
                                               _gameData.Blackjack.Player.Imageurl);

                _gameData.Blackjack.Player.Username = name;
            }
        }

        //Rests value as user signs out 
        private async void SignOut()
        {
            _gameData.Reset();
            IsValuesVisible = false;
            IsLoggedIn = true;
            await _navigationHelper.Navigate("Main", _gameData.Settings.TextToSpeech);
            _popUpHelper.ShowPopUp("Successfully logged out", _gameData.Settings.TextToSpeech);
        }

        //Changes Font size based on what button is pressed: S/M/L
        private void ChangeFontSize(string size)
        {
            var fontSize = 0;
            switch (size)
            {
                case "S":
                    fontSize = 14;
                    Application.Current.Resources["IconHeightSize"] = 30;
                    Application.Current.Resources["IconWidthSize"] = 20;
                    Application.Current.Resources["ImageHeightSize"] = 85;
                    Application.Current.Resources["ImageWidthSize"] = 100;
                    break;
                case "M":
                    fontSize = 18;
                    Application.Current.Resources["IconHeightSize"] = 35;
                    Application.Current.Resources["IconWidthSize"] = 25;
                    Application.Current.Resources["ImageHeightSize"] = 90;
                    Application.Current.Resources["ImageWidthSize"] = 105;
                    break;
                case "L":
                    fontSize = 22;
                    Application.Current.Resources["IconHeightSize"] = 40;
                    Application.Current.Resources["IconWidthSize"] = 30;
                    Application.Current.Resources["ImageHeightSize"] = 95;
                    Application.Current.Resources["ImageWidthSize"] = 110;
                    break;
                default:
                    fontSize = 18;
                    break;
            }

            Application.Current.Resources["FontSize"] = fontSize;
        }

        //Change from darkmode to lightmode
        private void ChangeColor()
        {
            _gameData.Settings.DarkMode = _gameData.Settings.DarkMode ? false : true;

            if (_gameData.Settings.DarkMode)
            {
                Application.Current.Resources["ButtonBackColor"] = "#414144";
                Application.Current.Resources["BackButton"] = "yellowbackarrow.svg";
                Application.Current.Resources["LabelTextColor"] = "#E6BD57";
                Application.Current.Resources["ButtonTextColor"] = "#E6BD57";
                Application.Current.Resources["BackGroundColorSet"] = "#1E1E20";
                BackButtonIcon = "yellowbackarrow.svg";
                SunMoonIcon = "sun.svg";
                Application.Current.Resources["InformationButton"] = "dark_infoicon.svg";
                Application.Current.Resources["AudioButton"] = "dark_yellow_icon.svg";


                if (_gameData.Blackjack.Player.Imageurl == "defaultprofile.svg" || _gameData.Blackjack.Player.Imageurl == "darkdefaultprofile.svg")
                {
                    _gameData.Blackjack.Player.Imageurl = "darkdefaultprofile.svg";
                }
            }
            else
            {
                Application.Current.Resources["ButtonBackColor"] = "#1B4332";
                Application.Current.Resources["BackButton"] = "backarrow.svg";
                Application.Current.Resources["LabelTextColor"] = "#000000";
                Application.Current.Resources["ButtonTextColor"] = "#FFFFFF";
                Application.Current.Resources["BackGroundColorSet"] = "#B7D1B7";
                Application.Current.Resources["InformationButton"] = "info_icon.svg";
                Application.Current.Resources["AudioButton"] = "audio_icon.svg";
                BackButtonIcon = "backarrow.svg";
                SunMoonIcon = "moon.svg";

                if (_gameData.Blackjack.Player.Imageurl == "defaultprofile.svg" || _gameData.Blackjack.Player.Imageurl == "darkdefaultprofile.svg")
                {
                    _gameData.Blackjack.Player.Imageurl = "defaultprofile.svg";
                }
            }
        }
        //Take a photo for user profile
        private async Task TakePhoto()
        {
            var photoData = await new TakingImage().TakePicture();


            var client = new CognitoAuthService(_gameData.Blackjack.Player.Idtoken, _configuration);
            if (photoData.ImagePath != null)
            {
                var url = await client.UploadImageAsync(photoData.Image, photoData.ImagePath);
                _gameData.Blackjack.Player.Imageurl = url;


                await client.SaveUserDataAsync(_gameData.Blackjack.Player.Userid,
                                               _gameData.Blackjack.Player.Wins,
                                               _gameData.Blackjack.Player.Losses,
                                               _gameData.Blackjack.Player.Balance,
                                               _gameData.Blackjack.Player.Username,
                                               _gameData.Blackjack.Player.Imageurl);
            }
        }

        private void setBackButton()
        {
            if (!_gameData.Settings.DarkMode)
            {
                BackButtonIcon = "backarrow.svg";
                SunMoonIcon = "moon.svg";
            }
            else
            {
                BackButtonIcon = "yellowbackarrow.svg";
                SunMoonIcon = "sun.svg";
            }
        }
    }
}

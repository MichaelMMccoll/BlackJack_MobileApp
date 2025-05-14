using assignment_2425.DataBase;
using assignment_2425.Helpers;
using assignment_2425.Models;
using assignment_2425.Services;
using assignment_2425.Views;
using CommunityToolkit.Maui.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly GameData _gameData;
        private readonly BlackJackDatabase _dataBase;
        private readonly Random _random = new Random();
        private readonly NavigationHelper _navigationHelper = new();
        private readonly PopUpHelper _popUpHelper = new();
        private readonly ConnectivityService _connectivityService = new();
        public ObservableCollection<string> FallingImages { get; } = new();
        public ICommand NavigateCommand { get; }
        public ICommand LeaderBoardCommand { get; }

        public MainPageViewModel(GameData gameData, BlackJackDatabase dataBase, Settings settings)
        {
            _gameData = gameData;
            _gameData.Settings = settings;
            _dataBase = dataBase;

            Connectivity.ConnectivityChanged += CheckForWifi;
            NavigateCommand = new Command<string>(async (name) => await _navigationHelper.Navigate(name, _gameData.Settings.TextToSpeech));
            LeaderBoardCommand = new Command(LeaderBoard);

            OnRun();
        }

        //Stores settings values into settings
        private async void OnRun()
        {
            var data = await _dataBase.GetItemsAsync();

            if (data != null)
            {
                var settings = data.First();
                _gameData.Settings.Id = settings.Id;
                _gameData.Settings.Vibration = settings.Vibration;
                _gameData.Settings.TextToSpeech = settings.TextToSpeech;
                _gameData.Settings.HapticFeedback = settings.HapticFeedback;
            }

                // retrieves card for fallinf animation
                PopulateFalling();
        }

        //Adds cards to fallinga animation thread
        private void PopulateFalling()
        {
            var fruits = new[] { "ace_apple.svg", "ace_pear.svg", "ace_banana.svg", "ace_orange.svg" };
            foreach (var fruit in fruits)
            {
                StartFallingAnimation(fruit);
            }
        }

        //Check internet connection
        private void CheckForWifi(object sender, ConnectivityChangedEventArgs e)
        {
            _connectivityService.CheckForWifi(_gameData);
        }

        //Starts falling animtion on mainThread with delay between addition
        public async void StartFallingAnimation(string imageUrl)
        {
            while (true)
            {
                await Task.Delay(_random.Next(2000, 10000));
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    FallingImages.Add(imageUrl);
                });
            }
        }

        //Adds balance when user presses card
        public void OnImagePressed()
        {
            _gameData.Blackjack.Player.Balance += 5;
        }

        //Navigates to the leaderboard page
        private async void LeaderBoard()
        {
            if (!string.IsNullOrEmpty(_gameData.Blackjack.Player.Userid))
            {
                await _navigationHelper.Navigate("LeaderBoard", _gameData.Settings.TextToSpeech);
            }
            else
            {
                _popUpHelper.ShowPopUp("Please Log in",_gameData.Settings.TextToSpeech);
            }
        }

        public void UpdateSize(double width)
        {
            var newWidth = width * 50 / 100;
            Application.Current.Resources["InputBoxWidth"] = newWidth;
            Application.Current.Resources["ButtonBoxWidth"] = width * 25 / 100;
            Application.Current.Resources["LeaderboardWidth"] = width * 90 / 100;
        }
    }
}

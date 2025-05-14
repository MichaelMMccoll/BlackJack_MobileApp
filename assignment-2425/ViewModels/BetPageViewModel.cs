using assignment_2425.Helpers;
using assignment_2425.Services;
using assignment_2425.Views;
using MvvmHelpers;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public class BetPageViewModel : BaseViewModel
    {
        private GameData _gameData;
        private readonly AccessabilityHelper _helper = new();
        private readonly NavigationHelper _navigationHelper = new();
        public Player Player { get; set; }
        public ICommand BetCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public BetPageViewModel(GameData gameData)
        {
            _gameData = gameData;
            Player = _gameData.Blackjack.Player;

            BetCommand = new Command<string>(ExecuteBet);
            BackCommand = new Command(async () => await _navigationHelper.Navigate("Main", _gameData.Settings.TextToSpeech));

            _helper.TextToSpeach("Your current balance is " +
            _gameData.Blackjack.Player.Balance,
            _gameData.Settings.TextToSpeech
            );

            _helper.TextToSpeach("Please select a Bet", _gameData.Settings.TextToSpeech);
        }

        //Places a bet
        private async void ExecuteBet(string amount)
        {
            if (Player != null)
            {
                _helper.TextToSpeach("You placed a bet of "+amount, _gameData.Settings.TextToSpeech);
                _helper.HapticHelper(_gameData.Settings.HapticFeedback);

                Player.Bet = Convert.ToInt32(amount);
                Player.Balance -= Convert.ToInt32(amount);

                await Shell.Current.GoToAsync(nameof(BackJackPage));
            }
        }
    }
}

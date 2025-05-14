using assignment_2425.Helpers;
using assignment_2425.Services;
using MvvmHelpers;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public partial class InstructionsPageViewModel : BaseViewModel
    {
        private readonly NavigationHelper _navigationHelper = new();
        private readonly AccessabilityHelper _accessabilityHelper = new();
        private readonly GameData _gameData;
        public ICommand NavigationCommand { get; }
        public ICommand ReadCommand { get; }

        public InstructionsPageViewModel(GameData gameData)
        {
            _gameData = gameData;
            NavigationCommand = new Command(async () => await _navigationHelper.Navigate("Main", _gameData.Settings.TextToSpeech));
            ReadCommand =  new Command(PlayAudio);
        }

        private void PlayAudio()
        {
            _accessabilityHelper.TextToSpeach("Basic Game Loop. \r\n1 Press Start\r\n2 Place a Bet ranging from 10 to 150\r\n3 Play Against a Bot\r\n4 Repeat\r\n\r\nThe Leaderboard Displays Top 10 Players based on Wins.\r\nWithin settings you can:\r\n\r\n-Log In\r\n-Sign Up\r\n-Change the local settings \r\n-Check player stats\r\n", _gameData.Settings.TextToSpeech);
        }
    }
}

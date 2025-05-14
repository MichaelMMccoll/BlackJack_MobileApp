using assignment_2425.Helpers;
using assignment_2425.Models;
using assignment_2425.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public partial class LeaderBoardViewModel : ObservableObject
    {
        private readonly GameData _gameData;
        private readonly IConfiguration _configuration;
        private readonly NavigationHelper _navigationHelper = new();

        public ObservableCollection<LeaderBoardObject> LeaderBoard { get; set; } = new ObservableCollection<LeaderBoardObject>();
        public ICommand LoadLeaderBoardCommand { get; }
        public ICommand BackCommand { get; }

        public LeaderBoardViewModel(GameData gameData,
                                    IConfiguration configuration)
        {
            _gameData = gameData;
            _configuration = configuration;

            LoadLeaderBoardCommand = new Command(async () => await LoadLeaderBoard());
            BackCommand = new Command(async () => await _navigationHelper.Navigate("Main", _gameData.Settings.TextToSpeech));
            LoadLeaderBoardCommand.Execute(null);
        }

        //fetches the data from the db and adds to object
        public async Task LoadLeaderBoard()
        {
            var dynamoDB = new CognitoAuthService(_gameData.Blackjack.Player.Idtoken, _configuration);
            var leaderBoard = await dynamoDB.GetLeaderBoardDataAsync(_gameData.Blackjack.Player.Userid);

            AddLeaderBoard(leaderBoard);
        }

        //Add requested leaderboard to object list 
        private void AddLeaderBoard(List<LeaderBoardObject> list)
        {
            foreach (var item in list)
            {
                LeaderBoard.Add(item);
            }
        }
    }
}

using assignment_2425.Helpers;
using assignment_2425.Services;
using assignment_2425.Views;
using Microsoft.Extensions.Configuration;
using MvvmHelpers;

namespace assignment_2425.ViewModels
{
    public partial class BlackJackPageViewModel : BaseViewModel
    {
        private readonly GameData _gameData;
        IConfiguration _configuration;
        private readonly AccessabilityHelper _helper = new();
        private readonly PopUpHelper _popUpHelper = new();
        public Player Player { get; set; }
        public Player Dealer { get; set; }
        public Command GetHandCommand { get; set; }
        public Command GetCardCommand { get; set; }
        public Command HoldCommand { get; set; }
        public Command InformationCommand { get; set; }
        public bool TextToSpeach { get; set; }
        public bool Vibration { get; set; }

        public string BetImage { get; set; }
        public BlackJackPageViewModel(GameData gameData, IConfiguration configuration)
        {
            _gameData = gameData;
            _configuration = configuration;
            BetImage = _gameData.Blackjack.Player.GetBetImage();
            Player = _gameData.Blackjack.Player;
            Dealer = _gameData.Blackjack.Dealer;
            TextToSpeach = _gameData.Settings.TextToSpeech;
            Vibration = _gameData.Settings.Vibration;
            ClearHand();
            GetHandCommand = new Command( async () => await GetHand());
            HoldCommand = new Command( async  ( ) => await Hold());
            InformationCommand = new Command(BlackJackRules);
            GetCardCommand = new Command( async () => await GetCard());
            //Ensures users get their hand 
            GetHandCommand.Execute(null);
            _gameData.Blackjack.Dealer.CurrentHand[1].Image.Source = "backcard.svg";
        }

        //Stand and let dealer draw until loss or win
        private async Task Hold()
        {
            _helper.TextToSpeach("You have pressed Hold", _gameData.Settings.TextToSpeech);
            _helper.HapticHelper(_gameData.Settings.HapticFeedback);
            _gameData.Blackjack.Dealer.CurrentHand[1] = Player.GetImage(_gameData.Blackjack.Dealer.CurrentHand[1]);

            await (_gameData.Blackjack.DealerDraw());
            await Task.Delay(2000);

            //Check if delear has a better hand
            if (_gameData.Blackjack.Dealer.Currenthandvalue > _gameData.Blackjack.Player.Currenthandvalue &&
                _gameData.Blackjack.Dealer.Currenthandvalue <= 21)
            {
                _helper.TextToSpeach("Dealer Wins", TextToSpeach);

                _gameData.Blackjack.SmartPlayer.WLRatio.Add(1);
                _gameData.Blackjack.SmartPlayer.Recalc();
                _gameData.Blackjack.Player.Losses++;


                _popUpHelper.ShowPopUp("Dealer Wins", _gameData.Settings.TextToSpeech);
            }//Checks to see if both dealer and player have the same hand value
            else if (_gameData.Blackjack.Dealer.Currenthandvalue == _gameData.Blackjack.Player.Currenthandvalue)
            {
                _helper.TextToSpeach("It's a draw", TextToSpeach);

                _gameData.Blackjack.Player.Balance += _gameData.Blackjack.Player.Bet;

                _popUpHelper.ShowPopUp("It's a draw", _gameData.Settings.TextToSpeech);
            }
            else
            {
                _helper.TextToSpeach("You Win", TextToSpeach);

                _gameData.Blackjack.SmartPlayer.WLRatio.Add(0);
                _gameData.Blackjack.SmartPlayer.Recalc();
                _gameData.Blackjack.Player.Wins++;
                _gameData.Blackjack.Player.Balance += _gameData.Blackjack.Player.Bet * 2;

                _popUpHelper.ShowPopUp("You Win", _gameData.Settings.TextToSpeech);
            }

            _helper.VibrationHelper(1000, _gameData.Settings.HapticFeedback);

            //db calls
            if (!string.IsNullOrEmpty(_gameData.Blackjack.Player.Idtoken))
            {
                var dynamoDB = new CognitoAuthService(_gameData.Blackjack.Player.Idtoken,
                                                      _configuration);

                await dynamoDB.SaveUserDataAsync(_gameData.Blackjack.Player.Userid,
                                                 _gameData.Blackjack.Player.Wins,
                                                 _gameData.Blackjack.Player.Losses,
                                                 _gameData.Blackjack.Player.Balance,
                                                 _gameData.Blackjack.Player.Username,
                                                 _gameData.Blackjack.Player.Imageurl);
            }

            PlayAgain();
        }


        //Get a new card for the player
        private async Task GetCard()
        {
            _gameData.Blackjack.GetNewCard("player");

            _helper.TextToSpeach("You Drew a " +
            _gameData.Blackjack.Player.CurrentHand.Last().Value.ToString(),
            TextToSpeach
            );

           _helper.HapticHelper(_gameData.Settings.HapticFeedback);

            if (_gameData.Blackjack.Player.Currenthandvalue > 21)
            {
                await Task.Delay(2000);

                _helper.TextToSpeach("You have gone bust loser", TextToSpeach);

                _popUpHelper.ShowPopUp("You have gone bust loser", _gameData.Settings.TextToSpeech);

                PlayAgain();
            }
        }

        //Start a new game and get a new hand
        async void PlayAgain()
        {
            await Shell.Current.GoToAsync(nameof(BetPage));
        }

        //Empty hand of cards
        void ClearHand()
        {
            _gameData.Blackjack.Player.ClearHand();
            _gameData.Blackjack.Dealer.ClearHand();
        }

        //Show blackjack  Rules
        private void BlackJackRules() 
        {
            _popUpHelper.ShowPopUp("Rules:\r\n" +
            "\n" +
            " Normal Mode:\r\n" +
            "\n" +
            "  -Closes to 21 whilst being below 22 wins. \r\n" +
            "\n" +
            "  -Dealer must hit until 17.\r\n" +
            "\n" +
            " Hard Mode:\r\n" +
            "\n" +
            "  -Dealer will play more akin to a player. \r\n",
            _gameData.Settings.TextToSpeech
            );
        }
        //Get the initial hand for the player and the dealer
        private async Task GetHand()
        {
            _gameData.Blackjack.Initialhand();

            await VibrateBasedOnCount();

            _helper.TextToSpeach("The Dealer has " +
            _gameData.Blackjack.Dealer.CurrentHand.First().Value,
            TextToSpeach
            );

            _helper.TextToSpeach("You have " +
            _gameData.Blackjack.Player.CurrentHand.First().Value +
            " and a " +
            _gameData.Blackjack.Player.CurrentHand.Last().Value,
            TextToSpeach
            );
        }
        //Vibrates based on the current hand value
        private async Task VibrateBasedOnCount()
        {
            var repetitions = _gameData.Blackjack.Player.Currenthandvalue;
            for(var a = 0; a<= repetitions; a++)
            {
                _helper.VibrationHelper(500, _gameData.Settings.HapticFeedback);

                await Task.Delay(500);
            }
        }
    }
}

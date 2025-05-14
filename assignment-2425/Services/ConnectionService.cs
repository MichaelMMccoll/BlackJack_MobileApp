
using assignment_2425.Helpers;
using assignment_2425.ViewModels;

namespace assignment_2425.Services
{
    //Ref from official .maui connectivity service
    public class ConnectivityService
    {
        private readonly PopUpHelper _popUpHelper = new ();
        private readonly NavigationHelper _navigationHelper = new ();
        //Check internet connection
        public async void CheckForWifi(GameData gameData)
        {
            if (string.IsNullOrEmpty(gameData.Blackjack.Player.Idtoken))
            {
                return;
            }

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _popUpHelper.ShowPopUp("Internet Connection lost \n" +
                                       "User has been logged out", false);
                gameData.Blackjack = new BlackJack();
                await _navigationHelper.Navigate("Main",gameData.Settings.TextToSpeech);
            }
        }

        public bool CheckWifiStatus()
        {
            return Connectivity.NetworkAccess != NetworkAccess.Internet;
        }
    }
}

using assignment_2425.Views;

namespace assignment_2425.Helpers
{
    public class NavigationHelper
    {

        private readonly AccessabilityHelper _accessabilityHelper = new();
        //Determine where user want to go
        public async Task Navigate(string value, bool isEnabled)
        {
            switch (value)
            {
                case "Login":
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                    _accessabilityHelper.TextToSpeach("Going to Log In page", isEnabled);
                    break;
                case "Main":
                    await Shell.Current.GoToAsync(nameof(MainPage));
                    _accessabilityHelper.TextToSpeach("Going to Main page", isEnabled);
                    break;
                case "Signup":
                    await Shell.Current.GoToAsync(nameof(SignUpPage));
                    _accessabilityHelper.TextToSpeach("Going to Sign Up page", isEnabled);
                    break;
                case "Settings":
                    await Shell.Current.GoToAsync(nameof(SettingPage));
                    _accessabilityHelper.TextToSpeach("Going to Settings page", isEnabled);
                    break;
                case "LeaderBoard":
                    await Shell.Current.GoToAsync(nameof(LeaderBoardPage));
                    _accessabilityHelper.TextToSpeach("Going to Leaderboard page", isEnabled);
                    break;
                case "Bet":
                    _accessabilityHelper.TextToSpeach("Going to Bet page", isEnabled);
                    await Shell.Current.GoToAsync(nameof(BetPage));
                    break;
                case "Confirmation":
                    await Shell.Current.GoToAsync(nameof(ConfirmationCodePage));
                    _accessabilityHelper.TextToSpeach("Going to Confirmation code page", isEnabled);
                    break;
                case "Walkthrought":
                    await Shell.Current.GoToAsync(nameof(InstructionsPage));
                    _accessabilityHelper.TextToSpeach("Going to Instructions page", isEnabled);
                    break;
                default:
                    break;
            }
        }
    }
}

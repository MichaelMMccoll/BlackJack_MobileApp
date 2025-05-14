using assignment_2425.Views;

namespace assignment_2425
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(BackJackPage), typeof(BackJackPage));
            Routing.RegisterRoute(nameof(BetPage), typeof(BetPage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(ConfirmationCodePage), typeof(ConfirmationCodePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LeaderBoardPage), typeof(LeaderBoardPage));
            Routing.RegisterRoute(nameof(InstructionsPage), typeof(InstructionsPage));
        }
    }
}

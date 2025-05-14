using assignment_2425.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;

namespace assignment_2425.Views;

public partial class LeaderBoardPage : ContentPage
{
    private readonly LeaderBoardViewModel _viewModel;
    public LeaderBoardPage(GameData gameData,
                           IConfiguration configuration)
	{
		InitializeComponent();
        _viewModel = new LeaderBoardViewModel(gameData,configuration);
        BindingContext = _viewModel;
    }
}
using assignment_2425.DataBase;
using assignment_2425.Models;
using assignment_2425.ViewModels;
using Microsoft.Maui.Layouts;
using System.Collections.Specialized;

namespace assignment_2425.Views;

public partial class MainPage : ContentPage
{
    private readonly GameData _gameData;
    private readonly System.Timers.Timer _imageTapTimer = new();
    private readonly BlackJackDatabase _dataBase;
    private readonly Random random = new Random();

    public MainPage(GameData gameData,
                    BlackJackDatabase dataBase,
                    Settings settings)
    {
        InitializeComponent();
        _gameData = gameData;
        _dataBase = dataBase;
        _gameData.Settings = settings;
        BindingContext = new MainPageViewModel(gameData, dataBase, settings);
        this.Loaded += OnPageLoad;
    }

    //Change input box size when based on tablet
    private void OnPageLoad(object sender, EventArgs e)
    {
        ((MainPageViewModel)BindingContext).UpdateSize(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
    }

    //Ensuresg falling cards start when screen reached
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((MainPageViewModel)BindingContext).FallingImages.CollectionChanged += OnFallingImageAdded;

    }

    //Adds cards to thefalling animation
    private void OnFallingImageAdded(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (string source in e.NewItems)
            {
                StartFallingAnimation(source);
            }
        }
    }

    //Starts falling animation
    private async void StartFallingAnimation(string imageLink)
    {
        var image = new Image
        {
            Source = imageLink,
            WidthRequest = 60,
            HeightRequest = 60
        };

        double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

        double startX = random.Next(0, (int)(screenWidth - 60));

        AbsoluteLayout.SetLayoutBounds(image, new Rect(startX, -100, 60, 60));
        AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.None);

        var gesture = new TapGestureRecognizer();
        gesture.Tapped += (s, e) =>
        {
            ((MainPageViewModel)BindingContext).OnImagePressed();
            absoluteLayout.Children.Remove(image);
        };
        image.GestureRecognizers.Add(gesture);
        absoluteLayout.Children.Add(image);

        // Start spin animation
        _ = image.RotateTo(30000, 1000000, Easing.Linear);
        // Fall down Y axis
        await image.TranslateTo(startX, screenHeight + 100, 15000);

        absoluteLayout.Children.Remove(image);
    }
}

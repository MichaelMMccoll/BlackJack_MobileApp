using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using assignment_2425.ViewModels;
using assignment_2425.Views;
using Microsoft.Extensions.Logging;
using assignment_2425.DataBase;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using assignment_2425.Models;
using assignment_2425.Services;

namespace assignment_2425
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            var awsOptions = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1 // Change region if needed
            };
            var awsCredentials = new BasicAWSCredentials("YOUR_ACCESS_KEY", "YOUR_SECRET_KEY");
            // Register AWS services
            builder.Services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(awsCredentials, awsOptions));
            builder.Services.AddSingleton<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient(awsCredentials, RegionEndpoint.USEast1));
            builder.Services.AddSingleton<DynamoDBContext>();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
            //Add custom fonts
            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("gothamrnd_medium.otf", "GothamRounded");
            });

            //Reading from appsettings.json
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("assignment_2425.appsettings.json");

            if(stream == null)
            {
                throw new System.Exception("appsettings.json not found");
            }
            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);

            builder.Services.AddSingleton<ConnectivityService>();
            builder.Services.AddSingleton<GameData>();
            builder.Services.AddSingleton<UserData>();
            builder.Services.AddSingleton<BlackJackDatabase>();
            builder.Services.AddSingleton<Settings>();
            builder.Services.AddSingleton<SmartPlayer>();
            builder.Services.AddTransient<BackJackPage>();
            builder.Services.AddTransient<BetPage>();
            builder.Services.AddTransient<SettingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<ConfirmationCodePage>();
            builder.Services.AddTransient<LeaderBoardPage>();
            builder.Services.AddTransient<InstructionsPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
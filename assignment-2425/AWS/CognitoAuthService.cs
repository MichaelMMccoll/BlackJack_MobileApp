using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using assignment_2425.AWS;
using assignment_2425.Models;
using assignment_2425.Helpers;

namespace assignment_2425.ViewModels
{
    public class CognitoAuthService
    {
        private static readonly RegionEndpoint region = RegionEndpoint.EUNorth1;
        IConfiguration _configuration;
        private AwsSettings _awsSettings;
        private readonly CognitoAWSCredentials credentials;
        private readonly AmazonDynamoDBClient dynamoDBClient;
        private readonly PopUpHelper _popUpHelper = new();

        public CognitoAuthService(string id,
                                  IConfiguration configuration)
        {
            _configuration = configuration;
            _awsSettings = _configuration.GetRequiredSection("AwsSettings").Get<AwsSettings>() ?? new AwsSettings();

            credentials = new CognitoAWSCredentials(_awsSettings.IdentityPoolId, region);
            credentials.AddLogin(_awsSettings.CognitoFamily,
                                  id);

            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = region
            };

            dynamoDBClient = new AmazonDynamoDBClient(credentials, config);
        }

        // Upload Image to S3 and Return URL
        public async Task<string> UploadImageAsync(string fileName, string filePath)
        {

            using var s3Client = new AmazonS3Client(credentials, region);
            var tranferUtility = new Amazon.S3.Transfer.TransferUtility(s3Client);

            try
            {
                await tranferUtility.UploadAsync(filePath, _awsSettings.S3BucketName, fileName);
                Console.WriteLine("Uploaded image to S3");

                string fileUrl = $"https://mobile-app-card-game.s3.amazonaws.com/{fileName}";
                return fileUrl;

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error with S3: " + e.Message);
                _popUpHelper.ShowPopUp("Error uploading image", false);
                return string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.Message);
                _popUpHelper.ShowPopUp("Error uploading image", false);
                return string.Empty;
            }
        }

        // Save User Data to DynamoDB
        public async Task SaveUserDataAsync(string userId,
                                            int wins,
                                            int losses,
                                            int balance,
                                            string userName,
                                            string imageUrl)
        {

            var list = await dynamoDBClient.ListTablesAsync();
            // Load the table
            var request = new PutItemRequest
            {
                TableName = list.TableNames.First(),
                Item = new Dictionary<string, AttributeValue>
                {
                    { "UserId", new AttributeValue { S = userId } },
                    { "wins", new AttributeValue { N = wins.ToString() } },
                    { "losses", new AttributeValue { N = losses.ToString() } },
                    { "balance", new AttributeValue { N = balance.ToString() } },
                    { "username", new AttributeValue { S = userName} },
                    { "imageurl", new AttributeValue { S = imageUrl ?? ""} }
                }
            };

            try
            {
                await dynamoDBClient.PutItemAsync(request);

            }catch(Exception e){
                Console.WriteLine(e.Message);
                _popUpHelper.ShowPopUp("Error saving user data", false);
            }
        }

        //Gets player data
        public async Task<UserData> GetUserDataAsync(string userId)
        {
            var list = await dynamoDBClient.ListTablesAsync();
            var player = new UserData();

            try
            {
                var pk = userId;
                var key = new Dictionary<string, AttributeValue>
                {
                    ["UserId"] = new AttributeValue { S = pk }
                };

               var data = await dynamoDBClient.GetItemAsync(list.TableNames.First(),key);

               foreach(var a in data.Item)
                {
                    var keyy = a.Key.ToString();
                    switch (keyy)
                    {
                        case "wins":
                            player.Wins = Convert.ToInt32(a.Value.N);
                            break;
                        case "losses":
                            player.Losses = Convert.ToInt32(a.Value.N);
                            break;
                        case "balance":
                            player.Balance = Convert.ToInt32(a.Value.N);
                            break;
                        case "username":
                            player.UserName = a.Value.S;
                            break;
                        case "imageurl":
                            player.ImageUrl = a.Value.S;
                            break;
                    }
               }

                return player;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _popUpHelper.ShowPopUp("Error getting user data", false);
            }
            return player;
        }

        //Calls db to get the leaderboard data
        public async Task<List<LeaderBoardObject>> GetLeaderBoardDataAsync(string userId)
        {
            var aa = await dynamoDBClient.ListTablesAsync();
            var playerList = new List<LeaderBoardObject>();

            try
            {
                var queryData = new List<string>() { "wins","username" };

                var data = await dynamoDBClient.ScanAsync(aa.TableNames.First(), queryData);

                foreach (var a in data.Items)
                {
                    var player = new LeaderBoardObject("",0,0);
                    //Review these linqs will crash if empty
                    player.Wins = Convert.ToInt32(a.Where(x => x.Key.ToString() == "wins").First().Value.N);
                    player.UserName = a.Where(x => x.Key.ToString() == "username").First().Value.S;

                    playerList.Add(player);
                }

                playerList = playerList.OrderBy(x => x.Wins).Take(10).ToList();

                var position = playerList.Count;

                foreach (var item in playerList)
                {
                    item.Postition = position;
                    position--;
                }


                return playerList.OrderBy(x => x.Postition).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _popUpHelper.ShowPopUp("Error getting leaderboard data", false);
            }
            return playerList;
        }
    }
}

using Amazon.DynamoDBv2.DataModel;

namespace assignment_2425.Models
{
    [DynamoDBTable("mobileAppData")]
    public class UserData
    {
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Balance { get; set; } = 2000;
        public string ImageUrl { get; set; } = "";
        public string UserName { get; set; } = "";
    }
}

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using assignment_2425.AWS;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace assignment_2425.ViewModels
{
    public class CognitoService2
    {
        private static readonly RegionEndpoint region = RegionEndpoint.EUNorth1;

        private readonly AmazonCognitoIdentityProviderClient _providerClient;
        private IConfiguration _configuration;
        private AwsSettings _awsSettings;

        public CognitoService2(IConfiguration configuration)
        {
            var awsCredentials = new AnonymousAWSCredentials();
            _configuration = configuration;
            _awsSettings = _configuration.GetRequiredSection("AwsSettings").Get<AwsSettings>() ?? new AwsSettings();
            _providerClient = new AmazonCognitoIdentityProviderClient(awsCredentials, region);
        }

        // Signs in and gets userId
        public async Task<Response> SignInAndGetUserIdAsync(string username, string password)
        {
            try
            {
                var authRequest = new InitiateAuthRequest
                {
                    ClientId = _awsSettings.ClientId,
                    AuthFlow = AuthFlowType.USER_PASSWORD_AUTH
                };
                authRequest.AuthParameters.Add("USERNAME", username);
                authRequest.AuthParameters.Add("PASSWORD", password);

                var authResponse = await _providerClient.InitiateAuthAsync(authRequest);
                string idToken = authResponse.AuthenticationResult.IdToken;

                string userId = ExtractUserIdFromToken(idToken);

                var response = new Response
                {
                    idToken = idToken,
                    userId = userId
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sign-in failed: {ex.Message}");
                return new Response();
            }
        }

        // Get userID
        private string ExtractUserIdFromToken(string idToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(idToken) as JwtSecurityToken;
            var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            return userId;
        }

        //Signs user up
        public async Task<bool> SignUpUserAsync(string email, string password)
        {
            try
            {
                var signUpRequest = new SignUpRequest
                {
                    ClientId = _awsSettings.ClientId,
                    Username = email,  // Use email as the username
                    Password = password,
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType { Name = "email", Value = email }
                    }
                };

                var signUpResponse = await _providerClient.SignUpAsync(signUpRequest);

                Console.WriteLine("Sign-up successful. Please check your email for a verification code.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sign-up failed: {ex.Message}");
                return false;
            }
        }

        // Confirms email code
        public async Task<bool> ConfirmSignUpAsync(string email, string confirmationCode)
        {
            try
            {
                var confirmSignUpRequest = new ConfirmSignUpRequest
                {
                    ClientId = _awsSettings.ClientId,
                    Username = email,
                    ConfirmationCode = confirmationCode
                };

                var confirmSignUpResponse = await _providerClient.ConfirmSignUpAsync(confirmSignUpRequest);

                Console.WriteLine("Email verification successful. You can now sign in.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email verification failed: {ex.Message}");
                return false;
            }
        }
    }
    //Response Object
    public class Response()
    {
        public string idToken { get; set; } = "";
        public string userId { get; set; } = "";
    }
}

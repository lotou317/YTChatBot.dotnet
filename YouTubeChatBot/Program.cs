using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.IO;
using System.Threading;

class Program
{
  static async Task Main()
  {
    UserCredential credential;
    using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
    {
      credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
          GoogleClientSecrets.FromStream(stream).Secrets,
          new[] { YouTubeService.Scope.YoutubeForceSsl },
          "user",
          CancellationToken.None
      );
    }

    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
    {
      HttpClientInitializer = credential,
      ApplicationName = "YouTube Chat Bot",
    });

    Console.WriteLine("✅ Authenticated successfully!");
  }
}

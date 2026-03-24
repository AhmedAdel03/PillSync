using System.Text;
using System.Text.Json;

public class TurboSmtpService(HttpClient httpClient,IConfiguration config)
{
 private string ApiUrl = config["TurboSetting:SendServerUrl"];
  private string consumerKey = config["TurboSetting:Consumer-Key"];
 private string consumerSecret = config["TurboSetting:Consumer-Secret"];


    public async Task SendEmailAsync(string to, string code)
    {
        var payload = new {
            from = "pillsync.com",
            to = to,
            subject = "Verify Code",
            content = code
        };

        var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
        // Put your keys here
        request.Headers.Add("consumerKey", consumerKey);
        request.Headers.Add("consumerSecret", consumerSecret);
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        await httpClient.SendAsync(request);
    }
}
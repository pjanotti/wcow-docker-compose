using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ForwardController : ControllerBase
{
    [HttpGet(Name = "GetForward")]
    public async Task<string> Get()
    {
        var forwardServer = Environment.GetEnvironmentVariable("FORWARD_SERVER");
        if (forwardServer is null)
        {
            return $"You reached the end of the chain at {Environment.MachineName}!";
        }

        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(5);
        HttpResponseMessage? resp = null;
        try
        {
            resp = await httpClient.GetAsync($"http://{forwardServer}/forward");
        }
        catch (Exception e)
        {
            return $"Exception [{e.Message}] trying to reach {forwardServer} from {Environment.MachineName}";
        }

        if (!resp.IsSuccessStatusCode)
        {
            return $"Failed to reach {resp.RequestMessage?.RequestUri} from {Environment.MachineName}";
        }

        var receivedMessage = (await resp.Content.ReadAsStringAsync()) ?? $"<empty_msg_from:{forwardServer}>";
        return $"Received message [{receivedMessage}] at {Environment.MachineName}";
    }
}

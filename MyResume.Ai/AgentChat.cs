using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MyResume.Ai.Manager;
using MyResume.Model.AiModels;
using Newtonsoft.Json;

namespace MyResume.Ai;

public class AgentChat
{
    private readonly ILogger<AgentChat> _logger;

    public AgentChat(ILogger<AgentChat> logger)
    {
        _logger = logger;
    }

    [Function("AgentChat")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger 'AgentChat' function is called.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var input = JsonConvert.DeserializeObject<AiRequestObject>(requestBody);

        var agentManager = new AgentManager();
        var reply = await agentManager.GetResponse(input!.Promt!);

        var response = new AiResponseObject
        {
            Reply = reply,
            TreadId = null
        };

        _logger.LogInformation("C# HTTP trigger 'AgentChat' function processed a request.");
        return new OkObjectResult(response);
    }
}
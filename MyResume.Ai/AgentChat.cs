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
    private readonly IAgenticManager _agentManager;

    public AgentChat(IAgenticManager manager, ILogger<AgentChat> logger)
    {
        _logger = logger;
        _agentManager = manager;
    }

    [Function("AgentChat")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        try
        {
            _logger.LogInformation("C# HTTP trigger 'AgentChat' function is called.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var reply = await _agentManager.GetResponse(requestBody);

            _logger.LogInformation("C# HTTP trigger 'AgentChat' function processed a request.");
            return new OkObjectResult(reply);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new BadRequestObjectResult(ex.ToString());
        }
    }
}
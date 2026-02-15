using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MyResume.Ai;

public class AgentChat
{
    private readonly ILogger<AgentChat> _logger;

    public AgentChat(ILogger<AgentChat> logger)
    {
        _logger = logger;
    }

    [Function("AgentChat")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
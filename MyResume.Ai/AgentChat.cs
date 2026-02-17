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
    private readonly IAgentManager _agentManager;

    public AgentChat(IAgentManager manager, ILogger<AgentChat> logger)
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
            var input = JsonConvert.DeserializeObject<AiRequestObject>(requestBody);

            var reply = await _agentManager.GetResponse(input!.Promt!);

            var response = new AiResponseObject
            {
                Reply = reply,
                TreadId = null
            };

            _logger.LogInformation("C# HTTP trigger 'AgentChat' function processed a request.");
            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new BadRequestObjectResult(ex.ToString());
        }
    }
}
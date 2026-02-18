using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MyResume.Api.Manager;
using MyResume.Model.ApiModel;
using Newtonsoft.Json;

namespace MyResume.Api;

public class ChatData
{
    private readonly ILogger<ChatData> _logger;
    private readonly IChatDataManager _manager;

    public ChatData(ILogger<ChatData> logger
        , IChatDataManager manager)
    {
        _logger = logger;
        _manager = manager;
    }

    [Function("SaveChatData")]
    public async Task<IActionResult> SaveChatData([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function 'SaveChatData' is called.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var loginRequestData = JsonConvert.DeserializeObject<UserLogChatRequestData>(requestBody);

        if (loginRequestData == null)
        {
            return new ObjectResult(new UserLogResponseData()
            {
                Message = "Invalid login request data.",
                IsSuccessful = false
            })
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        string ip = req.Headers.TryGetValue("X-Forwarded-For", out var values)
            ? values!.FirstOrDefault()?.Split(',')[0]!
            : req.HttpContext?.Connection?.RemoteIpAddress?.ToString()!;
        loginRequestData.IpAddress = ip;

        UserLogChatResponseData responseData = await _manager.SaveChatData(loginRequestData);

        _logger.LogInformation("C# HTTP trigger function 'SaveChatData' processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
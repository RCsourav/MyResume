using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MyResume.Api.Manager;
using MyResume.Model.ApiModel;
using Newtonsoft.Json;

namespace MyResume.Api;

public class LogActivity
{
    private readonly ILogger<LogActivity> _logger;
    private readonly ILogActivityManager _manager;

    public LogActivity(ILogger<LogActivity> logger
        , ILogActivityManager manager)
    {
        _logger = logger;
        _manager = manager;
    }

    [Function("LogOn")]
    public async Task<IActionResult> LogOn([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function 'LogOn' is called.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var loginRequestData = JsonConvert.DeserializeObject<UserLogRequestData>(requestBody);

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

        UserLogResponseData responseData = await _manager.LogOnAsync(loginRequestData);

        _logger.LogInformation("C# HTTP trigger function 'LogOn' processed a request.");
        return new OkObjectResult(responseData);
    }

    [Function("LogOff")]
    public async Task<IActionResult> LogOff([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function 'LogOff' is called.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var loginRequestData = JsonConvert.DeserializeObject<UserLogRequestData>(requestBody);
        
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

        UserLogResponseData responseData = await _manager.LogOffAsync(loginRequestData);

        _logger.LogInformation("C# HTTP trigger function 'LogOff' processed a request.");
        return new OkObjectResult(responseData);
    }

    [Function("UpdatedActivity")]
    public async Task<IActionResult> UpdatedActivity([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function 'UpdatedActivity' is called.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var loginRequestData = JsonConvert.DeserializeObject<UserLogRequestData>(requestBody);

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

        UserLogResponseData responseData = await _manager.UpdatedActivityAsync(loginRequestData);

        _logger.LogInformation("C# HTTP trigger function 'UpdatedActivity' processed a request.");
        return new OkObjectResult(responseData);
    }

    [Function("GetActiveSession")]
    public async Task<IActionResult> GetActiveSession([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function 'GetActiveSession' is called.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var loginRequestData = JsonConvert.DeserializeObject<UserLogRequestData>(requestBody);

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

        UserLogResponseData responseData = await _manager.GetActiveSessionAsync(loginRequestData);

        _logger.LogInformation("C# HTTP trigger function 'GetActiveSession' processed a request.");
        return new OkObjectResult(responseData);
    }
}
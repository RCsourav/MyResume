using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MyResume.Api.Manager;
using System;

namespace MyResume.Api;

public class CheckSession
{
    private readonly ILogger<CheckSession> _logger;
    private readonly ILogActivityManager _manager;

    public CheckSession(ILogger<CheckSession> logger
        , ILogActivityManager manager)
    {
        _logger = logger;
        _manager = manager;
    }

    [Function("LogOffSession")]
    public void LogOffSession([TimerTrigger("%LogOffSessionTimer%")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function 'LogOffSession' is called at: {executionTime}", DateTime.Now);

        _manager.LogOff();

        _logger.LogInformation("C# Timer trigger function 'LogOffSession' executed at: {executionTime}", DateTime.Now);

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}
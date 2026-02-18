using Microsoft.Extensions.Logging;
using MyResume.Api.Repo.Repo;
using MyResume.Model.ApiModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Api.Manager
{
    public interface ILogActivityManager
    {
        Task<UserLogResponseData> GetActiveSessionAsync(UserLogRequestData loginRequestData);
        void LogOff();
        Task<UserLogResponseData> LogOffAsync(UserLogRequestData loginRequestData);
        Task<UserLogResponseData> LogOnAsync(UserLogRequestData loginRequestData);
        Task<UserLogResponseData> UpdatedActivityAsync(UserLogRequestData loginRequestData);
    }
    public class LogActivityManager : ILogActivityManager
    {
        private readonly ILogger<LogActivityManager> _logger;
        private readonly ILogActivityRepo _repo;

        public LogActivityManager(ILogger<LogActivityManager> logger
            , ILogActivityRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<UserLogResponseData> GetActiveSessionAsync(UserLogRequestData loginRequestData)
        {
            _logger.LogInformation("C# LogActivityManager 'GetActiveSessionAsync' method is called.");

            if (String.IsNullOrEmpty(loginRequestData.IpAddress))
            {
                return new UserLogResponseData()
                {
                    Message = "IP address is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.Name))
            {
                return new UserLogResponseData()
                {
                    Message = "User name is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.EmaiId))
            {
                return new UserLogResponseData()
                {
                    Message = "Email ID is required.",
                    IsSuccessful = false
                };
            }

            UserLogResponseData response = await _repo.GetActiveSessionAsync(loginRequestData);

            _logger.LogInformation(response.Message);

            _logger.LogInformation("C# LogActivityManager 'GetActiveSessionAsync' method is successful.");

            return response;
        }

        public void LogOff()
        {
            _repo.LogOff();
        }

        public async Task<UserLogResponseData> LogOffAsync(UserLogRequestData loginRequestData)
        {
            _logger.LogInformation("C# LogActivityManager 'LogOffAsync' method is called.");

            if (String.IsNullOrEmpty(loginRequestData.IpAddress))
            {
                return new UserLogResponseData()
                {
                    Message = "IP address is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.Name))
            {
                return new UserLogResponseData()
                {
                    Message = "User name is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.EmaiId))
            {
                return new UserLogResponseData()
                {
                    Message = "Email ID is required.",
                    IsSuccessful = false
                };
            }

            UserLogResponseData response = await _repo.LogOffAsync(loginRequestData);

            _logger.LogInformation(response.Message);

            _logger.LogInformation("C# LogActivityManager 'LogOffAsync' method is successful.");

            return response;
        }

        public async Task<UserLogResponseData> LogOnAsync(UserLogRequestData loginRequestData)
        {
            _logger.LogInformation("C# LogActivityManager 'LogOnAsync' method is called.");

            if (String.IsNullOrEmpty(loginRequestData.IpAddress))
            {
                return new UserLogResponseData()
                {
                    Message = "IP address is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.Name))
            {
                return new UserLogResponseData()
                {
                    Message = "User name is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.EmaiId))
            {
                return new UserLogResponseData()
                {
                    Message = "Email ID is required.",
                    IsSuccessful = false
                };
            }


            UserLogResponseData response = await _repo.LogOnAsync(loginRequestData);

            _logger.LogInformation(response.Message);

            _logger.LogInformation("C# LogActivityManager 'LogOnAsync' method is successful.");

            return response;
        }

        public async Task<UserLogResponseData> UpdatedActivityAsync(UserLogRequestData loginRequestData)
        {
            _logger.LogInformation("C# LogActivityManager 'UpdatedActivityAsync' method is called.");

            if (String.IsNullOrEmpty(loginRequestData.IpAddress))
            {
                return new UserLogResponseData()
                {
                    Message = "IP address is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.Name))
            {
                return new UserLogResponseData()
                {
                    Message = "User name is required.",
                    IsSuccessful = false
                };
            }

            if (String.IsNullOrEmpty(loginRequestData.EmaiId))
            {
                return new UserLogResponseData()
                {
                    Message = "Email ID is required.",
                    IsSuccessful = false
                };
            }

            UserLogResponseData response = await _repo.UpdatedActivityAsync(loginRequestData);

            _logger.LogInformation(response.Message);

            _logger.LogInformation("C# LogActivityManager 'UpdatedActivityAsync' method is successful.");

            return response;
        }
    }
}

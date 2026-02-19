using Microsoft.Extensions.Logging;
using MyResume.Api.Repo.Repo;
using MyResume.Model.ApiModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Api.Manager
{
    public interface IChatDataManager
    {
        Task<UserLogChatResponseData> SaveChatData(UserLogChatRequestData chatRequestData);
    }
    public class ChatDataManager: IChatDataManager
    {
        private readonly ILogger<ChatDataManager> _logger;
        private readonly IChatDataRepo _repo;

        public ChatDataManager(ILogger<ChatDataManager> logger
            , IChatDataRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<UserLogChatResponseData> SaveChatData(UserLogChatRequestData chatRequestData)
        {
            _logger.LogInformation("C# ChatDataManager 'SaveChatData' method is called.");

            try
            {
                if (String.IsNullOrEmpty(chatRequestData.IpAddress))
                {
                    return new UserLogChatResponseData()
                    {
                        Message = "IP address is required.",
                        IsSuccessful = false
                    };
                }

                if (String.IsNullOrEmpty(chatRequestData.Name))
                {
                    return new UserLogChatResponseData()
                    {
                        Message = "User name is required.",
                        IsSuccessful = false
                    };
                }

                if (String.IsNullOrEmpty(chatRequestData.EmailId))
                {
                    return new UserLogChatResponseData()
                    {
                        Message = "Email ID is required.",
                        IsSuccessful = false
                    };
                }

                if (String.IsNullOrEmpty(chatRequestData.UserRequestPromt))
                {
                    return new UserLogChatResponseData()
                    {
                        Message = "User request prompt is required.",
                        IsSuccessful = false
                    };
                }

                if (String.IsNullOrEmpty(chatRequestData.AiResponseMessage))
                {
                    return new UserLogChatResponseData()
                    {
                        Message = "AI response message is required.",
                        IsSuccessful = false
                    };
                }

                UserLogChatResponseData repoResult = await _repo.SaveChatData(chatRequestData);

                _logger.LogInformation("C# ChatDataManager 'SaveChatData' method is successful.");
                return repoResult;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred in 'SaveChatData' method.");

                return new UserLogChatResponseData()
                {
                    Message = ex.ToString(),
                    IsSuccessful = false
                };
            }
        }
    }
}

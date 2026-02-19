using Microsoft.Extensions.Logging;
using MyResume.Api.Repo.Db.Context;
using MyResume.Model.ApiModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Api.Repo.Repo
{
    public interface IChatDataRepo
    {
        Task<UserLogChatResponseData> SaveChatData(UserLogChatRequestData chatRequestData);
    }
    public class ChatDataRepo : IChatDataRepo
    {
        private readonly ILogger<ChatDataRepo> _logger;
        private readonly MyResumeContext _dbContext;

        public ChatDataRepo(ILogger<ChatDataRepo> logger
            , MyResumeContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<UserLogChatResponseData> SaveChatData(UserLogChatRequestData chatRequestData)
        {
            try
            {
                var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var estTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone);

                UserLogChatResponseData responseData = new UserLogChatResponseData
                {
                    UserRequestPromt = chatRequestData.UserRequestPromt,
                    AiResponseMessage = chatRequestData.AiResponseMessage,
                    EmailId = chatRequestData.EmailId,
                    Name = chatRequestData.Name,
                    IpAddress = chatRequestData.IpAddress,
                    LoginId = chatRequestData.LoginId,
                };

                int ipDataId = 0;
                int userDataId = 0;

                var ipData = _dbContext.UserIpAddresses.Where(x => x.IpAddress == chatRequestData.IpAddress).FirstOrDefault();
                var userData = _dbContext.UserDetails.Where(x => x.EmailId == chatRequestData.EmailId
                && x.UserName == chatRequestData.Name).FirstOrDefault();

                if (ipData == null)
                {
                    responseData.IsSuccessful = false;
                    responseData.Message = "No active session found for the user.";
                    responseData.ReturnCode = 0;
                    return responseData;

                }
                else
                {
                    ipDataId = ipData.Id;
                }

                if (userData == null)
                {
                    responseData.IsSuccessful = false;
                    responseData.Message = "No active session found for the user.";
                    responseData.ReturnCode = 0;
                    return responseData;
                }
                else
                {
                    userDataId = userData.Id;
                }

                var lastActiveSession = _dbContext.UserLoginDetails.Where(x => x.IpAddressId == ipDataId
                && x.UserId == userDataId && x.IsActive && x.LogoutTime == null).FirstOrDefault();

                if (lastActiveSession == null)
                {
                    responseData.IsSuccessful = false;
                    responseData.Message = "No active session found for the user. Please login to continue.";
                    responseData.ReturnCode = 0;
                    return responseData;
                }
                else
                {
                    var loginHistoryData = _dbContext.UserLoginHistories.Where(x => x.Id == lastActiveSession.UserHistoryId).FirstOrDefault();
                    if (loginHistoryData != null)
                    {
                        loginHistoryData.LastActivityTime = estTime;
                        _dbContext.UserLoginHistories.Update(loginHistoryData);
                        _dbContext.SaveChanges();

                        var newChatData = new Db.Entity.LoginChatHistory()
                        {
                            UserLoginHistoryId = lastActiveSession.UserHistoryId,
                            RequestMessage = chatRequestData.UserRequestPromt!,
                            ResponseMessage = chatRequestData.AiResponseMessage!,
                            CreatedAt = estTime
                        };

                        _dbContext.LoginChatHistories.Add(newChatData);
                        await _dbContext.SaveChangesAsync();
                        responseData.IsSuccessful = true;
                        responseData.Message = "Chat data saved successfully.";
                        responseData.ReturnCode = 1;
                    }
                    else
                    {
                        responseData.IsSuccessful = false;
                        responseData.Message = "No active session found for the user.";
                        responseData.ReturnCode = 0;
                    }
                }

                return responseData;
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

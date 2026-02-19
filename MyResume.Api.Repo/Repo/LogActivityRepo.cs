using Microsoft.Extensions.Logging;
using MyResume.Api.Repo.Db.Context;
using MyResume.Model.ApiModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Api.Repo.Repo
{
    public interface ILogActivityRepo
    {
        Task<UserLogResponseData> GetActiveSessionAsync(UserLogRequestData loginRequestData);
        void LogOff();
        Task<UserLogResponseData> LogOffAsync(UserLogRequestData loginRequestData);
        Task<UserLogResponseData> LogOnAsync(UserLogRequestData loginRequestData);
        Task<UserLogResponseData> UpdatedActivityAsync(UserLogRequestData loginRequestData);
    }
    public class LogActivityRepo : ILogActivityRepo
    {
        private readonly ILogger<LogActivityRepo> _logger;
        private readonly MyResumeContext _dbContext;

        public LogActivityRepo(ILogger<LogActivityRepo> logger
            , MyResumeContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<UserLogResponseData> GetActiveSessionAsync(UserLogRequestData loginRequestData)
        {
            try
            {
                var returnData = new UserLogResponseData()
                {
                    EmailId = loginRequestData.EmailId,
                    Name = loginRequestData.Name,
                    IpAddress = loginRequestData.IpAddress,
                    LoginId = loginRequestData.LoginId,
                };

                int ipDataId = 0;
                int userDataId = 0;
                var ipData = _dbContext.UserIpAddresses.Where(x => x.IpAddress == loginRequestData.IpAddress).FirstOrDefault();
                var userData = _dbContext.UserDetails.Where(x => x.EmailId == loginRequestData.EmailId
                && x.UserName == loginRequestData.Name).FirstOrDefault();

                if (ipData == null)
                {
                    returnData.IsSuccessful = false;
                    returnData.Message = "No active session found for the user.";
                    returnData.ReturnCode = 0;
                    return returnData;

                }
                else
                {
                    ipDataId = ipData.Id;
                }

                if (userData == null)
                {
                    returnData.IsSuccessful = false;
                    returnData.Message = "No active session found for the user.";
                    returnData.ReturnCode = 0;
                    return returnData;
                }
                else
                {
                    userDataId = userData.Id;
                }

                var lastActiveSession = _dbContext.UserLoginDetails.Where(x => x.IpAddressId == ipDataId
                && x.UserId == userDataId && x.IsActive && x.LogoutTime == null).FirstOrDefault();
                if (lastActiveSession != null && lastActiveSession.UserHistoryId == loginRequestData.LoginId)
                {
                    returnData.LoggedOnTime = lastActiveSession.LoginTime;
                    returnData.IsActive = lastActiveSession.IsActive;
                    returnData.LastActivityTime = lastActiveSession.LastActivityTime;
                    returnData.IsSuccessful = true;
                    returnData.Message = "Active session found for the user.";
                    returnData.LoginId = lastActiveSession.UserHistoryId;
                    returnData.ReturnCode = 1;
                }
                else
                {
                    returnData.IsSuccessful = false;
                    returnData.Message = "No active session found for the user.";
                    returnData.ReturnCode = 0;
                }
                return returnData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in 'GetActiveSessionAsync' method.");

                return new UserLogResponseData()
                {
                    Message = ex.ToString(),
                    IsSuccessful = false
                };
            }
        }

        public void LogOff()
        {
            try
            {
                var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var estTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone);
                var lastActivityThreshold = estTime.AddMinutes(-30);

                var activeSessions = _dbContext.UserLoginDetails.Where(x => x.IsActive && x.LogoutTime == null
                && x.LastActivityTime <= lastActivityThreshold).ToList();

                foreach (var session in activeSessions)
                {
                    var loginHistoryData = _dbContext.UserLoginHistories.Where(x => x.Id == session.UserHistoryId).FirstOrDefault();
                    if (loginHistoryData != null)
                    {
                        loginHistoryData.LogoutTime = DateTime.UtcNow;
                        loginHistoryData.IsActive = false;
                        _dbContext.UserLoginHistories.Update(loginHistoryData);
                    }
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in 'LogOff' method.");
            }
        }

        public async Task<UserLogResponseData> LogOffAsync(UserLogRequestData loginRequestData)
        {
            try
            {
                var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var estTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone);

                var returnData = new UserLogResponseData()
                {
                    EmailId = loginRequestData.EmailId,
                    Name = loginRequestData.Name,
                    IpAddress = loginRequestData.IpAddress,
                    LoginId = loginRequestData.LoginId,
                };

                int ipDataId = 0;
                int userDataId = 0;

                var ipData = _dbContext.UserIpAddresses.Where(x => x.IpAddress == loginRequestData.IpAddress).FirstOrDefault();
                var userData = _dbContext.UserDetails.Where(x => x.EmailId == loginRequestData.EmailId
                && x.UserName == loginRequestData.Name).FirstOrDefault();

                if (ipData == null || userData == null)
                {
                    returnData.IsSuccessful = false;
                    returnData.Message = "No active session found for the user.";
                    returnData.ReturnCode = 0;
                }
                else
                {
                    ipDataId = ipData.Id;
                    userDataId = userData.Id;
                    var lastActiveSession = _dbContext.UserLoginDetails.Where(x => x.IpAddressId == ipDataId
                    && x.UserId == userDataId && x.IsActive && x.LogoutTime == null).FirstOrDefault();
                    if (lastActiveSession != null)
                    {
                        if (lastActiveSession.UserHistoryId != loginRequestData.LoginId)
                        {
                            returnData.IsSuccessful = false;
                            returnData.Message = "The provided LoginId does not match the active session.";
                            returnData.ReturnCode = -1;
                            return returnData;
                        }
                        else
                        {
                            var loginHistoryData = _dbContext.UserLoginHistories.Where(x => x.Id == lastActiveSession.UserHistoryId).FirstOrDefault();
                            if (loginHistoryData != null)
                            {
                                loginHistoryData.LogoutTime = estTime;
                                loginHistoryData.IsActive = false;
                                _dbContext.UserLoginHistories.Update(loginHistoryData);
                                _dbContext.SaveChanges();

                                returnData.IsSuccessful = true;
                                returnData.Message = "Logout successful.";
                                returnData.ReturnCode = 1;
                            }
                            else
                            {
                                returnData.IsSuccessful = false;
                                returnData.Message = "No active session found for the user.";
                                returnData.ReturnCode = 0;
                            }
                        }
                    }
                    else
                    {
                        returnData.IsSuccessful = false;
                        returnData.Message = "No active session found for the user.";
                        returnData.ReturnCode = 0;
                    }
                }

                return returnData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in 'LogOffAsync' method.");

                return new UserLogResponseData()
                {
                    Message = ex.ToString(),
                    IsSuccessful = false
                };
            }
        }

        public async Task<UserLogResponseData> LogOnAsync(UserLogRequestData loginRequestData)
        {
            try
            {
                var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var estTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone);

                var returnData = new UserLogResponseData()
                {
                    EmailId = loginRequestData.EmailId,
                    Name = loginRequestData.Name,
                    IpAddress = loginRequestData.IpAddress,
                    LoginId = loginRequestData.LoginId,
                };

                int ipDataId = 0;
                int userDataId = 0;

                var ipData = _dbContext.UserIpAddresses.Where(x => x.IpAddress == loginRequestData.IpAddress).FirstOrDefault();
                var userData = _dbContext.UserDetails.Where(x => x.EmailId == loginRequestData.EmailId
                && x.UserName == loginRequestData.Name).FirstOrDefault();

                if (ipData == null)
                {
                    var newIpData = new Db.Entity.UserIpAddress()
                    {
                        IpAddress = loginRequestData.IpAddress!,
                        CreatedAt = estTime,
                        UpdatedAt = estTime
                    };
                    _dbContext.UserIpAddresses.Add(newIpData);
                    await _dbContext.SaveChangesAsync();
                    ipDataId = newIpData.Id;
                }
                else
                {
                    ipDataId = ipData.Id;
                }

                if (userData == null)
                {
                    var newUserData = new Db.Entity.UserDetail()
                    {
                        EmailId = loginRequestData.EmailId!,
                        UserName = loginRequestData.Name!,
                        CreatedAt = estTime,
                        UpdatedAt = estTime
                    };
                    _dbContext.UserDetails.Add(newUserData);
                    await _dbContext.SaveChangesAsync();
                    userDataId = newUserData.Id;
                }
                else
                {
                    userDataId = userData.Id;
                }

                var lastActiveSession = _dbContext.UserLoginDetails.Where(x => x.IpAddressId == ipDataId
                && x.UserId == userDataId && x.IsActive && x.LogoutTime == null).FirstOrDefault();

                if (lastActiveSession == null)
                {
                    var loginhistoryData = new Db.Entity.UserLoginHistory()
                    {
                        UserId = userDataId,
                        IpAddressId = ipDataId,
                        LoginTime = estTime,
                        IsActive = true,
                        LogoutTime = null,
                        LastActivityTime = estTime,
                    };
                    _dbContext.UserLoginHistories.Add(loginhistoryData);
                    await _dbContext.SaveChangesAsync();
                    returnData.LoggedOnTime = loginhistoryData.LoginTime;
                    returnData.IsActive = loginhistoryData.IsActive;
                    returnData.LastActivityTime = loginhistoryData.LastActivityTime;
                    returnData.IsSuccessful = true;
                    returnData.Message = "Login successful.";
                    returnData.LoginId = loginhistoryData.Id;
                    returnData.ReturnCode = 1;
                }
                else
                {
                    returnData.LoggedOnTime = lastActiveSession.LoginTime;
                    returnData.IsActive = lastActiveSession.IsActive;
                    returnData.LastActivityTime = lastActiveSession.LastActivityTime;
                    returnData.IsSuccessful = true;
                    returnData.Message = "User already has an active session.";
                    returnData.LoginId = lastActiveSession.UserHistoryId;
                    returnData.ReturnCode = 2;
                }
                return returnData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in 'LogOnAsync' method.");

                return new UserLogResponseData()
                {
                    Message = ex.ToString(),
                    IsSuccessful = false
                };
            }
        }

        public async Task<UserLogResponseData> UpdatedActivityAsync(UserLogRequestData loginRequestData)
        {
            try
            {
                var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var estTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone);

                var returnData = new UserLogResponseData()
                {
                    EmailId = loginRequestData.EmailId,
                    Name = loginRequestData.Name,
                    IpAddress = loginRequestData.IpAddress,
                    LoginId = loginRequestData.LoginId,
                };

                int ipDataId = 0;
                int userDataId = 0;

                var ipData = _dbContext.UserIpAddresses.Where(x => x.IpAddress == loginRequestData.IpAddress).FirstOrDefault();
                var userData = _dbContext.UserDetails.Where(x => x.EmailId == loginRequestData.EmailId
                    && x.UserName == loginRequestData.Name).FirstOrDefault();

                if (ipData == null)
                {
                    returnData.IsSuccessful = false;
                    returnData.Message = "No active session found for the user.";
                    returnData.ReturnCode = 0;
                    return returnData;

                }
                else
                {
                    ipDataId = ipData.Id;
                }

                if (userData == null)
                {
                    returnData.IsSuccessful = false;
                    returnData.Message = "No active session found for the user.";
                    returnData.ReturnCode = 0;
                    return returnData;
                }
                else
                {
                    userDataId = userData.Id;
                }

                var lastActiveSession = _dbContext.UserLoginDetails.Where(x => x.IpAddressId == ipDataId
                && x.UserId == userDataId && x.IsActive && x.LogoutTime == null).FirstOrDefault();

                if (lastActiveSession != null)
                {
                    var loginHistoryData = _dbContext.UserLoginHistories.Where(x => x.Id == lastActiveSession.UserHistoryId).FirstOrDefault();
                    if (loginHistoryData != null)
                    {
                        loginHistoryData.LastActivityTime = estTime;
                        _dbContext.UserLoginHistories.Update(loginHistoryData);
                        _dbContext.SaveChanges();
                        returnData.IsSuccessful = true;
                        returnData.Message = "Activity updated successfully.";
                        returnData.ReturnCode = 1;
                    }
                    else
                    {
                        returnData.IsSuccessful = false;
                        returnData.Message = "No active session found for the user.";
                        returnData.ReturnCode = 0;
                    }
                }
                else
                {
                    returnData.IsSuccessful = false;
                    returnData.Message = "No active session found for the user.";
                    returnData.ReturnCode = 0;
                }

                return returnData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in 'UpdatedActivityAsync' method.");

                return new UserLogResponseData()
                {
                    Message = ex.ToString(),
                    IsSuccessful = false
                };
            }
        }
    }
}

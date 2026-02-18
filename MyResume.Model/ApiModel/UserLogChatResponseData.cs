using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Model.ApiModel
{
    public class UserLogChatResponseData : UserLogResponseData
    {
        public string? UserRequestPromt { get; set; }
        public string? AiResponseMessage { get; set; }
    }
}

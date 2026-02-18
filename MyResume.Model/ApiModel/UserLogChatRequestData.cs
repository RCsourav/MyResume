using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Model.ApiModel
{
    public class UserLogChatRequestData:UserLogRequestData
    {
        public string? UserRequestPromt { get; set; }
        public string? AiResponseMessage { get; set; }
    }
}

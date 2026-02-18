using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Model.ApiModel
{
    public class UserLogResponseData : UserLogRequestData
    {
        public string? Message { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime? LoggedOnTime { get; set; }
        public DateTime? LastActivityTime { get; set; }
        public bool? IsActive { get; set; }
        public int ReturnCode { get; set; } = 0;
    }
}

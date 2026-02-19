using System;
using System.Collections.Generic;
using System.Text;

namespace MyResume.Model.ApiModel
{
    public class UserLogRequestData
    {
        public int? LoginId { get; set; }
        public string? IpAddress { get; set; }
        public string? Name { get; set; }
        public string? EmailId { get; set; }
    }
}

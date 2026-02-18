using System;
using System.Collections.Generic;

namespace MyResume.Api.Repo.Db.Entity;

public partial class UserLoginDetail
{
    public int UserHistoryId { get; set; }

    public int IpAddressId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string IpAddress { get; set; } = null!;

    public DateTime LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    public bool IsActive { get; set; }

    public DateTime LastActivityTime { get; set; }
}

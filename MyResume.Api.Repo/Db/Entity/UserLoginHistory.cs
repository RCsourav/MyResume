using System;
using System.Collections.Generic;

namespace MyResume.Api.Repo.Db.Entity;

public partial class UserLoginHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int IpAddressId { get; set; }

    public DateTime LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    public bool IsActive { get; set; }

    public DateTime LastActivityTime { get; set; }

    public virtual UserIpAddress IpAddress { get; set; } = null!;

    public virtual ICollection<LoginChatHistory> LoginChatHistories { get; set; } = new List<LoginChatHistory>();

    public virtual UserDetail User { get; set; } = null!;
}

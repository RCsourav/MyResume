using System;
using System.Collections.Generic;

namespace MyResume.Api.Repo.Db.Entity;

public partial class UserIpAddress
{
    public int Id { get; set; }

    public string IpAddress { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<UserLoginHistory> UserLoginHistories { get; set; } = new List<UserLoginHistory>();
}

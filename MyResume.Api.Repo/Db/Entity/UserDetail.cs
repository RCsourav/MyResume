using System;
using System.Collections.Generic;

namespace MyResume.Api.Repo.Db.Entity;

public partial class UserDetail
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<UserLoginHistory> UserLoginHistories { get; set; } = new List<UserLoginHistory>();
}

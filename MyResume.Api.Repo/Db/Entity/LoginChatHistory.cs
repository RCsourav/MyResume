using System;
using System.Collections.Generic;

namespace MyResume.Api.Repo.Db.Entity;

public partial class LoginChatHistory
{
    public int Id { get; set; }

    public int UserLoginHistoryId { get; set; }

    public string RequestMessage { get; set; } = null!;

    public string ResponseMessage { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual UserLoginHistory UserLoginHistory { get; set; } = null!;
}

CREATE TABLE [dbo].[UserLoginHistory] (
    [Id]               INT      IDENTITY (1, 1) NOT NULL,
    [UserId]           INT      NOT NULL,
    [IpAddressId]      INT      NOT NULL,
    [LoginTime]        DATETIME CONSTRAINT [DfUserLoginHistoryLoginTime] DEFAULT (getutcdate()) NOT NULL,
    [LogoutTime]       DATETIME NULL,
    [IsActive]         BIT      CONSTRAINT [DfUserLoginHistoryIsActive] DEFAULT ((1)) NOT NULL,
    [LastActivityTime] DATETIME NOT NULL,
    CONSTRAINT [PkUserLoginHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkUserLoginHistoryUserDetails] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserDetails] ([Id]),
    CONSTRAINT [FkUserLoginHistoryUserIpAddress] FOREIGN KEY ([IpAddressId]) REFERENCES [dbo].[UserIpAddress] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IXUserLoginHistoryUserId]
    ON [dbo].[UserLoginHistory]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IXUserLoginHistoryIpAddressId]
    ON [dbo].[UserLoginHistory]([IpAddressId] ASC);


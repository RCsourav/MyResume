CREATE TABLE [dbo].[UserIpAddress] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [IpAddress] VARCHAR (45) NOT NULL,
    [CreatedAt] DATETIME     CONSTRAINT [DfUserIpAddressCreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt] DATETIME     CONSTRAINT [DfUserIpAddressUpdatedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PkUserIpAddress] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IXUserIpAddressIpAddress]
    ON [dbo].[UserIpAddress]([IpAddress] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UXUserIpAddressIpAddress]
    ON [dbo].[UserIpAddress]([IpAddress] ASC);


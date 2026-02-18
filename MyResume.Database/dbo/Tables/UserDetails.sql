CREATE TABLE [dbo].[UserDetails] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [UserName]  VARCHAR (255) NOT NULL,
    [EmailId]   VARCHAR (255) NOT NULL,
    [CreatedAt] DATETIME      CONSTRAINT [DfUserDetailsCreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt] DATETIME      CONSTRAINT [DfUserDetailsUpdatedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PkUserDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IXUserDetailsEmailId]
    ON [dbo].[UserDetails]([EmailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IXUserDetailsUserName]
    ON [dbo].[UserDetails]([UserName] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UXUserDetailsUserNameEmailId]
    ON [dbo].[UserDetails]([UserName] ASC, [EmailId] ASC);


CREATE TABLE [dbo].[LoginChatHistory] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [UserLoginHistoryId] INT           NOT NULL,
    [RequestMessage]     VARCHAR (MAX) NOT NULL,
    [ResponseMessage]    VARCHAR (MAX) NOT NULL,
    [CreatedAt]          DATETIME      CONSTRAINT [DfLoginChatHistoryCreatedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PkLoginChatHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkLoginChatHistoryUserLoginHistory] FOREIGN KEY ([UserLoginHistoryId]) REFERENCES [dbo].[UserLoginHistory] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IXLoginChatHistoryUserLoginHistoryId]
    ON [dbo].[LoginChatHistory]([UserLoginHistoryId] ASC);


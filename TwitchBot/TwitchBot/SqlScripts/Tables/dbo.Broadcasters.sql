USE [twitchbotdb]
GO

/****** Object: Table [dbo].[tblBroadcasters] Script Date: 9/8/2016 2:09:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Broadcasters] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [username]  VARCHAR (30) NOT NULL,
    [timeAdded] DATETIME     DEFAULT (getdate()) NOT NULL,
    [twitchId]  INT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_username] UNIQUE NONCLUSTERED ([username] ASC)
);

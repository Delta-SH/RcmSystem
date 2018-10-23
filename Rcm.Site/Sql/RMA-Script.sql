/*
* Web Sql Script Library v1.0.0
* Copyright 2018, Delta
* Author: Steven
* Date: 2018/10/23
*/

USE [RMA]
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--修改表[dbo].[MStation]
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MStation]') AND name = N'Longitude')
BEGIN
	ALTER TABLE [dbo].[MStation] ALTER COLUMN [Longitude] FLOAT NOT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MStation]') AND name = N'Latitude')
BEGIN
	ALTER TABLE [dbo].[MStation] ALTER COLUMN [Latitude] FLOAT NOT NULL;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--修改表[dbo].[GVideo]
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[GVideo]') AND name = N'ImgIP')
BEGIN
	ALTER TABLE [dbo].[GVideo] ADD [ImgIP] VARCHAR(15) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[GVideo]') AND name = N'ImgPort')
BEGIN
	ALTER TABLE [dbo].[GVideo] ADD [ImgPort] INT NULL;
END

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--新增表[dbo].[SExSetting]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SExSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SExSetting](
	[ID] [int] NOT NULL,
	[Desc] [varchar](80) NOT NULL,
	[Value] [varchar](max) NOT NULL,
 CONSTRAINT [PK_SExSetting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO
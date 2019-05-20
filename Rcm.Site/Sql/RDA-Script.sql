/*
* Web Sql Script Library v1.4.1
* Copyright 2019, Delta
* Author: Steven
* Date: 2019/05/20
*/

USE [RDA]
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[MNode]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MNode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MNode](
	[NodeID] [int] NOT NULL,
	[NodeType] [int] NOT NULL,
	[OpType] [int] NULL,
	[OpValue] [real] NULL,
 CONSTRAINT [PK_MNode] PRIMARY KEY CLUSTERED 
(
	[NodeID] ASC,
	[NodeType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[HNode]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HNode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HNode](
	[DevID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[NodeType] [int] NOT NULL,
	[Value] [real] NOT NULL,
	[State] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_HNode_1] PRIMARY KEY CLUSTERED 
(
	[DevID] ASC,
	[NodeID] ASC,
	[NodeType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--修改表[dbo].[MNode]
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MNode]') AND name = N'OpValue')
BEGIN
	ALTER TABLE [dbo].[MNode] ALTER COLUMN [OpValue] FLOAT NULL;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--修改表[dbo].[HNode]
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HNode]') AND name = N'Value')
BEGIN
	ALTER TABLE [dbo].[HNode] ALTER COLUMN [Value] FLOAT NOT NULL;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[UpdateHNode]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateHNode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateHNode]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Steven>
-- Create date: <2019/05/20>
-- Description:	<Update HNode Table>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateHNode]
	@DevID int,
	@NodeID int,
	@NodeType int,
	@Value float,
	@State int,
	@UpdateTime datetime
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].[HNode] SET [Value]=@Value, [State]=@State, [UpdateTime]=@UpdateTime WHERE [DevID] = @DevID AND [NodeID]=@NodeID AND [NodeType]=@NodeType;
	IF(@@ROWCOUNT = 0)
	BEGIN
		INSERT INTO [dbo].[HNode]([DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime]) VALUES(@DevID,@NodeID,@NodeType,@Value,@State,@UpdateTime);
	END
END
GO
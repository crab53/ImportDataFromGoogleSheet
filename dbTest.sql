USE [master]
GO
CREATE DATABASE [Test]
GO
USE [Test]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 10/17/2018 7:49:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentName] [nvarchar](50) NULL,
	[Gender] [nvarchar](50) NULL,
	[ClassLevel] [nvarchar](50) NULL,
	[HomeState] [nvarchar](50) NULL,
	[Major] [nvarchar](50) NULL,
	[ExtracurricularActivity] [nvarchar](50) NULL
) ON [PRIMARY]
GO

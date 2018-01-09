/****** Object:  Table [dbo].[BusMessage]    Script Date: 1/9/2018 12:57:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BusMessage](
	[MessageKey] [varchar](50) NOT NULL,
	[InsertDate] [datetime] NOT NULL
)

GO

SET ANSI_PADDING OFF
GO



/****** Object:  Table [dbo].[FunctionException]    Script Date: 1/9/2018 12:58:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FunctionException](
	[RunKey] [varchar](50) NOT NULL,
	[Step] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[FuncVer] [int] NOT NULL,
	[Details] [varchar](max) NOT NULL
)

GO

SET ANSI_PADDING OFF
GO




/****** Object:  Table [dbo].[FunctionRun]    Script Date: 1/9/2018 12:58:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FunctionRun](
	[ProcessId] [varchar](50) NOT NULL,
	[ThreadId] [varchar](50) NOT NULL,
	[RunKey] [varchar](50) NOT NULL,
	[Step] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[FuncVer] [int] NOT NULL
)

GO

SET ANSI_PADDING OFF
GO



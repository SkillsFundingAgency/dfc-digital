USE [dfc-digital-sitefinity]
GO

/****** Object:  Table [dbo].[MigrationToolMapping]    Script Date: 26/01/2022 15:20:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MigrationToolMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[SitefinityId] [uniqueidentifier] NOT NULL,
	[OrchardCoreId] [nvarchar](450) NOT NULL,
	[ContentType] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_MigrationToolMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



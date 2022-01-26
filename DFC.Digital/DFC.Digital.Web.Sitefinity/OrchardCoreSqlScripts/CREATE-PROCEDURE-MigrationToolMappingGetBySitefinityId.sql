USE [dfc-digital-sitefinity]
GO

/****** Object:  StoredProcedure [dbo].[MigrationToolMappingGetBySitefinityId]    Script Date: 26/01/2022 15:21:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MigrationToolMappingGetBySitefinityId] 
(
			@SitefinityId uniqueidentifier
)
AS
BEGIN
	SELECT [MappingId]
		  ,[SitefinityId]
		  ,[OrchardCoreId]
		  ,[ContentType]
	  FROM [MigrationToolMapping]
	  WHERE [SitefinityId] = @SitefinityId
END
GO



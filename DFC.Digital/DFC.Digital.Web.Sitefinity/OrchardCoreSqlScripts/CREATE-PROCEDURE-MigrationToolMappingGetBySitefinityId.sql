USE [dfc-digital-sitefinity]
GO

/****** Object:  StoredProcedure [dbo].[MigrationToolMappingGetBySitefinityId]    Script Date: 01/03/2022 21:10:44 ******/
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
		  ,[ContentItemVersionId]
		  ,[ContentType]
	  FROM [MigrationToolMapping]
	  WHERE [SitefinityId] = @SitefinityId
	  ORDER BY [MappingId] DESC
END
GO



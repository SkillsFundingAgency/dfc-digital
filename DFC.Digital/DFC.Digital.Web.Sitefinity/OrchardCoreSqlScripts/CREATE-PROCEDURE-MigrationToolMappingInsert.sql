USE [dfc-digital-sitefinity]
GO

/****** Object:  StoredProcedure [dbo].[MigrationToolMappingInsert]    Script Date: 01/03/2022 21:11:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MigrationToolMappingInsert] 
(
			@SitefinityId uniqueidentifier
           ,@OrchardCoreId nvarchar(450)
		   ,@ContentItemVersionId nvarchar(450)
           ,@ContentType nvarchar(450)
)
AS
BEGIN
	INSERT INTO [MigrationToolMapping]
           ([SitefinityId]
           ,[OrchardCoreId]
		   ,[ContentItemVersionId]
           ,[ContentType])
     VALUES
           (@SitefinityId
           ,@OrchardCoreId
		   ,@ContentItemVersionId
           ,@ContentType)
END
GO



USE [dfc-digital-sitefinity]
GO

/****** Object:  StoredProcedure [dbo].[MigrationToolMappingInsert]    Script Date: 26/01/2022 15:21:58 ******/
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
           ,@ContentType nvarchar(450)
)
AS
BEGIN
	INSERT INTO [MigrationToolMapping]
           ([SitefinityId]
           ,[OrchardCoreId]
           ,[ContentType])
     VALUES
           (@SitefinityId
           ,@OrchardCoreId
           ,@ContentType)
END
GO



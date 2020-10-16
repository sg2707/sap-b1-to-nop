-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--EXEC SI_GetCompanyList
ALTER PROCEDURE SI_GetCompanyList
	-- Add the parameters for the stored procedure here
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select C1.ID [ID], IPAddress [Server], C1.LicenseServer [LicenseServer], SAPDatabase [CompanyDB], 
	UserID [DBUser], Password [DBPass], SAPUserName [CompanyUser],SAPPassword [CompanyPass], C1.DBServerType [DBServerType],
	C2.SourcePath [SourcePath], C2.ArchivePath [ArchivePath], IsActive [Active] 
	from [dbo].[CompanyManagement] C1 inner join [dbo].[SI_VendorFolderPath] C2 on C1.ID = C2.ID where C1.IsActive = 1

END
GO
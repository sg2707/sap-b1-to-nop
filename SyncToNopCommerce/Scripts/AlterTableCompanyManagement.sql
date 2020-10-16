--alter table with 2 extra columns
ALTER TABLE [dbo].[CompanyManagement]
  ADD LicenseServer nvarchar(20) NULL, 
  DBServerType nvarchar(5) NULL
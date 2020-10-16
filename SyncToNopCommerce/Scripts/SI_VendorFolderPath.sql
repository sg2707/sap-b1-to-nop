USE [MHWindows]
GO

/****** Object:  Table [dbo].[CompanyManagement]    Script Date: 3/10/2019 1:51:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE SI_VendorFolderPath
( ID INT,
  SourcePath VARCHAR(max) NULL,
  ArchivePath VARCHAR(max) NULL
  CONSTRAINT company_pk PRIMARY KEY (ID)
);

GO




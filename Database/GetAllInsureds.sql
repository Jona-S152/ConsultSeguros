USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[GetAllInsureds]    Script Date: 31/10/2024 11:14:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllInsureds]
AS
BEGIN
    SELECT *
    FROM   Insured
    WHERE  Status = 1;
END

GO


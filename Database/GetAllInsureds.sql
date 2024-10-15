USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[GetAllInsureds]    Script Date: 15/10/2024 14:43:02 ******/
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


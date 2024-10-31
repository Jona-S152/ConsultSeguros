USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[GetInsuredByIdentification]    Script Date: 31/10/2024 11:15:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetInsuredByIdentification]
@Identification VARCHAR (10)
AS
BEGIN
    SELECT *
    FROM   Insured
    WHERE  Identification = @Identification
           AND Status = 1;
END

GO


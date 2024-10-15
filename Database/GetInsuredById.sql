USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[GetInsuredById]    Script Date: 15/10/2024 14:42:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetInsuredById]
@Id INT
AS
BEGIN
    SELECT *
    FROM   Insured
    WHERE  Id = @Id
           AND Status = 1;
END

GO


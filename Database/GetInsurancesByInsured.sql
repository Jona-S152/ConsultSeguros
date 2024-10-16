USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[GetInsurancesByInsured]    Script Date: 16/10/2024 15:13:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetInsurancesByInsured]
@Identification VARCHAR (10), @Result BIT OUTPUT
AS
BEGIN
    IF (SELECT COUNT(1)
        FROM   Insured
        WHERE  Identification = @Identification
               AND Status = 1) = 0
        BEGIN
            SET @Result = 0;
            RETURN @Result;
        END
    SELECT isc.*
    FROM   Insured AS i
           INNER JOIN
           InsuranceInsured AS isd
           ON isd.Id_Insured = i.Id
              AND i.Status = 1
           INNER JOIN
           Insurance AS isc
           ON isd.Id_Insurance = isc.Id
              AND isc.Status = 1
    WHERE  i.Identification = @Identification
           AND isd.Status = 1;
    SET @Result = 1;
    RETURN @Result;
END

GO


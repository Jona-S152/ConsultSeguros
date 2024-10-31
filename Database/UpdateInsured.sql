USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[UpdateInsured]    Script Date: 31/10/2024 11:16:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateInsured]
@Id INT, @Identification VARCHAR (10), @InsuredName VARCHAR (50), @PhoneNumber VARCHAR (10), @Age INT, @Insurances VARCHAR (MAX), @Result BIT OUTPUT
AS
BEGIN
    IF (SELECT COUNT(1)
        FROM   Insured
        WHERE  Id = @Id
               AND Status = 1) = 0
        BEGIN
            SET @Result = 0;
            RETURN @Result;
        END
    UPDATE Insured
    SET    Identification = @Identification,
           InsuredName    = @InsuredName,
           PhoneNumber    = @PhoneNumber,
           Age            = @Age
    WHERE  Id = @Id
           AND Status = 1;
    DECLARE @Insurancestbl AS dbo.InsurancesIU;
    INSERT INTO @Insurancestbl (Id_Insured, Id_Insurances, Status)
    SELECT @Id,
           CAST (VALUE AS INT),
           1
    FROM   STRING_SPLIT (@Insurances, ',');
    EXECUTE IU_InsuranceInsured @Id, @Insurancestbl;
    SET @Result = 1;
    RETURN @Result;
END

GO


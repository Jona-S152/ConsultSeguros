USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[InsertInsured]    Script Date: 31/10/2024 11:16:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertInsured]
@Identification VARCHAR (10), @InsuredName VARCHAR (50), @PhoneNumber VARCHAR (10), @Age INT, @Insurances VARCHAR (MAX), @Result BIT OUTPUT
AS
BEGIN
    IF (SELECT COUNT(1)
        FROM   Insured
        WHERE  Identification = @Identification) > 0
        BEGIN
            IF (SELECT COUNT(1)
                FROM   Insured
                WHERE  Identification = @Identification
                       AND Status = 1) > 0
                BEGIN
                    SET @Result = 0;
                    RETURN @Result;
                END
            ELSE
                BEGIN
                    UPDATE Insured
                    SET    InsuredName = @InsuredName,
                           PhoneNumber = @PhoneNumber,
                           Age         = @Age,
                           Status      = 1
                    WHERE  Identification = @Identification;
                    SET @Result = 1;
                    RETURN @Result;
                END
        END
    INSERT  INTO Insured (Identification, InsuredName, PhoneNumber, Age, Status)
    VALUES              (@Identification, @InsuredName, @PhoneNumber, @Age, 1);
    DECLARE @IdLastInsured AS INT = (SELECT   TOP 1 Id
                                     FROM     Insured
                                     WHERE    Status = 1
                                     ORDER BY Id DESC);
    DECLARE @Insurancestbl AS dbo.InsurancesIU;
    INSERT INTO @Insurancestbl (Id_Insured, Id_Insurances, Status)
    SELECT @IdLastInsured,
           CAST (VALUE AS INT),
           1
    FROM   STRING_SPLIT (@Insurances, ',');
    EXECUTE IU_InsuranceInsured @IdLastInsured, @Insurancestbl;
    SET @Result = 1;
    RETURN @Result;
END

GO


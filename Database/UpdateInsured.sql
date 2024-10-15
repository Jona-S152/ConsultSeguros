USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[UpdateInsured]    Script Date: 15/10/2024 14:42:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateInsured]
@Id INT, @Identification VARCHAR (10), @InsuredName VARCHAR (50), @PhoneNumber VARCHAR (10), @Age INT, @Result BIT OUTPUT
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
    SET @Result = 1;
    SELECT *
    FROM   Insured
    WHERE  Id = @Id
           AND Status = 1;
    RETURN @Result;
END

GO


USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[DeleteInsured]    Script Date: 15/10/2024 14:43:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [dbo].[DeleteInsured]
@Id INT, @Result BIT OUTPUT
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
    SET    Status = 0
    WHERE  Id = @Id
           AND Status = 1;
    SET @Result = 1;
    RETURN @Result;
END

GO


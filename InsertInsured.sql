USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[InsertInsured]    Script Date: 14/10/2024 22:35:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [dbo].[InsertInsured]
	@Identification VARCHAR(10),
	@InsuredName VARCHAR(50),
	@PhoneNumber DECIMAL(1,1),
	@Age INT,
	@Result BIT OUTPUT
AS
BEGIN
	IF (SELECT COUNT(1) FROM Insured WHERE Identification = @Identification) > 0
	BEGIN
		IF (SELECT COUNT(1) FROM Insured WHERE Identification = @Identification AND Status = 1) > 0
		BEGIN
			SET @Result = 0
			RETURN @Result
		END
		ELSE
		BEGIN
			UPDATE Insured SET InsuredName = @InsuredName, PhoneNumber = @PhoneNumber, Age = @Age, Status = 1 WHERE Identification = @Identification
			SET @Result = 1	
			RETURN @Result
		END
	END

	INSERT INTO Insured (Identification, InsuredName, PhoneNumber, Age, Status) VALUES (@Identification, @InsuredName, @PhoneNumber, @Age, 1)
	SET @Result = 1
	RETURN @Result
END
GO


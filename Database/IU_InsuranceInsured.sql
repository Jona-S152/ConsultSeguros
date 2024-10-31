USE [DB_Seguros]
GO

/****** Object:  StoredProcedure [dbo].[IU_InsuranceInsured]    Script Date: 31/10/2024 11:16:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [dbo].[IU_InsuranceInsured]
@Id_Insured INT, @Insurances InsurancesIU READONLY
AS
BEGIN
    UPDATE InsuranceInsured
    SET    Status = 0
    WHERE  NOT EXISTS (SELECT 1
                       FROM   @Insurances AS i
                       WHERE  InsuranceInsured.Id_Insured = i.Id_Insured
                              AND InsuranceInsured.Id_Insurance = i.Id_Insurances)
           AND Id_Insured = @Id_Insured;
    INSERT INTO InsuranceInsured (Id_Insured, Id_Insurance, Status)
    SELECT i.Id_Insured,
           i.Id_Insurances,
           Status
    FROM   @Insurances AS i
    WHERE  NOT EXISTS (SELECT 1
                       FROM   InsuranceInsured AS ii
                       WHERE  ii.Id_Insured = i.Id_Insured
                              AND ii.Id_Insurance = i.Id_Insurances)
           AND Id_Insured = @Id_Insured;
    UPDATE InsuranceInsured
    SET    Status = 1
    WHERE  EXISTS (SELECT 1
                   FROM   @Insurances AS i
                   WHERE  InsuranceInsured.Id_Insured = i.Id_Insured
                          AND InsuranceInsured.Id_Insurance = i.Id_Insurances)
           AND Id_Insured = @Id_Insured;
END

GO


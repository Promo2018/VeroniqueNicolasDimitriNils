--USE BoVoyage_VNND;

--########################################################################################################################--
--------------------------------------------FONCTIONS ----------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------
--########################################################################################################################--




------------FONCTION pour calculer des différences de date au jour près-------------------
CREATE FUNCTION JourPass(@idv INT)
RETURNS INT
AS 
BEGIN
DECLARE @now DATE, @dar DATE, @passjour INT --, @diffjour INT
SELECT @now=GETDATE(); 
SELECT @dar=(SELECT [date retour] FROM Voyages WHERE id_voyage = @idv);
SET @passjour = 0;
IF (@now > @dar) 
SET @passjour = 1;
ELSE 
SET @passjour = 2;
RETURN @passjour;
END;
GO

SELECT dbo.JourPass(8);
DROP FUNCTION dbo.JourPass;


CREATE FUNCTION JourDiff(@idv INT)
RETURNS INT
AS 
BEGIN
DECLARE @now DATE, @dar DATE, @diffjour INT 
SELECT @now=GETDATE(); 
SELECT @dar=(SELECT [date retour] FROM Voyages WHERE id_voyage = @idv);
SET @diffjour = (SELECT DATEDIFF(day, @dar, @now));
RETURN @diffjour;
END;
GO

SELECT dbo.JourDiff(8);
DROP FUNCTION dbo.JourDiff;


CREATE FUNCTION JourDernierSuivi(@idd INT)
RETURNS INT
AS 
BEGIN
DECLARE @now DATE, @dar DATE, @diffsuivi INT 
SELECT @now=GETDATE(); 
SELECT @dar=(SELECT [dernier suivi] FROM Dossiers WHERE id_dossier = @idd);
SET @diffsuivi = (SELECT DATEDIFF(day, @dar, @now));
RETURN @diffsuivi;
END;
GO

SELECT * FROM Dossiers WHERE id_dossier=8;
SELECT dbo.JourDernierSuivi(39);

DROP FUNCTION dbo.JourDernierSuivi;

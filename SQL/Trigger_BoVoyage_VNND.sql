--USE BoVoyage_VNND;
/*
--########################################################################################################################
--------------------------------TRIGGER --------------------------------
--########################################################################################################################
--DROP TRIGGER AnnulationDossier;

CREATE TRIGGER AnnulationDossier ON Dossiers
AFTER UPDATE 
AS 
IF exists (SELECT id_dossier FROM Dossiers WHERE etat = 3)
	BEGIN
		IF UPDATE (etat) 
		IF exists (SELECT id_dossier FROM Dossiers WHERE etat = 3 and [raison annulation]= 2 or [raison annulation]= 3)
		BEGIN 
		SELECT * FROM Dossiers WHERE etat = 3 and ([raison annulation]= 2)
		FOR JSON PATH, ROOT('Clientannulation')
		END
		BEGIN 
		SELECT * FROM Dossiers WHERE etat = 3 and ([raison annulation]= 3)
		FOR JSON PATH, ROOT('PlacesInsuffisantes')
		END
	END
GO

------
--Desactiver le trigger
------
DISABLE TRIGGER AnnulationDossier ON Dossiers;
GO

------
--Activer le trigger
------
ENABLE TRIGGER AnnulationDossier ON Dossiers;
GO
*/

--########################################################################################################################--
--------------------------------------------TRIGGER Suppression Voyage si ------------------------------------------------
--------------------------------------------Date Retour dépassée -----------------------------------------------------------
--########################################################################################################################--
--DROP TRIGGER PeremptionDate;

CREATE TRIGGER  PeremptionDate ON Voyages
AFTER UPDATE ,INSERT, DELETE 
AS 
DECLARE C2 CURSOR FOR SELECT id_voyage FROM Voyages; 
BEGIN
	OPEN C2;
	DECLARE @indexV INT, @diffjour INT, @passjour INT ;
	FETCH NEXT FROM C2 INTO @indexV;
	WHILE (@@FETCH_STATUS=0)
		BEGIN
			SET @passjour = (SELECT dbo.JourPass(@indexV));
			IF (@passjour = 1)
				BEGIN
					SET @diffjour = (SELECT dbo.JourDiff(@indexV));
					IF (@diffjour > 30)
						BEGIN
							DELETE Voyages WHERE id_voyage = @indexV;
							DELETE Dossiers WHERE voyage = @indexV;
						END
				END
			PRINT @indexV ;
			FETCH NEXT FROM C2 INTO @indexV;
		END
	CLOSE C2;
DEALLOCATE C2;
END
GO


------
--Desactiver le trigger
------
DISABLE TRIGGER PeremptionDate ON Voyages;
GO

------
--Activer le trigger
------
ENABLE TRIGGER PeremptionDate ON Voyages;
GO

--########################################################################################################################--
--------------------------------------------TRIGGER Transition après négociation Dossier si ------------------------------------------------
--------------------------------------------Date Retour dépassée et etat = 3-----------------------------------------------------------
--########################################################################################################################--

DROP TRIGGER Negociation;

CREATE TRIGGER Negociation ON Dossiers
AFTER UPDATE ,INSERT, DELETE 
AS 
DECLARE C4 CURSOR FOR SELECT id_dossier FROM Dossiers; 
BEGIN
	OPEN C4;
	DECLARE @indexD INT, @diff INT;
	FETCH NEXT FROM C4 INTO @indexD;
	WHILE (@@FETCH_STATUS=0)
		BEGIN
			SET @diff = (SELECT dbo.JourDernierSuivi(@indexD));
			IF (@diff > 2)
				BEGIN
					UPDATE Dossiers SET etat = 3 WHERE etat = 2 and id_dossier = @indexD;
				END
			PRINT @indexD ;
			FETCH NEXT FROM C4 INTO @indexD;
		END
	CLOSE C4;
	DEALLOCATE C4;
END
GO

SELECT * FROM Dossiers ORDER BY [dernier suivi];
UPDATE Dossiers SET etat = 2 WHERE etat = 1 and id_dossier = 40;


------
--Desactiver le trigger
------
DISABLE TRIGGER Negociation ON Dossiers;
GO

------
--Activer le trigger
------
ENABLE TRIGGER Negociation ON Dossiers;
GO


--########################################################################################################################--
--------------------------------------------TRIGGER Suppression Dossier si -------------------------------------------------
--------------------------------------------Date Retour dépassée et etat = 3------------------------------------------------
--########################################################################################################################--
--DROP TRIGGER SuppressionDossier ;

CREATE TRIGGER SuppressionDossier ON Dossiers
AFTER UPDATE 
AS 
IF UPDATE (etat) 
	BEGIN
		DECLARE C3 CURSOR FOR SELECT voyage FROM Dossiers; 
		BEGIN
			OPEN C3;
			DECLARE @indexV INT, @diffjour INT ;
			FETCH NEXT FROM C3 INTO @indexV;
			WHILE (@@FETCH_STATUS=0)
				BEGIN
					SET @diffjour = (SELECT dbo.JourDiff(@indexV));
					IF (@diffjour > 30)
						IF exists (SELECT * FROM Dossiers WHERE voyage = @indexV)
							BEGIN 
								DELETE FROM Dossiers WHERE voyage = @indexV;
							END
						PRINT @indexV ;
						FETCH NEXT FROM C3 INTO @indexV;
				END
		END
		CLOSE C3;
		DEALLOCATE C3;
	END
GO


------
--Desactiver le trigger
------
DISABLE TRIGGER SuppressionDossier ON Dossiers;
GO

------
--Activer le trigger
------
ENABLE TRIGGER SuppressionDossier ON Dossiers;
GO






------
--Activer tous les triggers
------

ENABLE TRIGGER AnnulationDossier ON Dossiers;
ENABLE TRIGGER PeremptionDate ON Voyages;
ENABLE TRIGGER Negociation ON Dossiers;
ENABLE TRIGGER SuppressionDossier ON Dossiers;

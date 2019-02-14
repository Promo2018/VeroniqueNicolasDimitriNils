
--DROP DATABASE BoVoyage_VNND;
--CREATE DATABASE BoVoyage_VNND;
--USE BoVoyage_VNND;
/*

DROP TABLE [Liste Participants];
DROP TABLE [Liste Assurances];
DROP TABLE Dossiers;
DROP TABLE [Etats Dossiers];
DROP TABLE [Raisons Annulations];
DROP TABLE Assurances;
DROP TABLE Voyages;
DROP TABLE Destinations;
DROP TABLE Continents;
DROP TABLE Agences;
DROP TABLE Personnes;
DROP TABLE Civilites;
DROP TABLE Authentifications;
DROP TABLE Statuts;

*/

------------------------TABLE GERANT ENUM POUR AUTHENTIFICATIONS----------------------------
CREATE TABLE Statuts(
	statut NVARCHAR(16) UNIQUE NOT NULL, -- mettre les 4 choix Client, Administrateur, Commercial, Marketting
	PRIMARY KEY (statut),
);
INSERT INTO Statuts (statut) VALUES ('Client');
INSERT INTO Statuts (statut) VALUES ('Commercial');
INSERT INTO Statuts (statut) VALUES ('Marketing');
INSERT INTO Statuts (statut) VALUES ('Administrateur');

------------------------TABLE AUTHENTIFICATIONS-------------------------
CREATE TABLE Authentifications (
    email NVARCHAR(64) UNIQUE NOT NULL,
	[mot de passe] VARBINARY(64) NOT NULL,
    statut NVARCHAR(16) NOT NULL, --FK
);
ALTER TABLE Authentifications ADD CONSTRAINT Fk_0Authentifications FOREIGN KEY(statut) REFERENCES Statuts(statut);

------------------------TABLE GERANT ENUM POUR PERSONNES ----------------------------
CREATE TABLE Civilites(
	civilite NVARCHAR(8) UNIQUE NOT NULL, --mettre deux lignes M ou Mme 
	PRIMARY KEY (civilite),
);
INSERT INTO Civilites (civilite) VALUES ('M');
INSERT INTO Civilites (civilite) VALUES ('Mme');

------------------------TABLE PERSONNES-------------------------
CREATE TABLE Personnes(
    id_personne INT IDENTITY(1,1),
    civilite NVARCHAR(8) NOT NULL, --FK
    nom NVARCHAR(32) NOT NULL,
    prenom NVARCHAR(32) NOT NULL,
	adresse NVARCHAR(230) NOT NULL,
	telephone NVARCHAR(32) NOT NULL,  
	[date naissance] DATE NOT NULL,
	client INT, 
	participant INT, 
	email NVARCHAR(64) UNIQUE, -- à faire trigger pour contrainte non null si client true
	PRIMARY KEY(id_personne)
);

ALTER TABLE Personnes ADD CONSTRAINT Fk_1Civilites FOREIGN KEY(civilite) REFERENCES Civilites(civilite);


------------------------TABLE AGENCES-------------------------------

CREATE TABLE Agences(
	id_agence INT IDENTITY(1,1),
	agence NVARCHAR(64) NOT NULL,
	PRIMARY KEY(id_agence)
);


------------------------TABLE GERANT ENUM POUR DESTINATIONS----------------------------

CREATE TABLE Continents(
	continent NVARCHAR(16) NOT NULL, --mettre six lignes Afrique, Amerique, Antarctique, Asie, Europe, Océanie
	PRIMARY KEY(continent)
);
INSERT INTO Continents (continent) VALUES ('Afrique');
INSERT INTO Continents (continent) VALUES ('Amerique');
INSERT INTO Continents (continent) VALUES ('Antarctique');
INSERT INTO Continents (continent) VALUES ('Asie');
INSERT INTO Continents (continent) VALUES ('Europe');
INSERT INTO Continents (continent) VALUES ('Océanie');

------------------------TABLE DESTINATIONS----------------------
CREATE TABLE Destinations(
	id_destination INT IDENTITY(1,1),
	continent NVARCHAR(16) NOT NULL, --FK
	pays NVARCHAR(32) NOT NULL,
	region NVARCHAR(32),
	descriptif NVARCHAR(MAX),
	PRIMARY KEY(id_destination)
);

ALTER TABLE Destinations ADD CONSTRAINT Fk_2Continents FOREIGN KEY(continent) REFERENCES Continents(continent);

------------------------TABLE VOYAGES---------------------------
CREATE TABLE Voyages(
	id_voyage INT IDENTITY(1,1),
	[date aller] DATE NOT NULL,
	[date retour] DATE NOT NULL,
	[places disponibles] INT NOT NULL,
	[tarif tout compris] MONEY NOT NULL,
	agence INT NOT NULL, --FK
	destination INT NOT NULL, --FK
	PRIMARY KEY(id_voyage)
);

ALTER TABLE Voyages ADD CONSTRAINT Fk_3Destination FOREIGN KEY(destination) REFERENCES Destinations(id_destination);
ALTER TABLE Voyages ADD CONSTRAINT Fk_4Agences FOREIGN KEY(agence) REFERENCES Agences(id_agence);


------------------------TABLE ASSURANCES--------------------------
CREATE TABLE Assurances(
	id_assurance INT IDENTITY(1,1),
	libelle NVARCHAR(64) NOT NULL,
	prix FLOAT NOT NULL,-- on mettra 1 pour les assurances non définies et un pourcentage supérieur à 1 qui multipliera le prix total du dossier
	descriptif NVARCHAR(MAX),
	PRIMARY KEY(id_assurance)
);

------------------------TABLES GERANT ENUM POUR DOSSIERS----------------------------

CREATE TABLE [Raisons Annulations](
annulation_raison NVARCHAR(32)NOT NULL, -- mettre trois lignes Clients ou places Insuffisantes ou aucune
PRIMARY KEY(annulation_raison)
);
INSERT INTO [Raisons Annulations] (annulation_raison) VALUES ('Client');
INSERT INTO [Raisons Annulations] (annulation_raison) VALUES ('Places Insuffisantes');



CREATE TABLE [Etats Dossiers](
etat_dossier NVARCHAR(16)NOT NULL, --mettre quatre lignes 'enAttente' OR 'enCours' OR 'refusee' OR 'acceptee'
PRIMARY KEY(etat_dossier)
);
INSERT INTO [Etats Dossiers] (etat_dossier) VALUES ('en attente');
INSERT INTO [Etats Dossiers] (etat_dossier) VALUES ('en cours');
INSERT INTO [Etats Dossiers] (etat_dossier) VALUES ('refusee');
INSERT INTO [Etats Dossiers] (etat_dossier) VALUES ('acceptee');


------------------------TABLE DOSSIERS----------------------------
CREATE TABLE Dossiers(
	id_dossier INT IDENTITY(1,1),
	[numero carte bancaire] NVARCHAR(32) NOT NULL,
	[raison annulation] NVARCHAR(32), --FK
	etat NVARCHAR(16) NOT NULL, --FK
	voyage INT NOT NULL, --FK
	[numero client] INT NOT NULL, --FK
	[dernier suivi] DATE, ---- date dernier changement d'etat
	PRIMARY KEY(id_dossier)
);

ALTER TABLE Dossiers ADD CONSTRAINT Fk_5Annulation FOREIGN KEY([raison annulation]) REFERENCES [Raisons Annulations](annulation_raison);
ALTER TABLE Dossiers ADD CONSTRAINT Fk_6EtatDossier FOREIGN KEY(etat) REFERENCES [Etats Dossiers](etat_dossier);
ALTER TABLE Dossiers ADD CONSTRAINT Fk_7Client FOREIGN KEY([numero client]) REFERENCES Personnes(id_personne);
ALTER TABLE Dossiers ADD CONSTRAINT Fk_8Voyage FOREIGN KEY(voyage) REFERENCES Voyages(id_voyage) ON DELETE CASCADE;


------------------------TABLE [Liste Assurances] -------------------------
CREATE TABLE [Liste Assurances](
	assurance INT NOT NULL, --FK
	dossier INT NOT NULL, --FK
);

ALTER TABLE [Liste Assurances] ADD CONSTRAINT Fk_5Assurance FOREIGN KEY(assurance) REFERENCES Assurances(id_assurance);
ALTER TABLE [Liste Assurances] ADD CONSTRAINT Fk_6Dossier FOREIGN KEY(dossier) REFERENCES Dossiers(id_dossier) ON DELETE CASCADE;

-----------------------TABLE [Liste Participants]--------------------------
CREATE TABLE [Liste Participants](
	participant INT NOT NULL, --FK
	dossier INT NOT NULL, --FK
);

ALTER TABLE [Liste Participants] ADD CONSTRAINT Fk_6Personne FOREIGN KEY(participant) REFERENCES Personnes(id_personne);
ALTER TABLE [Liste Participants] ADD CONSTRAINT Fk_7Dossier FOREIGN KEY(dossier) REFERENCES Dossiers(id_dossier) ON DELETE CASCADE;

---------------------VISUALISATION DES TABLES-------------------------------------
--SELECT * FROM ;
SELECT * FROM Statuts;
SELECT * FROM Authentifications;
SELECT * FROM Civilites;
SELECT * FROM Personnes;
SELECT * FROM Agences;
SELECT * FROM Continents;
SELECT * FROM Destinations;
SELECT * FROM Voyages;
SELECT * FROM Assurances;
SELECT * FROM [Raisons Annulations];
SELECT * FROM [Etats Dossiers];
SELECT * FROM Dossiers;
SELECT * FROM [Liste Assurances];
SELECT * FROM [Liste Participants];


--################  EXEMPLE INSERT ##################


----------------------------------------------------------------------------------------------------
/*

INSERT INTO Authentification (Email, MotdePasse, Statut) VALUES ('sodales.elit@elitdictum.com', CONVERT(varbinary,'2598lalala'),'Client');
INSERT INTO Authentification (Email, MotdePasse, Statut) VALUES ('bibendum@elitEtiam.ca', CONVERT(varbinary,'pasmoipoix'),'commercial');
INSERT INTO Authentification (Email, MotdePasse, Statut) VALUES ('pellentesque@sitametrisus.ca', CONVERT(varbinary,'cestamoi!!!'),'marketting');

INSERT INTO Personnes(Civilite,Nom,Prenom,DateNaissance,Adresse,Telephone,Email,Client,Participant) VALUES('M','Beck','While''mina','17/02/2009','Ap #245-2016 Duis St.','0378588069','sodales.elit@elitdictum.com',0, 0);
INSERT INTO Personnes(Civilite,Nom,Prenom,DateNaissance,Adresse,Telephone,Email,Client,Participant) VALUES('M','Chang','Meredith','09/12/1967','Ap #878-9210 Pharetra Av.','0692930681','bibendum@elitEtiam.ca',1, 0);
INSERT INTO Personnes(Civilite,Nom,Prenom,DateNaissance,Adresse,Telephone,Email,Client,Participant) VALUES('M','Morrison','Joy','15/05/1946','P.O. Box 472, 8823 Etiam Avenue','0604220689','pellentesque@sitametrisus.ca',1, 0);
*/

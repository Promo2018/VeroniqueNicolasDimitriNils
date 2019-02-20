
--DROP DATABASE BoVoyage_VNND;
--CREATE DATABASE BoVoyage_VNND;
USE BoVoyage_VNND;
--use Northwind;
/*
DROP TABLE [Liste Participants];
DROP TABLE [Liste Assurances];
DROP TABLE Dossiers;
DROP TABLE [Etats Dossiers];
DROP TABLE [Raisons Annulations];
DROP TABLE Assurances;
DROP TABLE Voyages;
DROP TABLE Personnes;
DROP TABLE Destinations;
DROP TABLE Continents;
DROP TABLE Agences;
DROP TABLE Civilites;
DROP TABLE Authentifications;
DROP TABLE Statuts;
DROP TABLE OuisNons;
*/

------------------------TABLE OUI/NON ENUM POUR PERSONNES (client et participant) ----------------------------
CREATE TABLE OuisNons(
	id_ouinon int identity NOT NULL,
	valeur NVARCHAR(3) UNIQUE NOT NULL, 
	PRIMARY KEY (id_ouinon)
);
INSERT INTO OuisNons VALUES ('Non');
INSERT INTO OuisNons VALUES ('Oui');

update OuisNons set valeur='Non' where id_ouinon=1;
update OuisNons set valeur='Oui' where id_ouinon=2;



------------------------TABLE GERANT ENUM POUR AUTHENTIFICATIONS----------------------------
CREATE TABLE Statuts(
	id_statut int identity NOT NULL,
	statut NVARCHAR(16) UNIQUE NOT NULL, -- mettre les 4 choix Client, Administrateur, Commercial, Marketting
	PRIMARY KEY (id_statut),
);
INSERT INTO Statuts (statut) VALUES ('Client');
INSERT INTO Statuts (statut) VALUES ('Commercial');
INSERT INTO Statuts (statut) VALUES ('Marketing');
INSERT INTO Statuts (statut) VALUES ('Administrateur');

------------------------TABLE AUTHENTIFICATIONS-------------------------
CREATE TABLE Authentifications (
    email NVARCHAR(64) UNIQUE NOT NULL,
	[mot de passe] NVARCHAR(64) NOT NULL,
    statut int NOT NULL, --FK
	PRIMARY KEY (email)
);
ALTER TABLE Authentifications ADD CONSTRAINT Fk_0Authentifications FOREIGN KEY(statut) REFERENCES Statuts(id_statut);

------------------------TABLE GERANT ENUM POUR PERSONNES ----------------------------
CREATE TABLE Civilites(
	civilite NVARCHAR(8) UNIQUE NOT NULL, --mettre deux lignes M ou Mme 
	PRIMARY KEY (civilite),
);
INSERT INTO Civilites (civilite) VALUES ('M');
INSERT INTO Civilites (civilite) VALUES ('Mme');

------------------------TABLE PERSONNES-------------------------
CREATE TABLE Personnes(
    id_personne INT IDENTITY,
    civilite NVARCHAR(8) NOT NULL, --FK
    prenom NVARCHAR(32) NOT NULL,
	nom NVARCHAR(32) NOT NULL,  
	adresse NVARCHAR(230) NOT NULL,
	telephone NVARCHAR(32) NOT NULL,  
	[date naissance] DATE NOT NULL,
	client INT default 1, -- FK
	participant INT default 1, --FK
	email NVARCHAR(64) UNIQUE, -- à faire trigger pour contrainte non null si client true
	PRIMARY KEY(id_personne)
);

ALTER TABLE Personnes ADD CONSTRAINT Fk_1Civilites FOREIGN KEY(civilite) REFERENCES Civilites(civilite);
ALTER TABLE Personnes ADD CONSTRAINT Fk_Client FOREIGN KEY(client) REFERENCES OuisNons(id_ouinon);
ALTER TABLE Personnes ADD CONSTRAINT Fk_Participant FOREIGN KEY(participant) REFERENCES OuisNons(id_ouinon);

------------------------TABLE AGENCES-------------------------------

CREATE TABLE Agences(
	id_agence INT IDENTITY,
	agence NVARCHAR(64) NOT NULL unique,
	PRIMARY KEY(id_agence)
);


------------------------TABLE GERANT ENUM POUR DESTINATIONS----------------------------

CREATE TABLE Continents(
	continent NVARCHAR(16) NOT NULL, --mettre six lignes Afrique, Amerique, Antarctique, Asie, Europe, Océanie
	PRIMARY KEY(continent)
);
INSERT INTO Continents (continent) VALUES ('Afrique');
INSERT INTO Continents (continent) VALUES ('Amerique du Nord');
INSERT INTO Continents (continent) VALUES ('Amerique du Sud');
INSERT INTO Continents (continent) VALUES ('Asie');
INSERT INTO Continents (continent) VALUES ('Europe');
INSERT INTO Continents (continent) VALUES ('Océanie');

------------------------TABLE DESTINATIONS----------------------
CREATE TABLE Destinations(
	id_destination INT IDENTITY,
	continent NVARCHAR(16) NOT NULL, --FK
	pays NVARCHAR(32) NOT NULL,
	region NVARCHAR(32),
	descriptif NVARCHAR(MAX),
	PRIMARY KEY(id_destination)
);

ALTER TABLE Destinations ADD CONSTRAINT Fk_2Continents FOREIGN KEY(continent) REFERENCES Continents(continent);

------------------------TABLE VOYAGES---------------------------
CREATE TABLE Voyages(
	id_voyage INT IDENTITY,
	[date aller] DATE NOT NULL,
	[date retour] DATE NOT NULL,
	[places disponibles] INT NOT NULL,
	[tarif tout compris] decimal(19,4) NOT NULL,
	agence INT NOT NULL, --FK
	destination INT NOT NULL, --FK
	PRIMARY KEY(id_voyage)
);

ALTER TABLE Voyages ADD CONSTRAINT Fk_3Destination FOREIGN KEY(destination) REFERENCES Destinations(id_destination);
ALTER TABLE Voyages ADD CONSTRAINT Fk_4Agences FOREIGN KEY(agence) REFERENCES Agences(id_agence);


------------------------TABLE ASSURANCES--------------------------
CREATE TABLE Assurances(
	id_assurance INT IDENTITY(1,1),
	libelle NVARCHAR(64) NOT NULL unique,
	prix FLOAT NOT NULL default 0,-- on mettra 1 pour les assurances non définies et un pourcentage supérieur à 1 qui multipliera le prix total du dossier
	descriptif NVARCHAR(MAX),
	PRIMARY KEY(id_assurance)
);

------------------------TABLES GERANT ENUM POUR DOSSIERS----------------------------

CREATE TABLE [Raisons Annulations](
annulation_raison NVARCHAR(32)NOT NULL, -- mettre trois lignes Clients ou places Insuffisantes ou aucune
PRIMARY KEY(annulation_raison)
);
INSERT INTO [Raisons Annulations] (annulation_raison) VALUES (' ');
INSERT INTO [Raisons Annulations] (annulation_raison) VALUES ('Client');
INSERT INTO [Raisons Annulations] (annulation_raison) VALUES ('Places Insuffisantes');



CREATE TABLE [Etats Dossiers](
id_etat int identity not null,
etat NVARCHAR(16)NOT NULL, --mettre quatre lignes 'enAttente' OR 'enCours' OR 'refusee' OR 'acceptee'
PRIMARY KEY(id_etat)
);
INSERT INTO [Etats Dossiers] (etat) VALUES ('en attente');
INSERT INTO [Etats Dossiers] (etat) VALUES ('en cours');
INSERT INTO [Etats Dossiers] (etat) VALUES ('refusé');
INSERT INTO [Etats Dossiers] (etat) VALUES ('accepté');

UPDATE [Etats Dossiers] set etat='En Attente' where id_etat=1;
UPDATE [Etats Dossiers] set etat='En Cours' where id_etat=2;
UPDATE [Etats Dossiers] set etat='Refusé' where id_etat=3;
UPDATE [Etats Dossiers] set etat='Accepté' where id_etat=4;


------------------------TABLE DOSSIERS----------------------------
CREATE TABLE Dossiers(
	id_dossier INT IDENTITY,
	[numero carte bancaire] NVARCHAR(32) NOT NULL,
	[raison annulation] NVARCHAR(32) default null, --FK
	etat int NOT NULL default 1, --FK
	voyage INT NOT NULL, --FK
	client INT NOT NULL, --FK
	[dernier suivi] DATE default getdate(), ---- date dernier changement d'etat
	PRIMARY KEY(id_dossier)
);

ALTER TABLE Dossiers ADD CONSTRAINT Fk_5Annulation FOREIGN KEY([raison annulation]) REFERENCES [Raisons Annulations](annulation_raison);
ALTER TABLE Dossiers ADD CONSTRAINT Fk_6EtatDossier FOREIGN KEY(etat) REFERENCES [Etats Dossiers](id_etat);
ALTER TABLE Dossiers ADD CONSTRAINT Fk_7Client FOREIGN KEY(client) REFERENCES Personnes(id_personne);
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



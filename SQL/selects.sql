-- use Northwind;
use BoVoyage_VNND;

-- TABLES MODEL -- 

select * from Agences;
select * from Assurances;
select * from Destinations;
select * from Authentifications;
select * from Voyages;
select * from Personnes;
select * from Dossiers;

--TABLES INTERMEDIAIRES --

select * from [Liste Assurances];
select * from [Liste Participants];

-- ENUMERATIONS --
select * from Civilites;
select * from Continents;
select * from [Etats Dossiers];
select * from [Raisons Annulations];
select * from Statuts;
select * from OuisNons;
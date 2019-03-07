--use northwind;
use BoVoyage_VNND;

--Agences

insert into Agences (agence) values ('Krajcik LLC');
insert into Agences (agence) values ('Zulauf, Hettinger and Haag');
insert into Agences (agence) values ('Wyman LLC');
insert into Agences (agence) values ('Goyette, McDermott and Mohr');
insert into Agences (agence) values ('Schimmel LLC');
insert into Agences (agence) values ('Jacobs Group');
insert into Agences (agence) values ('Gusikowski, Thiel and Bailey');
insert into Agences (agence) values ('Schneider, Herman and Stamm');
insert into Agences (agence) values ('Murazik Group');
insert into Agences (agence) values ('Graham-Strosin');

--Assurances

insert into assurances (libelle, prix, descriptif) values ('Annulation', 0.1, 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
;

--Destinations

insert into Destinations (continent, pays, region, descriptif) values (4, 'Indonesia', 'Caprifoliaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (3, 'Brazil', 'Scrophulariaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (5, 'Poland', 'Fagaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (3, 'Brazil', 'Hydrophyllaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (5, 'Croatia', 'Apocynaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (4, 'China', 'Asteraceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (5, 'Portugal', 'Salicaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (4, 'Russia', 'Asteraceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (4, 'Indonesia', 'Rutaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');
insert into Destinations (continent, pays, region, descriptif) values (6, 'United States', 'Juncaceae', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. ');

--Authentifications

insert into Authentifications (email, [mot de passe], statut) values ('knairns0@si.edu', 'CkGDJzA', 1);
insert into Authentifications (email, [mot de passe], statut) values ('ksimenel1@discuz.net', 'iscKl9Ebevd', 2);
insert into Authentifications (email, [mot de passe], statut) values ('phusby2@netlog.com', 'KYvBSw2', 4);
insert into Authentifications (email, [mot de passe], statut) values ('lbalsillie3@live.com', 'gniF3c5lrIRO', 1);
insert into Authentifications (email, [mot de passe], statut) values ('osnailham4@weather.com', 'lIkYUgyfE', 2);
insert into Authentifications (email, [mot de passe], statut) values ('clulham5@canalblog.com', 'XctjAM', 3);
insert into Authentifications (email, [mot de passe], statut) values ('fpiecha6@ow.ly', 'b9cjZzGVUM5', 1);
insert into Authentifications (email, [mot de passe], statut) values ('kwilkins7@sphinn.com', '1dW2qenbADd', 2);
insert into Authentifications (email, [mot de passe], statut) values ('crameau8@blinklist.com', 'oQQhUTGmUdxe', 3);
insert into Authentifications (email, [mot de passe], statut) values ('dmoodie9@ameblo.jp', 'esHbbJ', 1);

--Voyages

insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('28/02/2019','29/03/2019',23, 264.36, 2, 7);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('06/03/2019','23/04/2019',23, 415.01, 10, 10);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('03/03/2019','15/06/2019',24, 515.83, 10, 9);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('09/03/2019','05/06/2019',5, 436.94, 1, 8);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('22/02/2019','16/05/2019',34, 347.22, 6, 4);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('23/02/2019','17/05/2019',26, 405.45, 3, 2);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('09/03/2019','17/03/2019',26, 450.03, 2, 10);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('15/02/2019','18/05/2019',6, 445.74, 9, 6);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('15/03/2019','09/06/2019',35, 612.01, 1, 1);
insert into Voyages ([date aller], [date retour], [places disponibles], [tarif tout compris], agence, destination) values ('04/03/2019','03/04/2019',16, 277.89, 10, 5);

--Personnes

insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (2, 'Perree', 'Abigael', '371 Bellgrove Terrace', '971-697-8888', '2012-08-10', 'aperree0@fotki.com');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (1, 'Wetwood', 'Onfre', '78 Burning Wood Way', '689-705-8702', '1961-03-02', 'owetwood1@gmpg.org');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (1, 'Wiz', 'Thornie', '330 Forest Court', '860-462-0184', '2012-06-14', 'twiz2@cbsnews.com');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (2, 'Cow', 'Amity', '18512 Grayhawk Pass', '384-682-0554', '1965-05-05', 'acow3@domainmarket.com');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (1, 'Propper', 'Kalil', '70163 Starling Parkway', '280-952-5248', '1966-08-14', 'kpropper4@theglobeandmail.com');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (2, 'Bamell', 'Melany', '5306 Vera Point', '684-661-5638', '2014-07-09', 'mbamell5@unesco.org');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (1, 'Caslin', 'Chiquita', '5689 Bay Center', '913-280-0958', '2000-04-30', 'ccaslin6@elegantthemes.com');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (1, 'Melonby', 'Silvain', '68 Bultman Place', '207-948-2670', '1976-05-02', 'smelonby7@twitter.com');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (2, 'Monument', 'Delora', '9115 Corben Place', '789-114-5754', '1964-07-16', 'dmonument8@bloglovin.com');
insert into Personnes (civilite, nom, prenom, adresse, telephone, [date naissance], email) values (1, 'Belchem', 'Amil', '96 Sage Trail', '786-516-7635', '1998-07-26', 'abelchem9@indiatimes.com');

--Dossiers

insert into Dossiers ([numero carte bancaire],  voyage, client) values ('5448982854595582',  1, 2 );
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('3582945378359752', 2, 9);
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('3541934982748261', 3, 8);
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('374622547053370',  4, 7);
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('6331104585972999468', 5, 8);
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('5610499123419866867',  6, 9);
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('4017953356060',7, 4);
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('3549568678640606', 8, 3);
insert into Dossiers ([numero carte bancaire],  voyage, client) values ('3587552955431577', 9, 8);
insert into Dossiers ([numero carte bancaire],  voyage, client ) values ('3588063986738492', 10, 3);


--[Liste Assurances]

insert into [Liste Assurances] (assurance, dossier) values (1, 4);
insert into [Liste Assurances] (assurance, dossier) values (1, 10);
insert into [Liste Assurances] (assurance, dossier) values (1, 5);
insert into [Liste Assurances] (assurance, dossier) values (1, 9);
insert into [Liste Assurances] (assurance, dossier) values (1, 7);
insert into [Liste Assurances] (assurance, dossier) values (1, 6);

----[Liste Participants]

insert into [Liste Participants] (participant, dossier) values (9, 7);
insert into [Liste Participants] (participant, dossier) values (8, 7);
insert into [Liste Participants] (participant, dossier) values (2, 10);
insert into [Liste Participants] (participant, dossier) values (7, 10);
insert into [Liste Participants] (participant, dossier) values (4, 7);
insert into [Liste Participants] (participant, dossier) values (10, 6);
insert into [Liste Participants] (participant, dossier) values (5, 3);
insert into [Liste Participants] (participant, dossier) values (5, 7);
insert into [Liste Participants] (participant, dossier) values (10, 10);
insert into [Liste Participants] (participant, dossier) values (7, 6);








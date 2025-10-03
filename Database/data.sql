use kozmeticki_salon_hci;

INSERT INTO mjesto(BrojPoste, Naziv) VALUES (78000, 'Banja Luka');
INSERT INTO mjesto(BrojPoste, Naziv) VALUES (79101, 'Prijedor');
INSERT INTO mjesto(BrojPoste, Naziv) VALUES (74000, 'Doboj');
INSERT INTO mjesto(BrojPoste, Naziv) VALUES (78400, 'Gradiska');

INSERT INTO cjenovnik(DatumObjavljivanja) VALUES(sysdate());
INSERT INTO cjenovnik(DatumObjavljivanja) VALUES('2024-05-20');
INSERT INTO cjenovnik(DatumObjavljivanja) VALUES('2024-06-19');
INSERT INTO cjenovnik(DatumObjavljivanja) VALUES('2024-05-20');
INSERT INTO cjenovnik(DatumObjavljivanja) VALUES('2024-07-25');

INSERT INTO kozmeticki_salon(Naziv, Adresa, RadnoVrijeme, MJESTO_BrojPoste, Cjenovnik_IdCjenovnika, Telefon) 
VALUES ('Beauty studio', 'Stepe Stepanovica 16', 'PONEDJELJAK-PETAK: 09:00-20:00\nSUBOTA: 09:00-18:00\nNEDJELJA: NERADNA', 78000, 1, '+387 (0)66 452 111');
INSERT INTO kozmeticki_salon(Naziv, Adresa, RadnoVrijeme, MJESTO_BrojPoste, Cjenovnik_IdCjenovnika, Telefon) 
VALUES ('Coco studio', 'Carice Milice 39', 'PONEDJELJAK-PETAK: 09:00-20:00\nSUBOTA: 10:00-16:00\nNEDJELJA: NERADNA', 78000, 2, '+387 (0)66 452 222');
INSERT INTO kozmeticki_salon(Naziv, Adresa, RadnoVrijeme, MJESTO_BrojPoste, Cjenovnik_IdCjenovnika, Telefon) 
VALUES ('Butterfly', 'Kozarska 20', 'PONEDJELJAK-PETAK: 09:00-20:00\nSUBOTA: 10:00-16:00\nNEDJELJA: 10:00-16:00', 79101, 3, '+387 (0)66 452 333');
INSERT INTO kozmeticki_salon(Naziv, Adresa, RadnoVrijeme, MJESTO_BrojPoste, Cjenovnik_IdCjenovnika, Telefon) 
VALUES ('Mia', 'Gavrila Principa 14', 'PONEDJELJAK-PETAK: 09:00-20:00\nSUBOTA: 09:00-18:00\nNEDJELJA: NERADNA', 74000, 4, '+387 (0)66 452 444');

INSERT INTO tip_usluge(Naziv) VALUES ('Manikir'); 
INSERT INTO tip_usluge(Naziv) VALUES ('Pedikir'); 
INSERT INTO tip_usluge(Naziv) VALUES ('Masaza'); 
INSERT INTO tip_usluge(Naziv) VALUES ('Sminkanje'); 
INSERT INTO tip_usluge(Naziv) VALUES ('Trajna sminka'); 
INSERT INTO tip_usluge(Naziv) VALUES ('Trepavice'); 
INSERT INTO tip_usluge(Naziv) VALUES ('Obrve'); 

INSERT INTO smjena(Naziv, Od, Do, KOZMETICKI_SALON_IdKozmetickogSalona) VALUES ('Prva smjena', '09:00:00', '15:00:00', 2);
INSERT INTO smjena(Naziv, Od, Do, KOZMETICKI_SALON_IdKozmetickogSalona) VALUES ('Druga smjena', '15:00:00', '21:00:00', 2);
INSERT INTO smjena(Naziv, Od, Do, KOZMETICKI_SALON_IdKozmetickogSalona) VALUES ('Medjusmjena', '12:00:00', '18:00:00', 2);

INSERT INTO smjena(Naziv, Od, Do, KOZMETICKI_SALON_IdKozmetickogSalona) VALUES ('Prva smjena', '09:00:00', '15:00:00', 1);
INSERT INTO smjena(Naziv, Od, Do, KOZMETICKI_SALON_IdKozmetickogSalona) VALUES ('Druga smjena', '15:00:00', '21:00:00', 1);
INSERT INTO smjena(Naziv, Od, Do, KOZMETICKI_SALON_IdKozmetickogSalona) VALUES ('Medjusmjena', '12:00:00', '18:00:00', 1);

-- DELETE FROM korisnik WHERE IdKorisnika>0;
-- DELETE FROM zaposleni WHERE KORISNIK_IdKorisnika>0;

-- ALTER TABLE KORISNIK AUTO_INCREMENT = 1;

INSERT INTO korisnik(KorisnickoIme, Lozinka, EmailAdresa, TipNaloga, Ime, Prezime) 
VALUES ('markov', 'marko1234', 'markov@mail.com', 'Manager', 'Marko', 'Vukic'),
       ('mia', 'mia1234', 'mia@mail.com', 'Manager', 'Mia', 'Malic'),
       ('aleks', 'aleks1234', 'aleks@mail.com', 'Manager', 'Aleks', 'Aleksic'),
       ('iva', 'iva1234', 'iva@mail.com', 'Manager', 'Iva', 'Ivanovic'),
       ('anjam', 'anja', 'anjam@mail.com', 'Client', 'Anja', 'Markovic'),
       ('dajanav', 'dajana', 'dajanav@mail.com', 'Client', 'Dajana', 'Vukovic'),
       ('majam', '1234', 'majam@mail.com', 'Client', 'Maja', 'Markovic'),
       ('andrea', 'andrea1', 'andrea1@mail.com', 'Beautician', 'Andrea', 'Andric'),
       ('milam', 'mila', 'milam@mail.com', 'Beautician', 'Mila', 'Milic'),
       ('anaanic', 'ana', 'ana@mail.com', 'Beautician', 'Ana', 'Anic'),
       ('mirak', 'mira', 'mirak@mail.com', 'Beautician', 'Mira', 'Katic'),
       ('vanjav', 'vanja', 'vanjav@mail.com', 'Beautician', 'Vanja', 'Vukovic'),
       ('dijanad', 'dijana', 'dijanad@mail.com', 'Beautician', 'Dijana', 'Dulic'),
       ('ivanai', 'ivana', 'ivanai@mail.com', 'Beautician', 'Ivana', 'Ivanovic'),
       ('andjelaa', 'andjela', 'andjelaa@mail.com', 'Beautician', 'Andjela', 'Aleksic'),
       ('tarat', 'tara', 'tarat@mail.com', 'Beautician', 'Tara', 'Topic'),
       ('nadjan', 'nadja', 'nadjan@mail.com', 'Beautician', 'Nadja', 'Nedic'),
       ('laral', 'lara', 'laral@mail.com', 'Beautician', 'Lara', 'Lolic');
       
INSERT INTO zaposleni(Adresa, DatumZaposlenja, Plata, KORISNIK_IdKorisnika, KOZMETICKI_SALON_IdKozmetickogSalona) 
VALUES ('Majke Jugovica 12, Banja Luka', '2024-03-20', 1500.00, 1, 1),
       ('Gavrila Principa 14, Gradiska', '2024-04-20', 1700.00, 2, 2),
       ('Carice Milice 39, Banja Luka', '2024-03-20', 1550.00, 3, 3),
       ('Kozarska 16, Prijedor', '2024-02-01', 1400.00, 4, 4),
       ('Kozarska 4, Prijedor', '2024-03-01', 1150.50, 8, 3),
       ('Zanatska 5, Prijedor', '2024-03-03', 1145.00, 9, 3),
       ('Zanatska 20, Prijedor', '2024-03-03', 1150.50, 10, 3),
       ('Gavrila Principa 16, Gradiska', '2024-01-10', 1200.00, 11, 4),
       ('Prvomajska 25, Gradiska', '2024-01-10', 1200.00, 12, 4),
       ('Prvomajska 11, Gradiska', '2024-03-15', 1100.00, 13, 4),
       ('Ive Andrica 12, Banja Luka', '2024-03-21', 1300.00, 14, 2),
       ('Macvanska 5, Banja Luka', '2024-02-20', 1380.50, 15, 2),
       ('Vilsonova 9, Banja Luka', '2024-05-10', 1270.00, 16, 2),
       ('Jovana Raskovica 70, Banja Luka', '2024-05-07', 1300.00, 17, 1),
       ('Ranka Sipke 3, Banja Luka', '2024-01-11', 1310.00, 18, 1);
       
INSERT INTO kozmeticar(ZAPOSLENI_KORISNIK_IdKorisnika) 
VALUES (8), (9), (10), (11), (12), (13), (14), (15), (16), (17), (18); 

INSERT INTO menadzer(ZAPOSLENI_KORISNIK_IdKorisnika) 
VALUES (1), (2), (3), (4); 


INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Lakiranje noktiju', 18.00, '00:20:00', 'Standardno lakiranje noktiju.', 1, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Lakiranje noktiju', 17.00, '00:20:00', 'Standardno lakiranje noktiju.', 3, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Lakiranje noktiju', 15.00, '00:20:00', 'Standardno lakiranje noktiju.', 4, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Lakiranje noktiju', 15.00, '00:20:00', 'Standardno lakiranje noktiju.', 2, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Trajni lak', 20.00, '00:30:00', 'Lakiranje noktiju trajnim lakom.', 1, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Trajni lak', 20.00, '00:30:00', 'Lakiranje noktiju trajnim lakom.', 3, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Trajni lak', 20.00, '00:30:00', 'Lakiranje noktiju trajnim lakom.', 4, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Trajni lak', 20.00, '00:30:00', 'Lakiranje noktiju trajnim lakom.', 2, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Gel na prirodne nokte', 25.00, '00:45:00', 'Lakiranje noktiju trajnim lakom.', 1, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Gel na prirodne nokte', 24.00, '00:40:00', 'Lakiranje noktiju trajnim lakom.', 3, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Gel na prirodne nokte', 22.00, '00:40:00', 'Lakiranje noktiju trajnim lakom.', 4, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Gel na prirodne nokte', 22.00, '00:45:00', 'Lakiranje noktiju trajnim lakom.', 2, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja noktiju', 40.00, '01:30:00', 'Nadogradnja noktiju tipsama.', 1, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja noktiju', 50.00, '01:45:00', 'Izlijevanje noktiju.', 1, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja noktiju', 40.00, '01:30:00', 'Nadogradnja noktiju tipsama.', 3, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja noktiju', 35.00, '02:00:00', 'Nadogradnja noktiju tipsama.', 4, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja noktiju', 30.00, '01:45:00', 'Nadogradnja noktiju tipsama.', 2, 1);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja noktiju', 40.00, '02:00:00', 'Izlijevanje noktiju.', 2, 1);

INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Pedikir', 25.00, '01:00:00', 'Standardni pedikir.', 1, 2);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Pedikir', 30.00, '01:00:00', 'Standardni pedikir.', 3, 2);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Pedikir', 20.00, '01:00:00', 'Standardni pedikir.', 4, 2);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Pedikir', 20.00, '01:00:00', 'Standardni pedikir.', 2, 2);

INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza lica', 20.00, '00:20:00', 'Masaza za smanjenje napetosti misica lica.', 1, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza lica', 30.00, '00:30:00', 'Masaza za hranjenje i hidratiziranje koze lica.', 1, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza lica', 30.00, '00:20:00', 'Masaza protiv bora na licu.', 1, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza lica', 20.00, '00:15:00', 'Masaza za smanjenje napetosti misica lica.', 3, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza lica', 20.00, '00:15:00', 'Masaza protiv bora na licu.', 3, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza lica', 20.00, '00:20:00', 'Masaza za smanjenje napetosti misica lica.', 4, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza lica', 20.00, '00:20:00', 'Masaza za smanjenje napetosti misica lica.', 2, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza ledja', 20.00, '00:20:00', 'Relax masaza ledja.', 1, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza ledja', 40.00, '00:40:00', 'Medicinska masaza ledja.', 1, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza ledja', 40.00, '00:30:00', 'Sportska masaza.', 1, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza ledja', 20.00, '00:20:00', 'Relax masaza ledja.', 3, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza ledja', 20.00, '00:20:00', 'Relax masaza ledja.', 4, 3);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Masaza ledja', 20.00, '00:20:00', 'Relax masaza ledja.', 2, 3);

INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje', 50.00, '01:00:00', 'Standardno sminkanje.', 1, 4);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje', 50.00, '01:00:00', 'Standardno sminkanje.', 3, 4);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje', 40.00, '01:00:00', 'Standardno sminkanje.', 4, 4);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje', 40.00, '01:00:00', 'Standardno sminkanje.', 2, 4);

INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje za posebne prilike', 70.00, '01:30:00', 'Napredno sminkanje za specijalne prilike.', 1, 4);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje za posebne prilike', 75.00, '01:30:00', 'Napredno sminkanje za specijalne prilike.', 3, 4);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje za posebne prilike', 50.00, '01:30:00', 'Napredno sminkanje za specijalne prilike.', 4, 4);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Sminkanje za posebne prilike', 60.00, '01:30:00', 'Napredno sminkanje za specijalne prilike.', 2, 4);

INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Usne', 400.00, '01:30:00', 'Trajna sminka usana.', 1, 5);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Trajni ajlajner', 300.00, '01:00:00', 'Napredno sminkanje za specijalne prilike.', 1, 5);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Usne', 380.00, '01:30:00', 'Trajna sminka usana.', 3, 5);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Usne', 350.00, '01:30:00', 'Trajna sminka usana.', 4, 5);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Usne', 360.00, '01:30:00', 'Trajna sminka usana.', 2, 5);

INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja trepavica', 100.00, '00:30:00', 'Nadogradnja svilenih trepavica.', 1, 6);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Nadogradnja trepavica', 120.00, '00:45:00', 'Nadogradnja ruskoh trepavica.', 1, 6);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Korekcija nadogradnje trepavica', 60.00, '00:30:00', 'Korekcija nakon mjesec dana.', 1, 6);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Uvijanje trepavica', 60.00, '00:30:00', 'Uvijanje i farbanje prirodnih trepavica.', 1, 6);

INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Cupanje obrva', 12.00, '00:15:00', 'Cupanje obrva pincetom.', 1, 7);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Cupanje obrva', 10.00, '00:10:00', 'Cupanje obrva voskom.', 1, 7);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Farbanje obrva', 10.00, '00:10:00', 'Farbanje obrva.', 1, 7);
INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge) VALUES ('Trajne obrve', 250.00, '01:00:00', 'Puder obrve.', 1, 7);

INSERT INTO raspored(MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika, Od, `Do`) VALUES (1, '2025-10-06', '2025-10-12');
INSERT INTO raspored(MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika, Od, `Do`) VALUES (2, '2025-10-06', '2025-10-12');
INSERT INTO raspored(MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika, Od, `Do`) VALUES (3, '2025-10-06', '2025-10-12');
INSERT INTO raspored(MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika, Od, `Do`) VALUES (4, '2025-10-06', '2025-10-12');

INSERT INTO raspored_stavka(DanUSedmici, SMJENA_KOZMETICKI_SALON_IdKozmetickogSalona, SMJENA_Naziv, RASPORED_IdRasporeda, KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika)
VALUES ('Ponedjeljak', 2, 'Prva smjena', 4, 14),
	('Ponedjeljak', 2, 'Druga smjena', 4, 15),
	('Ponedjeljak', 2, 'Medjusmjena', 4, 16),
	('Utorak', 2, 'Prva smjena', 4, 16),
	('Utorak', 2, 'Druga smjena', 4, 15),
	('Utorak', 2, 'Medjusmjena', 4, 14),
	('Srijeda', 2, 'Prva smjena', 4, 15),
	('Srijeda', 2, 'Druga smjena', 4, 16),
	('Srijeda', 2, 'Medjusmjena', 4, 14),
    ('Petak', 2, 'Prva smjena', 4, 15),
	('Petak', 2, 'Druga smjena', 4, 14),
	('Petak', 2, 'Medjusmjena', 4, 16),
	('Subota', 2, 'Medjusmjena', 4, 14),
	('Subota', 2, 'Medjusmjena', 4, 15),
	('Subota', 2, 'Druga smjena', 4, 16);
    
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Hidratantna krema', 'Bogata krema za dubinsku hidrataciju kože, pogodna za sve tipove kože.', 25.99, 1, 1);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Serum za lice s hijaluronskom kiselinom', 'Intenzivni serum za hidrataciju i podmlađivanje kože.', 39.50, 1, 1);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Maska za lice s glinom', 'Prirodna maska koja čisti pore i smanjuje masnoću kože.', 15.75, 1, 1);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Ulje za masažu', 'Relaksirajuće esencijalno ulje za masažu tela s lavandom.', 18.90, 1, 2);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Profesionalni piling za lice', 'Eksfolijant sa sitnim granulama koji uklanja mrtve ćelije kože.', 22.00, 1, 2);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Lak za nokte - crvena boja', 'Dugotrajni gel lak visoke pigmentacije.', 9.99, 1, 3);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Šampon za kosu bez sulfata', 'Blagi šampon koji neguje kosu i ne isušuje teme.', 19.80, 1, 3);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Regenerator za oštećenu kosu', 'Dubinski obnavljajući regenerator s keratinom.', 21.45, 1, 3);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Profesionalna maskara', 'Vodootporna maskara za dugotrajan volumen i uvijene trepavice.', 14.30, 1, 4);
INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika)
VALUES ('Krema za ruke s bademovim uljem', 'Dubinski hrani i omekšava kožu ruku.', 12.00, 1, 4);
INSERT INTO `kozmeticki_salon`.`PROIZVOD` (`Naziv`, `Opis`, `Cijena`, `Dostupnost`, `CJENOVNIK_IdCjenovnika`)
VALUES 
('Hidratantna krema', 'Bogata krema za dubinsku hidrataciju kože, pogodna za sve tipove kože.', 25.99, 1, 1),
('Serum za lice s hijaluronskom kiselinom', 'Intenzivni serum za hidrataciju i podmlađivanje kože.', 39.50, 1, 1),
('Maska za lice s glinom', 'Prirodna maska koja čisti pore i smanjuje masnoću kože.', 15.75, 1, 1),
('Ulje za masažu', 'Relaksirajuće esencijalno ulje za masažu tela s lavandom.', 18.90, 1, 2),
('Profesionalni piling za lice', 'Eksfolijant sa sitnim granulama koji uklanja mrtve ćelije kože.', 22.00, 1, 2),
('Lak za nokte - crvena boja', 'Dugotrajni gel lak visoke pigmentacije.', 9.99, 1, 3),
('Šampon za kosu bez sulfata', 'Blagi šampon koji neguje kosu i ne isušuje teme.', 19.80, 1, 3),
('Regenerator za oštećenu kosu', 'Dubinski obnavljajući regenerator s keratinom.', 21.45, 1, 3),
('Profesionalna maskara', 'Vodootporna maskara za dugotrajan volumen i uvijene trepavice.', 14.30, 1, 4),
('Krema za ruke s bademovim uljem', 'Dubinski hrani i omekšava kožu ruku.', 12.00, 1, 4),
('Depilacijski vosak', 'Prirodni vosak za nežno uklanjanje dlačica s kože.', 17.20, 1, 5),
('Losion za telo s kokosovim uljem', 'Hidratantni losion koji ostavlja kožu svilenkastom.', 16.75, 1, 5),
('Ulje za masažu', 'Relaksirajuće esencijalno ulje za masažu tela s lavandom.', 18.90, 1, 1),
('Profesionalni piling za lice', 'Eksfolijant sa sitnim granulama koji uklanja mrtve ćelije kože.', 22.00, 1, 1),
('Lak za nokte - crvena boja', 'Dugotrajni gel lak visoke pigmentacije.', 9.99, 1, 1),
('Šampon za kosu bez sulfata', 'Blagi šampon koji neguje kosu i ne isušuje teme.', 19.80, 1, 1),
('Regenerator za oštećenu kosu', 'Dubinski obnavljajući regenerator s keratinom.', 21.45, 1, 1),
('Profesionalna maskara', 'Vodootporna maskara za dugotrajan volumen i uvijene trepavice.', 14.30, 1, 1),
('Krema za ruke s bademovim uljem', 'Dubinski hrani i omekšava kožu ruku.', 12.00, 1, 1),
('Depilacijski vosak', 'Prirodni vosak za nežno uklanjanje dlačica s kože.', 17.20, 1, 1),
('Losion za telo s kokosovim uljem', 'Hidratantni losion koji ostavlja kožu svilenkastom.', 16.75, 1, 1),
('Serum za rast obrva i trepavica', 'Formula koja podstiče rast gustih i jakih obrva i trepavica.', 29.95, 1, 1),
('Balzam za usne s medom', 'Prirodni balzam koji štiti i neguje usne.', 6.50, 1, 1),
('Tonik za čišćenje lica', 'Osvježavajući tonik koji uklanja nečistoće i hidrira kožu.', 20.00, 1, 1),
('Sprej za fiksiranje šminke', 'Dugotrajna formula koja učvršćuje šminku.', 15.60, 1, 2),
('Krema za sunčanje SPF 50', 'Visoka zaštita od UV zraka, pogodna za sve tipove kože.', 27.90, 1, 2),
('Gel za oblikovanje obrva', 'Prozirni gel koji učvršćuje obrve i daje prirodan izgled.', 13.75, 1, 2),
('Četka za lice', 'Silikonska četka za nežno čišćenje i masažu lica.', 11.50, 1, 1),
('Set profesionalnih četkica za šminku', 'Komplet visokokvalitetnih četkica za preciznu aplikaciju šminke.', 45.00, 1, 1),
('Set profesionalnih četkica za šminku', 'Komplet visokokvalitetnih četkica za preciznu aplikaciju šminke.', 44.00, 0, 2),
('Set profesionalnih četkica za šminku', 'Komplet visokokvalitetnih četkica za preciznu aplikaciju šminke.', 45.00, 1, 3);

INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika) VALUES
('Hidratantna krema', 'Bogata krema za dubinsku hidrataciju kože, pogodna za sve tipove kože.', 25.99, 1, 1),
('Šampon za volumen', 'Šampon koji daje kosi punoću i sjaj, bez parabena.', 12.50, 1, 1),
('Regenerator za kosu', 'Njegujući regenerator koji olakšava raščešljavanje.', 14.00, 1, 1),
('Maska za kosu s keratinom', 'Intenzivna njega za obnovu oštećene kose.', 18.75, 1, 1),
('Ulje za kosu argan', 'Prirodno ulje koje vraća sjaj i elastičnost.', 22.40, 1, 1),
('Lak za kosu', 'Fiksator frizure s dugotrajnim učinkom.', 9.99, 1, 1),
('Pjena za kosu', 'Lagani styling proizvod za volumen i oblikovanje.', 11.20, 1, 1),
('Gel za kosu', 'Snažan gel za oblikovanje frizura.', 10.50, 1, 1),
('Serum protiv ispucalih vrhova', 'Serum za njegu i zaštitu kose.', 19.99, 1, 1),
('Balzam za usne', 'Prirodni balzam sa shea maslacem i vitaminom E.', 5.99, 1, 1),
('Krema za ruke', 'Hranjiva krema koja omekšava kožu ruku.', 7.50, 1, 1),
('Gel za tuširanje', 'Osnažujući gel sa mirisom citrusa.', 6.80, 1, 1),
('Piling za tijelo', 'Prirodni piling s morskom soli i uljem badema.', 15.60, 1, 1),
('Krema protiv bora', 'Anti-age krema s hijaluronskom kiselinom.', 32.00, 1, 1),
('Maska za lice s glinom', 'Čisti i matira kožu lica.', 13.50, 1, 1),
('Toner za lice', 'Osvježavajući toner za svakodnevnu njegu.', 9.40, 1, 1),
('Mlijeko za tijelo', 'Hidratantno mlijeko s mirisom vanilije.', 11.90, 1, 1),
('Parfemska voda', 'Elegantna cvjetno-voćna mirisna nota.', 45.00, 1, 1),
('Set za manikir', 'Komplet alata za njegu noktiju.', 25.50, 1, 1),
('Lak za nokte - crveni', 'Dugotrajni lak visokog sjaja.', 8.90, 1, 1);

INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika) VALUES
('Hidratantna krema', 'Bogata krema za dubinsku hidrataciju kože.', 26.50, 1, 2),
('Šampon za volumen', 'Šampon koji daje kosi punoću i sjaj.', 12.90, 1, 2),
('Maska za kosu s keratinom', 'Intenzivna njega za obnovu oštećene kose.', 19.50, 1, 2),
('Krema za ruke', 'Hranjiva krema koja omekšava kožu ruku.', 7.80, 1, 2),
('Lak za nokte - crveni', 'Dugotrajni lak visokog sjaja.', 9.20, 1, 2),
('Balzam za usne', 'Prirodni balzam sa shea maslacem.', 6.20, 1, 2);

INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika) VALUES
('Regenerator za kosu', 'Njegujući regenerator koji olakšava raščešljavanje.', 14.50, 1, 3),
('Serum protiv ispucalih vrhova', 'Serum za njegu i zaštitu kose.', 20.50, 1, 3),
('Piling za tijelo', 'Prirodni piling s morskom soli i uljem badema.', 16.00, 1, 3),
('Krema protiv bora', 'Anti-age krema s hijaluronskom kiselinom.', 33.00, 1, 3),
('Maska za lice s glinom', 'Čisti i matira kožu lica.', 14.00, 1, 3);

INSERT INTO PROIZVOD (Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika) VALUES
('Mlijeko za tijelo', 'Hidratantno mlijeko s mirisom vanilije.', 12.50, 1, 4),
('Parfemska voda', 'Elegantna cvjetno-voćna mirisna nota.', 46.00, 1, 4),
('Gel za kosu', 'Snažan gel za oblikovanje frizura.', 10.90, 1, 4),
('Toner za lice', 'Osvježavajući toner za svakodnevnu njegu.', 9.90, 1, 4),
('Lak za kosu', 'Fiksator frizure s dugotrajnim učinkom.', 10.20, 1, 4),
('Set za manikir', 'Komplet alata za njegu noktiju.', 26.00, 1, 4);


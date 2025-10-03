use kozmeticki_salon_hci;

DELIMITER $$

CREATE PROCEDURE dohvati_korisnika
(IN pKorisnickoIme VARCHAR(40), IN pLozinka VARCHAR(40))
BEGIN
    SELECT * FROM korisnik k WHERE k.KorisnickoIme=pKorisnickoIme AND k.Lozinka=pLozinka;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dodaj_korisnika(
    IN pKorisnickoIme VARCHAR(40),
    IN pLozinka VARCHAR(40),
    IN pIme VARCHAR(40),
    IN pPrezime VARCHAR(40),
    IN pEmail VARCHAR(40),
    IN pTipNaloga VARCHAR(40)
)
BEGIN
    INSERT INTO korisnik (KorisnickoIme, Lozinka, Ime, Prezime, EmailAdresa, TipNaloga, Jezik, Tema)
    VALUES (pKorisnickoIme, pLozinka, pIme, pPrezime, pEmail, pTipNaloga);
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_kozmeticki_salon
(IN pIdKS INT)
BEGIN
    SELECT 
        ks.IdKozmetickogSalona, 
        ks.Naziv, 
        ks.Adresa, 
        CONVERT(ks.RadnoVrijeme USING utf8) AS RadnoVrijeme,
        ks.MJESTO_BrojPoste,
        ks.CJENOVNIK_IdCjenovnika,
        ks.Telefon
    FROM kozmeticki_salon ks
    WHERE ks.IdKozmetickogSalona = pIdKS
    GROUP BY ks.IdKozmetickogSalona, ks.Naziv;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_usluge
(IN pIdCj INT)
BEGIN
    SELECT 
        u.IdUsluge,
        u.Naziv,
        u.Cijena,
        u.PotrebnoVrijeme,
        u.Opis,
        u.TIP_USLUGE_IdTipaUsluge,
        t.Naziv AS TipNaziv
    FROM usluga u
    INNER JOIN tip_usluge t ON u.TIP_USLUGE_IdTipaUsluge = t.IdTipaUsluge
    WHERE u.CJENOVNIK_IdCjenovnika = pIdCj;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_zaposlene (IN pIdKs INT) 
BEGIN
    SELECT 
        k.IdKorisnika,
        k.KorisnickoIme,
        k.Lozinka,
        k.EmailAdresa,
        k.TipNaloga,
        k.Ime,
        k.Prezime,
        z.Adresa, 
        z.DatumZaposlenja,
        z.Plata,
        z.KOZMETICKI_SALON_IdKozmetickogSalona,
        z.Obrisan
    FROM zaposleni z
    INNER JOIN korisnik k ON k.IdKorisnika = z.KORISNIK_IdKorisnika
    WHERE z.KOZMETICKI_SALON_IdKozmetickogSalona = pIdKs
      AND (z.Obrisan IS NULL OR z.Obrisan = 0);
END$$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE dohvati_zaposlenog (IN pId INT)
BEGIN
    SELECT 
		k.IdKorisnika,
		k.KorisnickoIme,
        k.Lozinka,
        k.EmailAdresa,
        k.TipNaloga,
        k.Ime,
        k.Prezime,
        z.Adresa, 
        z.DatumZaposlenja,
        z.Plata,
        z.KOZMETICKI_SALON_IdKozmetickogSalona
    FROM zaposleni z
    INNER JOIN korisnik k ON k.IdKorisnika= z.KORISNIK_IdKorisnika
        WHERE k.IdKorisnika=pId;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE ukloni_kozmeticara
(IN pId INT)
BEGIN
    IF EXISTS(SELECT 1 FROM zaposleni WHERE KORISNIK_IdKorisnika = pId) THEN
        UPDATE zaposleni 
        SET Obrisan = 1
        WHERE KORISNIK_IdKorisnika=pId;
    END IF;
END $$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE izmijeni_platu
(IN pId INT, IN pPlata DECIMAL(6, 2))
BEGIN
	UPDATE zaposleni 
	SET Plata=pPlata 
	WHERE KORISNIK_IdKorisnika=pid;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_korisnicke_info(IN pId INT)
BEGIN
    SELECT 
        k.IdKorisnika AS Id,
        k.KorisnickoIme AS KorisnickoIme,
        k.Ime AS Ime,
        k.Prezime AS Prezime,
        k.EmailAdresa AS Email
    FROM korisnik k
    WHERE k.IdKorisnika = pId;
END$$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE dodaj_uslugu
(IN pNaziv VARCHAR(40), IN pCijena DECIMAL(6,2), IN pPotrebnoVrijeme TIME, IN pOpis VARCHAR(100), IN pIdCjenovnika INT, IN pIdTipaUsluge INT)
BEGIN
	INSERT INTO usluga(Naziv, Cijena, PotrebnoVrijeme, Opis, CJENOVNIK_IdCjenovnika, TIP_USLUGE_IdTipaUsluge)
    VALUES(pNaziv, pCijena, pPotrebnoVrijeme, pOpis, pIdCjenovnika, pIdTipaUsluge);
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dodaj_tip_usluge
(IN pNaziv VARCHAR(40))
BEGIN
	IF NOT EXISTS(SELECT * FROM tip_usluge WHERE UPPER(Naziv)=UPPER(pNaziv))
	THEN
		INSERT INTO tip_usluge(Naziv) 
		VALUES(pNaziv);
	END IF;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE obrisi_tip_usluge
(IN pId INT)
BEGIN
	IF EXISTS(SELECT * FROM tip_usluge WHERE IdTipaUsluge=pId)
	THEN
		DELETE FROM tip_usluge 
		WHERE IdTipaUsluge=pId;
	END IF;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE izmijeni_naziv_tipa_usluge
(IN pId INT, IN pNaziv VARCHAR(40))
BEGIN
	IF EXISTS(SELECT * FROM tip_usluge WHERE IdTipaUsluge=pId)
	THEN
		UPDATE tip_usluge 
		SET Naziv=pNaziv 
		WHERE IdTipaUsluge=pId;
	END IF;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE azuriraj_uslugu
(IN pId INT, IN pNaziv VARCHAR(40), IN pCijena DECIMAL(6, 2), IN pVrijeme TIME, IN pOpis VARCHAR(100), IN pIdCjenovnika INT, IN pIdTipaUsluge INT)
BEGIN
	IF EXISTS(SELECT * FROM usluga WHERE IdUsluge=pId)
    THEN
        UPDATE usluga 
		SET Naziv=pNaziv, Cijena=pCijena, PotrebnoVrijeme=pVrijeme, Opis=pOpis, CJENOVNIK_IdCjenovnika=pIdCjenovnika, TIP_USLUGE_IdTipaUsluge=pIdTipaUsluge 
		WHERE IdUsluge=pId;
	END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE obrisi_uslugu
(IN pId INT)
BEGIN
	IF EXISTS(SELECT * FROM usluga WHERE IdUsluge=pId)
	THEN
		DELETE FROM usluga 
		WHERE IdUsluge=pId;
	END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE azuriraj_korisnika
(
    IN pId INT, 
    IN pKorisnickoIme VARCHAR(20), 
    IN pLozinka VARCHAR(20), 
	IN pIme VARCHAR(45),
	IN pPrezime VARCHAR(45),
    IN pEmailAdresa VARCHAR(45), 
    IN pTipNaloga VARCHAR(45)
)
BEGIN
    IF EXISTS (SELECT * FROM korisnik WHERE IdKorisnika = pId) THEN
        UPDATE korisnik 
        SET KorisnickoIme = pKorisnickoIme, 
            Lozinka = pLozinka, 
			Ime = pIme,
			Prezime = pPrezime,
            EmailAdresa = pEmailAdresa, 
            TipNaloga = pTipNaloga 
        WHERE IdKorisnika = pId;
    END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_kozmeticki_salon_filter(
    IN grad VARCHAR(100),
    IN tip_usluge VARCHAR(100)
)
BEGIN
    SELECT DISTINCT ks.IdKozmetickogSalona, ks.Naziv, ks.Adresa, m.Naziv AS Mjesto
    FROM kozmeticki_salon ks
    INNER JOIN mjesto m ON ks.MJESTO_BrojPoste = m.BrojPoste
    INNER JOIN cjenovnik cj ON ks.CJENOVNIK_IdCjenovnika = cj.IdCjenovnika
    INNER JOIN usluga u ON cj.IdCjenovnika = u.CJENOVNIK_IdCjenovnika
    INNER JOIN tip_usluge tu ON u.TIP_USLUGE_IdTipaUsluge = tu.IdTipaUsluge
    WHERE (grad IS NULL OR m.Naziv = grad) AND (tip_usluge IS NULL OR tu.Naziv = tip_usluge)
    ORDER BY ks.Naziv;
END $$

DELIMITER ;

DELIMITER $$

CALL dohvati_kozmeticki_salon_filter(
    'Banja Luka', null
);

CREATE PROCEDURE azuriraj_kozmeticki_salon
(IN pId INT, IN pNaziv VARCHAR(45), IN pAdresa VARCHAR(45), IN pRadnoVrijeme text, IN pBrojPoste INT, IN pTelefon VARCHAR(45))
BEGIN
	IF EXISTS(SELECT * FROM kozmeticki_salon WHERE IdKozmetickogSalona=pId)
    THEN
        UPDATE kozmeticki_salon 
		SET Naziv=pNaziv, Adresa=pAdresa, RadnoVrijeme=pRadnoVrijeme, MJESTO_BrojPoste=pBrojPoste, Telefon=pTelefon 
		WHERE IdKozmetickogSalona=pId;
	END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE kreiraj_raspored
(IN pIdMenadzera INT, IN pOd DATE, IN pDo DATE)
BEGIN
    INSERT INTO raspored (MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika, Od, `Do`) 
    VALUES (pIdMenadzera, pOd, pDo);
    SELECT LAST_INSERT_ID() AS NoviId;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE obrisi_raspored
(IN pId INT)
BEGIN
    IF EXISTS(SELECT 1 FROM raspored WHERE IdRasporeda = pId) THEN
        DELETE FROM raspored_stavka 
        WHERE RASPORED_IdRasporeda = pId;

        DELETE FROM raspored 
        WHERE IdRasporeda = pId;
    END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_rasporede_za_kozmeticki_salon
(IN pIdSalona INT)
BEGIN
	SELECT * from raspored r 
    INNER JOIN menadzer m ON r.MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika=m.ZAPOSLENI_KORISNIK_IdKorisnika
    INNER JOIN zaposleni z ON m.ZAPOSLENI_KORISNIK_IdKorisnika=z.KORISNIK_IdKorisnika
    INNER JOIN korisnik k ON z.KORISNIK_IdKorisnika=k.IdKorisnika
    INNER JOIN kozmeticki_salon ks ON ks.IdKozmetickogSalona=z.KOZMETICKI_SALON_IdKozmetickogSalona
    WHERE ks.IdKozmetickogSalona=pIdSalona
    ORDER BY r.Od DESC;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dodaj_stavku_rasporeda
(
    IN pDanUSedmici VARCHAR(20),
    IN pIdKozmeticara INT,
    IN pIdSalona INT,
    IN pNazivSmjene VARCHAR(45),
    IN pIdRasporeda INT
)
BEGIN
    IF EXISTS (
        SELECT 1
        FROM raspored_stavka
        WHERE DanUSedmici = pDanUSedmici
          AND KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika = pIdKozmeticara
          AND SMJENA_KOZMETICKI_SALON_IdKozmetickogSalona = pIdSalona
          AND RASPORED_IdRasporeda = pIdRasporeda
    ) THEN
        UPDATE raspored_stavka
        SET SMJENA_Naziv = pNazivSmjene
        WHERE DanUSedmici = pDanUSedmici
          AND KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika = pIdKozmeticara
          AND SMJENA_KOZMETICKI_SALON_IdKozmetickogSalona = pIdSalona
          AND RASPORED_IdRasporeda = pIdRasporeda;
    ELSE
        INSERT INTO raspored_stavka (
            DanUSedmici,
            KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika,
            SMJENA_KOZMETICKI_SALON_IdKozmetickogSalona,
            SMJENA_Naziv,
            RASPORED_IdRasporeda
        )
        VALUES (
            pDanUSedmici,
            pIdKozmeticara,
            pIdSalona,
            pNazivSmjene,
            pIdRasporeda
        );
    END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_smjenu (
    IN pNaziv VARCHAR(45),
    IN pIdKozmetickogSalona INT
)
BEGIN
    SELECT * FROM smjena
    WHERE Naziv = pNaziv
      AND KOZMETICKI_SALON_IdKozmetickogSalona = pIdKozmetickogSalona;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_id_zaposlenog
(IN pIme VARCHAR(45), IN pPrezime VARCHAR(45))
BEGIN
	SELECT KORISNIK_IdKorisnika FROM zaposleni z 
    INNER JOIN korisnik k ON z.KORISNIK_IdKorisnika=k.IdKorisnika
    WHERE k.Ime=pIme AND k.Prezime=pPrezime;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_stavku_rasporeda_dan_zaposleni (IN pIdZaposlenog INT, IN pDan VARCHAR(20), IN pIdRasporeda INT)
BEGIN
	SELECT rs.*, s.Od, s.`Do`, ks.IdKozmetickogSalona AS KOZMETICKI_SALON_IdKozmetickogSalona
    FROM raspored_stavka rs
    INNER JOIN smjena s ON rs.SMJENA_Naziv=s.Naziv
    INNER JOIN zaposleni z ON rs.KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika=z.KORISNIK_IdKorisnika
    INNER JOIN korisnik k ON z.KORISNIK_IdKorisnika=k.IdKorisnika
    INNER JOIN kozmeticki_salon ks ON ks.IdKozmetickogSalona=z.KOZMETICKI_SALON_IdKozmetickogSalona
    WHERE rs.KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika=pIdZaposlenog AND rs.DanUSedmici=pDan AND rs.RASPORED_IdRasporeda=pIdRasporeda;
END$$

DELIMITER ;

CREATE PROCEDURE dohvati_stavke_rasporeda
(IN pIdZaposlenog INT, IN pIdRasporeda INT)
SELECT * FROM raspored_stavka
WHERE KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika=pIdZaposlenog AND RASPORED_IdRasporeda=pIdRasporeda;

DELIMITER $$

CREATE PROCEDURE kreiraj_smjenu
(IN pNaziv VARCHAR(45), IN pIdKozmetickogSalona INT, IN pOd TIME, IN pDo TIME)
BEGIN
	IF NOT EXISTS(SELECT * FROM smjena WHERE UPPER(Naziv)=UPPER(pNaziv) AND KOZMETICKI_SALON_IdKozmetickogSalona=pIdKozmetickogSalona)
	THEN
		INSERT INTO smjena(Naziv, KOZMETICKI_SALON_IdKozmetickogSalona, Od, `Do`) VALUES(pNaziv,pIdKozmetickogSalona, pOd, pDo);
	END IF;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE azuriraj_smjenu
(IN pNaziv VARCHAR(45), IN pIdKozmetickogSalona INT, IN pOd TIME, IN pDo TIME)
BEGIN
	IF EXISTS(SELECT * FROM smjena WHERE KOZMETICKI_SALON_IdKozmetickogSalona=pIdKozmetickogSalona AND Naziv=pNaziv)
    THEN
        UPDATE smjena 
		SET Od=pOd, `Do`=pDo
		WHERE KOZMETICKI_SALON_IdKozmetickogSalona=pIdKozmetickogSalona AND Naziv=pNaziv;
	END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE obrisi_smjenu
(IN pNaziv VARCHAR(45), IN pIdKozmetickogSalona INT)
BEGIN
	IF EXISTS(SELECT * FROM smjena WHERE KOZMETICKI_SALON_IdKozmetickogSalona=pIdKozmetickogSalona AND Naziv=pNaziv)
    THEN
        DELETE FROM smjena 
		WHERE KOZMETICKI_SALON_IdKozmetickogSalona=pIdKozmetickogSalona AND Naziv=pNaziv;
	END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_id_posljednjeg_rasporeda
(IN pIdMenadzera INT, OUT NoviId INT)
BEGIN
    SELECT MAX(IdRasporeda) 
    INTO NoviId
    FROM raspored
    WHERE MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika = pIdMenadzera;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_menadzera
(IN pIdSalona INT)
BEGIN
    SELECT * FROM menadzer m
    INNER JOIN zaposleni z
    WHERE z.KOZMETICKI_SALON_IdKozmetickogSalona=pIdSalona;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_proizvode
(IN pIdCj INT)
BEGIN
    SELECT 
        p.IdProizvoda,
        p.Naziv,
        p.Cijena,
        p.Opis,
        p.Dostupnost,
        P.Obrisan
    FROM proizvod p
    WHERE p.CJENOVNIK_IdCjenovnika = pIdCj;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dodaj_proizvod
(IN pNaziv VARCHAR(40), IN pOpis VARCHAR(100), IN pCijena DECIMAL(6,2), IN pDostupnost TINYINT(1), IN pIdCjenovnika INT)
BEGIN
	INSERT INTO proizvod(Naziv, Opis, Cijena, Dostupnost, CJENOVNIK_IdCjenovnika, Obrisan)
    VALUES(pNaziv, pOpis, pCijena, pDostupnost, pIdCjenovnika, 0);
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE azuriraj_proizvod
(IN pId INT, IN pNaziv VARCHAR(40), IN pOpis VARCHAR(100), IN pCijena DECIMAL(6,2), IN pDostupnost TINYINT(1), IN pIdCjenovnika INT)
BEGIN
	IF EXISTS(SELECT * FROM proizvod WHERE IdProizvoda=pId)
    THEN
        UPDATE proizvod 
		SET Naziv=pNaziv, Cijena=pCijena, Opis=pOpis, CJENOVNIK_IdCjenovnika=pIdCjenovnika, Dostupnost=pDostupnost 
		WHERE IdProizvoda=pId;
	END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE obrisi_proizvod
(IN pId INT)
BEGIN
    IF EXISTS(SELECT 1 FROM Proizvod WHERE IdProizvoda = pId) THEN
        UPDATE Proizvod 
        SET Obrisan = 1
        WHERE IdProizvoda = pId;
    END IF;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dodaj_zaposlenog (
    IN pKorisnickoIme VARCHAR(20),
    IN pLozinka VARCHAR(20),
    IN pEmailAdresa VARCHAR(45),
    IN pTipNaloga VARCHAR(45), 
    IN pIme VARCHAR(45),
    IN pPrezime VARCHAR(45),
    IN pIdSalona INT
)
BEGIN
    DECLARE v_IdKorisnika INT;

    INSERT INTO korisnik (KorisnickoIme, Lozinka, EmailAdresa, TipNaloga, Ime, Prezime)
    VALUES (pKorisnickoIme, pLozinka, pEmailAdresa, pTipNaloga, pIme, pPrezime);

    SET v_IdKorisnika = LAST_INSERT_ID();

    INSERT INTO zaposleni (Adresa, DatumZaposlenja, Plata, KORISNIK_IdKorisnika, KOZMETICKI_SALON_IdKozmetickogSalona)
    VALUES ("", CURDATE(), 0, v_IdKorisnika, pIdSalona);

    IF pTipNaloga = 'Manager' THEN
        INSERT INTO menadzer (ZAPOSLENI_KORISNIK_IdKorisnika)
        VALUES (v_IdKorisnika);
    END IF;

    IF pTipNaloga = 'Beautician' THEN
        INSERT INTO kozmeticar (ZAPOSLENI_KORISNIK_IdKorisnika)
        VALUES (v_IdKorisnika);
    END IF;
END$$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE dodaj_klijenta (
    IN pKorisnickoIme VARCHAR(20),
    IN pLozinka VARCHAR(20),
    IN pIme VARCHAR(45),
    IN pPrezime VARCHAR(45),
    IN pEmailAdresa VARCHAR(45),
    IN pTipNaloga VARCHAR(45)
)
BEGIN
    DECLARE v_IdKorisnika INT;

    INSERT INTO korisnik (KorisnickoIme, Lozinka, EmailAdresa, TipNaloga, Ime, Prezime)
    VALUES (pKorisnickoIme, pLozinka, pEmailAdresa, pTipNaloga, pIme, pPrezime);

    SET v_IdKorisnika = LAST_INSERT_ID();
    
    IF pTipNaloga = 'CLIENT' THEN
        INSERT INTO klijent (KORISNIK_IdKorisnika)
        VALUES (v_IdKorisnika);
    END IF;

END$$

DELIMITER ;

DELIMITER $$

/*
  1) Kreira novu korpu za korisnika i salon ili vraća postojeću IdKorpe
*/
CREATE PROCEDURE kreiraj_korpu_ako_ne_postoji(
    IN pIdKlijenta INT,
    IN pIdSalona INT
)
BEGIN
    DECLARE v_IdKorpe INT;

    SELECT IdKorpe INTO v_IdKorpe
    FROM KORPA
    WHERE KLIJENT_KORISNIK_IdKorisnika = pIdKlijenta
      AND KOZMETICKI_SALON_IdKozmetickogSalona = pIdSalona
    LIMIT 1;

    IF v_IdKorpe IS NULL THEN
        INSERT INTO KORPA (KLIJENT_KORISNIK_IdKorisnika, KOZMETICKI_SALON_IdKozmetickogSalona)
        VALUES (pIdKlijenta, pIdSalona);
        SELECT LAST_INSERT_ID() AS IdKorpe;
    ELSE
        SELECT v_IdKorpe AS IdKorpe;
    END IF;
    
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dodaj_u_korpu(
    IN pIdKlijenta INT,
    IN pIdProizvoda INT,
    IN pKolicina INT
)
BEGIN
    DECLARE v_IdSalona INT;
    DECLARE v_IdKorpe INT;

    -- nadji salon kroz cjenovnik povezan s proizvodom
    SELECT ks.IdKozmetickogSalona INTO v_IdSalona
    FROM PROIZVOD pr
    JOIN KOZMETICKI_SALON ks ON pr.CJENOVNIK_IdCjenovnika = ks.CJENOVNIK_IdCjenovnika
    WHERE pr.IdProizvoda = pIdProizvoda
    LIMIT 1;

    IF v_IdSalona IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Salon for product not found';
    END IF;

    -- pronadji ili kreiraj korpu za korisnika i salon
    SELECT IdKorpe INTO v_IdKorpe
    FROM KORPA
    WHERE KLIJENT_KORISNIK_IdKorisnika = pIdKlijenta
      AND KOZMETICKI_SALON_IdKozmetickogSalona = v_IdSalona
    LIMIT 1;

    IF v_IdKorpe IS NULL THEN
        INSERT INTO KORPA (KLIJENT_KORISNIK_IdKorisnika, KOZMETICKI_SALON_IdKozmetickogSalona)
        VALUES (pIdKlijenta, v_IdSalona);
        SET v_IdKorpe = LAST_INSERT_ID();
    END IF;

    -- ako vec postoji stavka za isti proizvod u toj korpi, povecaj kolicinu
    IF EXISTS (SELECT 1 FROM NARUDZBA_STAVKA WHERE Korpa_IdKorpe = v_IdKorpe AND PROIZVOD_IdProizvoda = pIdProizvoda AND NARUDZBA_IdNarudzbe IS NULL) THEN
        UPDATE NARUDZBA_STAVKA
        SET Kolicina = Kolicina + pKolicina
        WHERE Korpa_IdKorpe = v_IdKorpe AND PROIZVOD_IdProizvoda = pIdProizvoda AND NARUDZBA_IdNarudzbe IS NULL;
    ELSE
        INSERT INTO NARUDZBA_STAVKA (NARUDZBA_IdNarudzbe, PROIZVOD_IdProizvoda, Kolicina, Korpa_IdKorpe)
        VALUES (NULL, pIdProizvoda, pKolicina, v_IdKorpe);
    END IF;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_korpu_za_korisnika(
    IN pIdKlijenta INT
)
BEGIN
    SELECT k.IdKorpe,
           ns.IdStavke,
           ns.PROIZVOD_IdProizvoda AS IdProizvoda,
           p.Naziv AS NazivProizvoda,
           p.Cijena,
           ns.Kolicina,
           k.KOZMETICKI_SALON_IdKozmetickogSalona AS IdSalona,
           ks.Naziv AS NazivSalona
    FROM KORPA k
    JOIN NARUDZBA_STAVKA ns ON ns.Korpa_IdKorpe = k.IdKorpe AND ns.NARUDZBA_IdNarudzbe IS NULL
    JOIN PROIZVOD p ON p.IdProizvoda = ns.PROIZVOD_IdProizvoda
    JOIN KOZMETICKI_SALON ks ON ks.IdKozmetickogSalona = k.KOZMETICKI_SALON_IdKozmetickogSalona
    WHERE k.KLIJENT_KORISNIK_IdKorisnika = pIdKlijenta
    ORDER BY ks.Naziv, ns.IdStavke;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE obrisi_stavku_korpe(
    IN pIdStavke INT
)
BEGIN
    DELETE FROM NARUDZBA_STAVKA WHERE IdStavke = pIdStavke AND NARUDZBA_IdNarudzbe IS NULL;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE kreiraj_narudzbu_iz_korpe(
    IN pIdKorpe INT,
    IN pAdresa VARCHAR(100),
    IN pKontakt VARCHAR(20)
)
BEGIN
    DECLARE v_Klijent INT;
    DECLARE v_Salon INT;
    DECLARE v_Ukupno DECIMAL(10,2);
    DECLARE v_IdNarudzbe INT;

    SELECT KLIJENT_KORISNIK_IdKorisnika, KOZMETICKI_SALON_IdKozmetickogSalona
    INTO v_Klijent, v_Salon
    FROM KORPA 
    WHERE IdKorpe = pIdKorpe 
    LIMIT 1;

    IF v_Klijent IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Korpa not found';
    END IF;

    SELECT IFNULL(SUM(p.Cijena * ns.Kolicina),0) INTO v_Ukupno
    FROM NARUDZBA_STAVKA ns
    JOIN PROIZVOD p ON p.IdProizvoda = ns.PROIZVOD_IdProizvoda
    WHERE ns.Korpa_IdKorpe = pIdKorpe AND ns.NARUDZBA_IdNarudzbe IS NULL;

    START TRANSACTION;

    INSERT INTO NARUDZBA (
        Datum, 
        Status, 
        KLIJENT_KORISNIK_IdKorisnika, 
        KOZMETICKI_SALON_IdKozmetickogSalona,
        UkupnaVrijednost, 
        Adresa, 
        KontaktTelefon
    )
    VALUES (
        CURDATE(), 
        0, 
        v_Klijent, 
        v_Salon,
        v_Ukupno, 
        pAdresa, 
        pKontakt
    );

    SET v_IdNarudzbe = LAST_INSERT_ID();

    UPDATE NARUDZBA_STAVKA
    SET NARUDZBA_IdNarudzbe = v_IdNarudzbe,
        Korpa_IdKorpe = NULL
    WHERE Korpa_IdKorpe = pIdKorpe AND NARUDZBA_IdNarudzbe IS NULL;

    DELETE FROM KORPA WHERE IdKorpe = pIdKorpe;

    COMMIT;

    SELECT v_IdNarudzbe AS IdNarudzbe;
END$$


DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_narudzbe_klijenta(
    IN pIdKlijenta INT
)
BEGIN
    SELECT n.IdNarudzbe,
           n.Datum,
           n.Status,
           n.UkupnaVrijednost,
           n.Adresa,
           n.KontaktTelefon, 
           n.KOZMETICKI_SALON_IdKozmetickogSalona
    FROM NARUDZBA n
    WHERE n.KLIJENT_KORISNIK_IdKorisnika = pIdKlijenta
    ORDER BY n.Datum DESC;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_narudzbe_salona(
    IN pIdSalona INT
)
BEGIN
    SELECT DISTINCT n.IdNarudzbe,
           n.Datum,
           n.Status,
           n.UkupnaVrijednost,
           n.Adresa,
           n.KontaktTelefon,
           n.KLIJENT_KORISNIK_IdKorisnika,
           k.Ime AS KlijentIme,
           k.Prezime AS KlijentPrezime
    FROM NARUDZBA n
    JOIN NARUDZBA_STAVKA ns ON ns.NARUDZBA_IdNarudzbe = n.IdNarudzbe
    JOIN PROIZVOD p ON p.IdProizvoda = ns.PROIZVOD_IdProizvoda
    JOIN KOZMETICKI_SALON ks ON ks.CJENOVNIK_IdCjenovnika = p.CJENOVNIK_IdCjenovnika
    JOIN KORISNIK k ON k.IdKorisnika = n.KLIJENT_KORISNIK_IdKorisnika
    WHERE ks.IdKozmetickogSalona = pIdSalona
    ORDER BY n.Datum DESC;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_stavke_narudzbe(
    IN pIdNarudzbe INT
)
BEGIN
    SELECT ns.IdStavke,
           ns.PROIZVOD_IdProizvoda,
           p.Naziv AS NazivProizvoda,
           ns.Kolicina,
           p.Cijena,
           (ns.Kolicina * p.Cijena) AS StavkaVrijednost
    FROM NARUDZBA_STAVKA ns
    JOIN PROIZVOD p ON p.IdProizvoda = ns.PROIZVOD_IdProizvoda
    WHERE ns.NARUDZBA_IdNarudzbe = pIdNarudzbe;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE azuriraj_kolicinu_stavke (
    IN pIdStavke INT,
    IN pNovaKolicina INT
)
BEGIN
    IF pNovaKolicina <= 0 THEN
        DELETE FROM narudzba_stavka
        WHERE IdStavke = pIdStavke;
    ELSE
        UPDATE narudzba_stavka
        SET Kolicina = pNovaKolicina
        WHERE IdStavke = pIdStavke;
    END IF;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE azuriraj_status_narudzbe(
    IN pIdNarudzbe INT,
    IN pStatus TINYINT
)
BEGIN
    UPDATE NARUDZBA 
    SET Status = pStatus 
    WHERE IdNarudzbe = pIdNarudzbe;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE obrisi_narudzbu(
    IN pIdNarudzbe INT
)
BEGIN
    DELETE FROM NARUDZBA_STAVKA WHERE NARUDZBA_IdNarudzbe = pIdNarudzbe;
    DELETE FROM NARUDZBA WHERE IdNarudzbe = pIdNarudzbe;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_korpe(IN pIdKlijenta INT)
BEGIN
    SELECT k.IdKorpe, k.KLIJENT_KORISNIK_IdKorisnika, k.KOZMETICKI_SALON_IdKozmetickogSalona, s.Naziv AS NazivSalona
    FROM KORPA k
    JOIN KOZMETICKI_SALON s ON s.IdKozmetickogSalona = k.KOZMETICKI_SALON_IdKozmetickogSalona
    WHERE k.KLIJENT_KORISNIK_IdKorisnika = pIdKlijenta;
    
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_stavke_u_korpi(IN pIdKorpe INT)
BEGIN
    SELECT ns.IdStavke, ns.KORPA_IdKorpe, ns.PROIZVOD_IdProizvoda,
           p.Naziv as Naziv, ns.Kolicina, p.Cijena
    FROM NARUDZBA_STAVKA ns
    JOIN PROIZVOD p ON p.IdProizvoda = ns.PROIZVOD_IdProizvoda
    WHERE ns.KORPA_IdKorpe = pIdKorpe;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_racune_klijenta(
    IN pIdKorisnika INT
)
BEGIN
    SELECT DISTINCT r.NARUDZBA_IdNarudzbe AS IdRacuna,
           r.Iznos,
           r.Datum,
           r.Status,
           n.IdNarudzbe,
           n.Status AS NarudzbaStatus,
           n.Datum AS DatumNarudzbe,
           n.UkupnaVrijednost,
           s.Naziv AS NazivSalona
    FROM RACUN r
    JOIN NARUDZBA n 
        ON r.NARUDZBA_IdNarudzbe = n.IdNarudzbe
    JOIN NARUDZBA_STAVKA ns 
        ON ns.NARUDZBA_IdNarudzbe = n.IdNarudzbe
    JOIN KORPA k 
        ON ns.Korpa_IdKorpe = k.IdKorpe
    JOIN KOZMETICKI_SALON s 
        ON k.KOZMETICKI_SALON_IdKozmetickogSalona = s.IdKozmetickogSalona
    WHERE n.KLIJENT_KORISNIK_IdKorisnika = pIdKorisnika
    ORDER BY r.Datum DESC;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_racune_klijenta(
    IN pIdKorisnika INT
)
BEGIN
    SELECT r.NARUDZBA_IdNarudzbe AS IdRacuna,
           r.Iznos,
           r.Datum,
           r.Status,
           n.IdNarudzbe,
           n.Status AS NarudzbaStatus,
           n.Datum AS DatumNarudzbe,
           n.UkupnaVrijednost
    FROM RACUN r
    JOIN NARUDZBA n ON r.NARUDZBA_IdNarudzbe = n.IdNarudzbe
    WHERE n.KLIJENT_KORISNIK_IdKorisnika = pIdKorisnika
    ORDER BY r.Datum DESC;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE dohvati_racune_salona(
    IN pIdSalona INT
)
BEGIN
    SELECT DISTINCT r.NARUDZBA_IdNarudzbe AS IdRacuna,
           r.Iznos,
           r.Datum,
           r.Status,
           n.IdNarudzbe,
           n.Status AS NarudzbaStatus,
           n.Datum AS DatumNarudzbe,
           n.UkupnaVrijednost,
           k.Ime AS KlijentIme,
           k.Prezime AS KlijentPrezime
    FROM RACUN r
    JOIN NARUDZBA n ON r.NARUDZBA_IdNarudzbe = n.IdNarudzbe
    JOIN NARUDZBA_STAVKA ns ON ns.NARUDZBA_IdNarudzbe = n.IdNarudzbe
    JOIN PROIZVOD p ON p.IdProizvoda = ns.PROIZVOD_IdProizvoda
    JOIN KOZMETICKI_SALON ks ON ks.CJENOVNIK_IdCjenovnika = p.CJENOVNIK_IdCjenovnika
    JOIN KLIJENT kl ON kl.KORISNIK_IdKorisnika = n.KLIJENT_KORISNIK_IdKorisnika
    JOIN KORISNIK k ON k.IdKorisnika = kl.KORISNIK_IdKorisnika
    WHERE ks.IdKozmetickogSalona = pIdSalona
    ORDER BY r.Datum DESC;
END$$

DELIMITER ;



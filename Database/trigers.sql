use kozmeticki_salon_hci;

DELIMITER $$

CREATE TRIGGER trg_azuriranje_narudzbe
AFTER UPDATE ON NARUDZBA
FOR EACH ROW
BEGIN
    -- Ako narudžba prelazi u IN_PROGRESS (1)
    IF NEW.Status = 1 THEN
        -- Kreiraj račun samo ako već ne postoji
        IF NOT EXISTS (
            SELECT 1
            FROM RACUN
            WHERE NARUDZBA_IdNarudzbe = NEW.IdNarudzbe
        ) THEN
            INSERT INTO RACUN (Iznos, Datum, NARUDZBA_IdNarudzbe, Status)
            VALUES (NEW.UkupnaVrijednost, NOW(), NEW.IdNarudzbe, 0); 
        END IF;
    END IF;

    -- Ako narudžba prelazi u DELIVERED (2)
    IF NEW.Status = 2 THEN
        UPDATE RACUN
        SET Status = 1
        WHERE NARUDZBA_IdNarudzbe = NEW.IdNarudzbe;
    END IF;

    -- Ako narudžba prelazi u CANCELLED (3)
    IF NEW.Status = 3 THEN
        UPDATE RACUN
        SET Status = 2 
        WHERE NARUDZBA_IdNarudzbe = NEW.IdNarudzbe;
    END IF;
END$$

DELIMITER ;


/*DELIMITER $$

CREATE TRIGGER brisanje_raspored
BEFORE DELETE ON raspored
FOR EACH ROW
BEGIN
    DELETE FROM raspored_stavka 
    WHERE RASPORED_IdRasporeda = OLD.IdRasporeda;
END$$

DELIMITER ; */

/*DELIMITER $$

CREATE TRIGGER trg_proizvod_nedostupan
AFTER UPDATE ON Proizvod
FOR EACH ROW
BEGIN
    -- Ako je proizvod postao nedostupan (0), a ranije je bio dostupan (1)
    IF NEW.Dostupnost = 0 AND OLD.Dostupnost = 1 THEN
        DELETE FROM Narudzba_Stavka
        WHERE PROIZVOD_IdProizvoda = NEW.IdProizvoda
          AND NARUDZBA_IdNarudzbe IS NULL;
    END IF;
END$$

DELIMITER ; */

/*DELIMITER $$

CREATE TRIGGER trg_brisanje_proizvod
BEFORE DELETE ON proizvod
FOR EACH ROW
BEGIN
    DELETE FROM narudzba_stavka 
    WHERE PROIZVOD_IdProizvoda = OLD.IdProizvoda AND NARUDZBA_IdNarudzbe IS NULL;
END$$

DELIMITER ;*/



-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema kozmeticki_salon_hci
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema kozmeticki_salon_hci
-- -----------------------------------------------------


CREATE SCHEMA IF NOT EXISTS `kozmeticki_salon_hci` DEFAULT CHARACTER SET utf8 ;
USE `kozmeticki_salon_hci` ;

-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`KORISNIK`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`KORISNIK` (
  `IdKorisnika` INT NOT NULL AUTO_INCREMENT,
  `KorisnickoIme` VARCHAR(20) NOT NULL,
  `Lozinka` VARCHAR(20) NOT NULL,
  `EmailAdresa` VARCHAR(45) NOT NULL,
  `TipNaloga` VARCHAR(45) NOT NULL,
  `Ime` VARCHAR(45) NOT NULL,
  `Prezime` VARCHAR(45) NOT NULL,
  `Jezik` VARCHAR(10) NULL,
  `Tema` VARCHAR(20) NULL,
  PRIMARY KEY (`IdKorisnika`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`MJESTO`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`MJESTO` (
  `BrojPoste` INT NOT NULL,
  `Naziv` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`BrojPoste`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`CJENOVNIK`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`CJENOVNIK` (
  `IdCjenovnika` INT NOT NULL AUTO_INCREMENT,
  `DatumObjavljivanja` DATE NOT NULL,
  PRIMARY KEY (`IdCjenovnika`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`KOZMETICKI_SALON`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`KOZMETICKI_SALON` (
  `IdKozmetickogSalona` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NOT NULL,
  `Adresa` VARCHAR(45) NOT NULL,
  `RadnoVrijeme` VARCHAR(100) NULL,
  `MJESTO_BrojPoste` INT NOT NULL,
  `CJENOVNIK_IdCjenovnika` INT NOT NULL,
  `Telefon` VARCHAR(45) NULL,
  PRIMARY KEY (`IdKozmetickogSalona`),
  INDEX `fk_KOZMETICKI_SALON_MJESTO1_idx` (`MJESTO_BrojPoste` ASC) VISIBLE,
  INDEX `fk_KOZMETICKI_SALON_CJENOVNIK1_idx` (`CJENOVNIK_IdCjenovnika` ASC) VISIBLE,
  CONSTRAINT `fk_KOZMETICKI_SALON_MJESTO1`
    FOREIGN KEY (`MJESTO_BrojPoste`)
    REFERENCES `kozmeticki_salon_hci`.`MJESTO` (`BrojPoste`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_KOZMETICKI_SALON_CJENOVNIK1`
    FOREIGN KEY (`CJENOVNIK_IdCjenovnika`)
    REFERENCES `kozmeticki_salon_hci`.`CJENOVNIK` (`IdCjenovnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`ZAPOSLENI`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`ZAPOSLENI` (
  `Adresa` VARCHAR(45) NOT NULL,
  `DatumZaposlenja` DATE NOT NULL,
  `Plata` DECIMAL(6,2) NOT NULL,
  `KORISNIK_IdKorisnika` INT NOT NULL,
  `KOZMETICKI_SALON_IdKozmetickogSalona` INT NOT NULL,
  PRIMARY KEY (`KORISNIK_IdKorisnika`),
  INDEX `fk_ZAPOSLENI_KOZMETICKI_SALON1_idx` (`KOZMETICKI_SALON_IdKozmetickogSalona` ASC) VISIBLE,
  CONSTRAINT `fk_ZAPOSLENI_KORISNIK1`
    FOREIGN KEY (`KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`KORISNIK` (`IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ZAPOSLENI_KOZMETICKI_SALON1`
    FOREIGN KEY (`KOZMETICKI_SALON_IdKozmetickogSalona`)
    REFERENCES `kozmeticki_salon_hci`.`KOZMETICKI_SALON` (`IdKozmetickogSalona`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`KOZMETICAR`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`KOZMETICAR` (
  `ZAPOSLENI_KORISNIK_IdKorisnika` INT NOT NULL,
  PRIMARY KEY (`ZAPOSLENI_KORISNIK_IdKorisnika`),
  CONSTRAINT `fk_KOZMETICAR_ZAPOSLENI1`
    FOREIGN KEY (`ZAPOSLENI_KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`ZAPOSLENI` (`KORISNIK_IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`MENADZER`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`MENADZER` (
  `ZAPOSLENI_KORISNIK_IdKorisnika` INT NOT NULL,
  PRIMARY KEY (`ZAPOSLENI_KORISNIK_IdKorisnika`),
  CONSTRAINT `fk_MENADZER_ZAPOSLENI1`
    FOREIGN KEY (`ZAPOSLENI_KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`ZAPOSLENI` (`KORISNIK_IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`KLIJENT`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`KLIJENT` (
  `KORISNIK_IdKorisnika` INT NOT NULL,
  PRIMARY KEY (`KORISNIK_IdKorisnika`),
  CONSTRAINT `fk_KLIJENT_KORISNIK1`
    FOREIGN KEY (`KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`KORISNIK` (`IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`TIP_USLUGE`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`TIP_USLUGE` (
  `IdTipaUsluge` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`IdTipaUsluge`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`USLUGA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`USLUGA` (
  `IdUsluge` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NOT NULL,
  `Cijena` DECIMAL(6,2) NOT NULL,
  `PotrebnoVrijeme` TIME NOT NULL,
  `Opis` VARCHAR(100) NULL,
  `CJENOVNIK_IdCjenovnika` INT NOT NULL,
  `TIP_USLUGE_IdTipaUsluge` INT NOT NULL,
  PRIMARY KEY (`IdUsluge`),
  INDEX `fk_USLUGA_CJENOVNIK1_idx` (`CJENOVNIK_IdCjenovnika` ASC) VISIBLE,
  INDEX `fk_USLUGA_TIP_USLUGE1_idx` (`TIP_USLUGE_IdTipaUsluge` ASC) VISIBLE,
  CONSTRAINT `fk_USLUGA_CJENOVNIK1`
    FOREIGN KEY (`CJENOVNIK_IdCjenovnika`)
    REFERENCES `kozmeticki_salon_hci`.`CJENOVNIK` (`IdCjenovnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_USLUGA_TIP_USLUGE1`
    FOREIGN KEY (`TIP_USLUGE_IdTipaUsluge`)
    REFERENCES `kozmeticki_salon_hci`.`TIP_USLUGE` (`IdTipaUsluge`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`SMJENA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`SMJENA` (
  `Naziv` VARCHAR(45) NOT NULL,
  `KOZMETICKI_SALON_IdKozmetickogSalona` INT NOT NULL,
  `Od` TIME NOT NULL,
  `Do` TIME NOT NULL,
  PRIMARY KEY (`KOZMETICKI_SALON_IdKozmetickogSalona`, `Naziv`),
  INDEX `fk_SMJENA_KOZMETICKI_SALON1_idx` (`KOZMETICKI_SALON_IdKozmetickogSalona` ASC) VISIBLE,
  CONSTRAINT `fk_SMJENA_KOZMETICKI_SALON1`
    FOREIGN KEY (`KOZMETICKI_SALON_IdKozmetickogSalona`)
    REFERENCES `kozmeticki_salon_hci`.`KOZMETICKI_SALON` (`IdKozmetickogSalona`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`RASPORED`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`RASPORED` (
  `IdRasporeda` INT NOT NULL AUTO_INCREMENT,
  `MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika` INT NOT NULL,
  `Od` DATE NOT NULL,
  `Do` DATE NOT NULL,
  PRIMARY KEY (`IdRasporeda`),
  INDEX `fk_RASPORED_MENADZER1_idx` (`MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika` ASC) VISIBLE,
  CONSTRAINT `fk_RASPORED_MENADZER1`
    FOREIGN KEY (`MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`MENADZER` (`ZAPOSLENI_KORISNIK_IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`RASPORED_STAVKA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`RASPORED_STAVKA` (
  `DanUSedmici` VARCHAR(20) NOT NULL,
  `SMJENA_KOZMETICKI_SALON_IdKozmetickogSalona` INT NOT NULL,
  `SMJENA_Naziv` VARCHAR(45) NOT NULL,
  `RASPORED_IdRasporeda` INT NOT NULL,
  `KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika` INT NOT NULL,
  INDEX `fk_RASPORED_STAVKA_SMJENA1_idx` (`SMJENA_KOZMETICKI_SALON_IdKozmetickogSalona` ASC, `SMJENA_Naziv` ASC) VISIBLE,
  PRIMARY KEY (`DanUSedmici`, `RASPORED_IdRasporeda`, `KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika`),
  INDEX `fk_RASPORED_STAVKA_RASPORED1_idx` (`RASPORED_IdRasporeda` ASC) VISIBLE,
  INDEX `fk_RASPORED_STAVKA_KOZMETICAR1_idx` (`KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika` ASC) VISIBLE,
  CONSTRAINT `fk_RASPORED_STAVKA_SMJENA1`
    FOREIGN KEY (`SMJENA_KOZMETICKI_SALON_IdKozmetickogSalona` , `SMJENA_Naziv`)
    REFERENCES `kozmeticki_salon_hci`.`SMJENA` (`KOZMETICKI_SALON_IdKozmetickogSalona` , `Naziv`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_RASPORED_STAVKA_RASPORED1`
    FOREIGN KEY (`RASPORED_IdRasporeda`)
    REFERENCES `kozmeticki_salon_hci`.`RASPORED` (`IdRasporeda`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_RASPORED_STAVKA_KOZMETICAR1`
    FOREIGN KEY (`KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`KOZMETICAR` (`ZAPOSLENI_KORISNIK_IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`NARUDZBA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`NARUDZBA` (
  `IdNarudzbe` INT NOT NULL AUTO_INCREMENT,
  `Datum` DATE NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `KLIJENT_KORISNIK_IdKorisnika` INT NOT NULL,
  `UkupnaVrijednost` DECIMAL(10,2) NOT NULL,
  `Adresa` VARCHAR(100) NOT NULL,
  `KontaktTelefon` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`IdNarudzbe`),
  INDEX `fk_NARUDZBA_KLIJENT1_idx` (`KLIJENT_KORISNIK_IdKorisnika` ASC) VISIBLE,
  CONSTRAINT `fk_NARUDZBA_KLIJENT1`
    FOREIGN KEY (`KLIJENT_KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`KLIJENT` (`KORISNIK_IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`RACUN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`RACUN` (
  `Iznos` DECIMAL(6,2) NOT NULL,
  `NARUDZBA_IdNarudzbe` INT NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  PRIMARY KEY (`NARUDZBA_IdNarudzbe`),
  CONSTRAINT `fk_RACUN_NARUDZBA1`
    FOREIGN KEY (`NARUDZBA_IdNarudzbe`)
    REFERENCES `kozmeticki_salon_hci`.`NARUDZBA` (`IdNarudzbe`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`PROIZVOD`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`PROIZVOD` (
  `IdProizvoda` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NOT NULL,
  `Opis` TEXT NULL,
  `Cijena` DECIMAL(6,2) NOT NULL,
  `Dostupnost` TINYINT(1) NOT NULL,
  `CJENOVNIK_IdCjenovnika` INT NOT NULL,
  PRIMARY KEY (`IdProizvoda`),
  INDEX `fk_PROIZVOD_CJENOVNIK1_idx` (`CJENOVNIK_IdCjenovnika` ASC) VISIBLE,
  CONSTRAINT `fk_PROIZVOD_CJENOVNIK1`
    FOREIGN KEY (`CJENOVNIK_IdCjenovnika`)
    REFERENCES `kozmeticki_salon_hci`.`CJENOVNIK` (`IdCjenovnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`Korpa` TODOOOOOOO KORPA
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`KORPA` (
  `IdKorpe` INT NOT NULL AUTO_INCREMENT,
  `KLIJENT_KORISNIK_IdKorisnika` INT NOT NULL,
  PRIMARY KEY (`IdKorpe`),
  INDEX `fk_KORPA_KLIJENT1_idx` (`KLIJENT_KORISNIK_IdKorisnika` ASC) VISIBLE,
  CONSTRAINT `fk_KORPA_KLIJENT1`
    FOREIGN KEY (`KLIJENT_KORISNIK_IdKorisnika`)
    REFERENCES `kozmeticki_salon_hci`.`KLIJENT` (`KORISNIK_IdKorisnika`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `kozmeticki_salon_hci`.`NARUDZBA_STAVKA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kozmeticki_salon_hci`.`NARUDZBA_STAVKA` (
  `IdStavke` INT NOT NULL AUTO_INCREMENT,
  `NARUDZBA_IdNarudzbe` INT NULL,
  `PROIZVOD_IdProizvoda` INT NOT NULL,
  `Kolicina` INT NOT NULL DEFAULT 1,
  `Korpa_IdKorpe` INT NULL,
  INDEX `fk_NARUDZBA_has_PROIZVOD_PROIZVOD1_idx` (`PROIZVOD_IdProizvoda` ASC) VISIBLE,
  INDEX `fk_NARUDZBA_has_PROIZVOD_NARUDZBA1_idx` (`NARUDZBA_IdNarudzbe` ASC) VISIBLE,
  INDEX `fk_NARUDZBA_STAVKA_Korpa1_idx` (`Korpa_IdKorpe` ASC) VISIBLE,
  PRIMARY KEY (`IdStavke`),
  CONSTRAINT `fk_NARUDZBA_has_PROIZVOD_NARUDZBA1`
    FOREIGN KEY (`NARUDZBA_IdNarudzbe`)
    REFERENCES `kozmeticki_salon_hci`.`NARUDZBA` (`IdNarudzbe`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_NARUDZBA_has_PROIZVOD_PROIZVOD1`
    FOREIGN KEY (`PROIZVOD_IdProizvoda`)
    REFERENCES `kozmeticki_salon_hci`.`PROIZVOD` (`IdProizvoda`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_NARUDZBA_STAVKA_Korpa1`
    FOREIGN KEY (`Korpa_IdKorpe`)
    REFERENCES `kozmeticki_salon_hci`.`Korpa` (`IdKorpe`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

alter table racun
add column Datum DATE NOT NULL;

ALTER TABLE `kozmeticki_salon_hci`.`KORPA`
ADD COLUMN KOZMETICKI_SALON_IdKozmetickogSalona INT NOT NULL;

ALTER TABLE `kozmeticki_salon_hci`.`KORPA`
ADD CONSTRAINT FK_KORPA_SALON
FOREIGN KEY (KOZMETICKI_SALON_IdKozmetickogSalona) REFERENCES `kozmeticki_salon_hci`.`KOZMETICKI_SALON` (`IdKozmetickogSalona`);

-- 1-obrisan, 0 ne
ALTER TABLE `kozmeticki_salon_hci`.`PROIZVOD`
ADD COLUMN Obrisan TINYINT(1);

-- 1-obrisan, 0 ne
ALTER TABLE `kozmeticki_salon_hci`.`ZAPOSLENI`
ADD COLUMN Obrisan TINYINT(1);

ALTER TABLE `kozmeticki_salon_hci`.`NARUDZBA`
ADD COLUMN KOZMETICKI_SALON_IdKozmetickogSalona INT NOT NULL;

ALTER TABLE `kozmeticki_salon_hci`.`NARUDZBA`
ADD CONSTRAINT FK_NARUDZBA_SALON
FOREIGN KEY (KOZMETICKI_SALON_IdKozmetickogSalona)
REFERENCES `kozmeticki_salon_hci`.`KOZMETICKI_SALON` (IdKozmetickogSalona)
ON DELETE NO ACTION
ON UPDATE CASCADE;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

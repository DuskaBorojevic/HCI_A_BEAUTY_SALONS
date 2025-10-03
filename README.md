# Aplikacija za upravljanje kozmetičkim salonima

## Opis projekta
Desktop aplikacija razvijena sa ciljem da olakša i unaprijedi poslovanje kozmetičkih salona kroz digitalizaciju i automatizaciju procesa. 

Podržava tri tipa korisnika:
- **Klijent**
- **Kozmetičar**
- **Menadžer**

Glavne funkcionalnosti uključuju:
- Upravljanje salonima, uslugama i proizvodima
- Kreiranje i praćenje narudžbi i računa
- Upravljanje zaposlenima, smjenama i rasporedima rada
- Pretragu i filtriranje informacija
- Personalizaciju interfejsa (teme i jezici)

Cilj aplikacije je **unapređenje korisničkog iskustva** i **olakšavanje administrativnog poslovanja salona**.

## Tehnologije
- **Backend**: MySQL (procedure, triggeri, upiti)  
- **Frontend**: WPF (.NET)  
- **Jezik**: C#  
- **Internacionalizacija**: podržani jezici su *srpski* i *engleski*

## Instalacija i pokretanje
1. Klonirajte repozitorijum <br>
bash git clone https://github.com/DuskaBorojevic/HCI_A_Beauty_Salons.git
2. Kreirajte bazu podataka iz fajla <br>
 Database/kozmeticki_salon_hci_ddl.sql
3. U App.config podesite parametre za konekciju na MySQL server.
4. Pokrenite projekat iz Visual Studio ili drugog .NET okruženja.
   
## Tipovi korisnika
- **Klijent** – registracija, pregled salona i usluga, filtriranje i pretraživanje salona, pregled i naručivanje proizvoda, praćenje narudžbi, pregled računa  
- **Kozmetičar** – pregled informacija o salonu, upravljanje proizvodima i uslugama, upravljanje narudžbama, raspored rada  
- **Menadžer** – upravljanje informacijama o salonu, zaposlenima, uslugama, proizvodima, tipovima usluga, narudžbama, rasporedima, smjenama i računima

## Korisničko uputstvo
Detaljno korisničko uputstvo sa slikama nalazi se u fajlu:  
**HCI_A_KORISNICKO_UPUSTVO.pdf**

### Autor
Duška Borojević <br>
Predmet: Interakcija čovjek - računar
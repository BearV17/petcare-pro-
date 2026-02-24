# PetCarePro - Administratief Systeem voor Dierenpension

## Overzicht
PetCarePro is een Windows Forms applicatie ontwikkeld in C# voor het beheren van administratieve taken binnen een dierenpension. De applicatie biedt functionaliteiten voor het registreren van dieren, beheren van eigenaren, plannen van verblijven en het bijhouden van medische gegevens.

## Technologieën
- **Programmeertaal**: C# (.NET 8.0)
- **GUI Framework**: Windows Forms
- **Database**: SQLite
- **IDE**: Visual Studio

## Functionaliteiten

### 1. Dierenregistratie
- Aanmaken, bekijken en bewerken van huisdierprofielen
- Opslaan van gegevens zoals naam, soort, ras, leeftijd, geslacht, chipnummer
- Koppeling met de eigenaar

### 2. Eigenarenbeheer
- Invoeren en bijhouden van klantgegevens (naam, adres, telefoon, e-mail)
- Overzicht van alle huisdieren per eigenaar
- Zoekfunctionaliteit

### 3. Verblijfsplanning
- Inchecken en uitchecken van dieren
- Kalenderweergave van geplande verblijven
- Statusbeheer (Gepland, Actief, Voltooid, Geannuleerd)
- Hoknummer toewijzing

### 4. Gebruikersbeheer
- Authenticatie met gebruikersnaam en wachtwoord
- Rolgebaseerde toegang (Administrator, Gebruiker)
- Standaard admin account: gebruikersnaam `admin`, wachtwoord `admin123`

## Projectstructuur

```
PetCarePro/
├── Data/
│   └── DatabaseHelper.cs      # Database initialisatie en connectie
├── Models/
│   ├── User.cs                # Gebruikersmodel
│   ├── Owner.cs               # Eigenarenmodel
│   ├── Pet.cs                 # Dierenmodel
│   └── Stay.cs                # Verblijfsmodel
├── Forms/
│   ├── LoginForm.cs           # Inlogscherm
│   ├── MainForm.cs            # Hoofdformulier met tabbladen
│   ├── PetManagementForm.cs  # Dierenbeheer
│   ├── PetForm.cs             # Dier toevoegen/bewerken
│   ├── OwnerManagementForm.cs # Eigenarenbeheer
│   ├── OwnerForm.cs           # Eigenaar toevoegen/bewerken
│   ├── StayManagementForm.cs  # Verblijfsbeheer
│   ├── StayForm.cs            # Verblijf toevoegen/bewerken
│   ├── CalendarForm.cs        # Kalenderweergave
│   └── SettingsForm.cs        # Instellingen (alleen admin)
├── Program.cs                 # Entry point
└── PetCarePro.csproj          # Project configuratie
```

## Installatie en Gebruik

### Vereisten
- Visual Studio 2022 of nieuwer
- .NET 8.0 SDK

### Opstarten
1. Open de solution file `PetCarePro.sln` in Visual Studio
2. Herstel NuGet packages (rechtsklik op solution → Restore NuGet Packages)
3. Build de oplossing (F6)
4. Run de applicatie (F5)

### Eerste gebruik
- Standaard admin account:
  - Gebruikersnaam: `admin`
  - Wachtwoord: `admin123`

## Database
De applicatie gebruikt SQLite en maakt automatisch een databasebestand aan (`PetCarePro.db`) in de uitvoerdirectory bij de eerste start.

### Database Schema
- **Users**: Gebruikers en authenticatie
- **Owners**: Eigenaren/klanten
- **Pets**: Huisdieren
- **Stays**: Verblijfsperiodes
- **MedicalRecords**: Medische gegevens (voorbereid voor toekomstige uitbreiding)
- **CareSchedules**: Verzorgingsschema's (voorbereid voor toekomstige uitbreiding)

## Toekomstige Uitbreidingen
- Medische gegevens module
- Verzorgingsschema's
- Facturatie en betalingen
- Rapportage functionaliteit
- Foto upload voor dieren
- Export functionaliteit

## Licentie
Dit project is ontwikkeld als onderdeel van een educatieve opdracht.

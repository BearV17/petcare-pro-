# PetCarePro - Start Gids

Welkom bij je PetCarePro project! Dit document helpt je om snel te beginnen.

## 📁 Projectstructuur

```
petcare pro/
├── PetCarePro.sln                    # Solution file
├── PetCarePro/                       # Hoofdproject
│   ├── Program.cs                    # Entry point
│   ├── Data/
│   │   └── DatabaseHelper.cs         # Database beheer
│   ├── Models/                       # Data modellen
│   │   ├── User.cs
│   │   ├── Owner.cs
│   │   ├── Pet.cs
│   │   └── Stay.cs
│   └── Forms/                        # GUI formulieren
│       ├── LoginForm.cs
│       ├── MainForm.cs
│       ├── PetManagementForm.cs
│       ├── PetForm.cs
│       ├── OwnerManagementForm.cs
│       ├── OwnerForm.cs
│       ├── StayManagementForm.cs
│       ├── StayForm.cs
│       ├── CalendarForm.cs
│       └── SettingsForm.cs
├── Documentatie/                     # Alle documentatie
│   ├── Business_Requirements.md
│   ├── Planning.md
│   ├── Functioneel_Ontwerp.md
│   ├── Technisch_Ontwerp.md
│   ├── ERD_Diagram.txt
│   ├── Gebruikershandleiding_Template.md
│   └── Reflectie_Template.md
└── README.md                         # Project overzicht
```

## 🚀 Snel Starten

### Stap 1: Open het Project
1. Open Visual Studio 2022 (of nieuwer)
2. Open `PetCarePro.sln`
3. Wacht tot Visual Studio alle packages heeft hersteld

### Stap 2: Build het Project
1. Druk op `F6` of ga naar **Build → Build Solution**
2. Controleer of er geen fouten zijn

### Stap 3: Run de Applicatie
1. Druk op `F5` of klik op de **Play** knop
2. De applicatie start en toont het inlogscherm

### Stap 4: Inloggen
- **Gebruikersnaam:** `admin`
- **Wachtwoord:** `admin123`

## ✅ Wat is al Gebouwd?

### Functionaliteiten (MVP)
- ✅ Login/Authenticatie
- ✅ Dierenbeheer (toevoegen, bewerken, verwijderen, zoeken)
- ✅ Eigenarenbeheer (toevoegen, bewerken, verwijderen, zoeken)
- ✅ Verblijfsplanning (inchecken, uitchecken, statusbeheer)
- ✅ Kalenderweergave
- ✅ Gebruikersrollen (Administrator, Gebruiker)
- ✅ Database met alle tabellen
- ✅ Basis foutafhandeling

### Database
- ✅ Automatisch aangemaakt bij eerste start
- ✅ Standaard admin gebruiker
- ✅ Alle tabellen met relaties
- ✅ CASCADE DELETE voor data integriteit

## 📝 Documentatie

Alle documentatie templates staan in de `Documentatie/` folder:

1. **Business_Requirements.md** - Vul aan met je klantgesprek resultaten
2. **Planning.md** - Pas aan met je eigen planning
3. **Functioneel_Ontwerp.md** - Vul aan met wireframes/schetsen
4. **Technisch_Ontwerp.md** - Al grotendeels ingevuld, controleer en pas aan
5. **ERD_Diagram.txt** - Database model (tekstuele weergave)
6. **Gebruikershandleiding_Template.md** - Vul aan met screenshots
7. **Reflectie_Template.md** - Vul aan na afronding project

## 🎯 Volgende Stappen

### Voor de Opdracht:

1. **Deel 1: Verkenning & Voorbereiding**
   - [ ] Lees de opdracht goed door
   - [ ] Bereid 10+ vragen voor voor je docent
   - [ ] Voer klantgesprek (docent)
   - [ ] Vul Business Requirements aan
   - [ ] Maak je eigen planning

2. **Deel 2: Functioneel Ontwerp**
   - [ ] Maak wireframes/schetsen van schermen
   - [ ] Vul Functioneel Ontwerp aan
   - [ ] Werk gebruikersrollen uit

3. **Deel 3: Technisch Ontwerp**
   - [ ] Controleer Technisch Ontwerp
   - [ ] Maak ERD diagram (visueel, bijv. met draw.io)
   - [ ] Pas technisch ontwerp aan waar nodig

4. **Deel 4: Bouw de Basis** ✅ (Al gedaan!)
   - [x] Project opgezet
   - [x] Database gemaakt
   - [x] Basis GUI gebouwd

5. **Deel 5: Uitbreiden** ✅ (Al gedaan!)
   - [x] Alle MVP modules geïmplementeerd
   - [ ] Test alle functionaliteiten
   - [ ] Voeg eventueel nice-to-haves toe

6. **Deel 6: Afronden**
   - [ ] Maak screenshots van alle schermen
   - [ ] Vul gebruikershandleiding aan met screenshots
   - [ ] Maak presentatievideo
   - [ ] Schrijf reflectie
   - [ ] Lever alles netjes in

## 🔧 Aanpassingen Maken

### Database Aanpassen
- Wijzig `DatabaseHelper.cs` in de `CreateTables` methode
- Let op: bestaande databases worden niet automatisch geüpdatet

### Nieuwe Functionaliteit Toevoegen
1. Maak een nieuw formulier in `Forms/`
2. Voeg eventueel een nieuwe model toe in `Models/`
3. Voeg database operaties toe in `DatabaseHelper.cs` of maak een nieuwe helper

### Styling Aanpassen
- Alle formulieren gebruiken Windows Forms standaard styling
- Pas kleuren/fonts aan in de `InitializeComponent` methoden

## ⚠️ Belangrijke Opmerkingen

1. **Database Locatie:** De database (`PetCarePro.db`) wordt aangemaakt in de `bin/Debug/` of `bin/Release/` folder wanneer je de applicatie runt.

2. **Wachtwoorden:** Momenteel worden wachtwoorden in plaintext opgeslagen. Voor productie zou je hashing moeten gebruiken.

3. **Testdata:** Er is alleen een standaard admin gebruiker. Voeg testdata toe via de applicatie zelf.

4. **Screenshots:** Maak screenshots van alle schermen voor je gebruikershandleiding!

## 🐛 Problemen Oplossen

### Project compileert niet
- Rechtsklik op solution → "Restore NuGet Packages"
- Controleer of .NET 8.0 SDK geïnstalleerd is
- Sluit en open Visual Studio opnieuw

### Database wordt niet aangemaakt
- Controleer of de applicatie schrijfrechten heeft in de uitvoerdirectory
- Kijk in `bin/Debug/` of `bin/Release/` voor `PetCarePro.db`

### Forms worden niet getoond
- Controleer of alle namespaces correct zijn
- Build de solution opnieuw (Clean → Build)

## 📚 Handige Links

- [Windows Forms Documentatie](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
- [SQLite in C#](https://www.sqlite.org/docs.html)
- [C# Documentatie](https://docs.microsoft.com/en-us/dotnet/csharp/)

## 💡 Tips

1. **Test regelmatig** - Test elke nieuwe functionaliteit direct
2. **Commit vaak** - Als je Git gebruikt, commit regelmatig
3. **Documenteer tijdens ontwikkeling** - Maak screenshots terwijl je werkt
4. **Vraag hulp** - Als je vastloopt, vraag je docent om hulp
5. **Begin simpel** - Voeg complexiteit toe wanneer de basis werkt

## 🎉 Succes!

Je hebt nu een werkende basis voor PetCarePro! Vul de documentatie aan, test alles goed, en maak een mooie presentatie.

Veel succes met je project!

---

**Laatste update:** [Vul datum in]  
**Versie:** 1.0

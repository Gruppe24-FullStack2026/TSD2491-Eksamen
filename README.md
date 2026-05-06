# BookApp – Full Stack Web Application

## Om prosjektet
BookApp er en full stack MVC-applikasjon bygget med ASP.NET Core og .NET 10.
Applikasjonen lar brukere administrere bøker og forfattere, samt importere bøker
fra Nasjonalbibliotekets åpne API.

## Github-bruker
- Brukernavn: Gruppe24-FullStack2026

## Teknologi
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core med SQLite
- ASP.NET Core Identity
- Tailwind CSS
- Nasjonalbibliotekets API (api.nb.no) 

## Implementerte krav
- Krav 1: Opprettet ASP.NET Core MVC-prosjekt
- Krav 2: Domenemodeller med DataAnnotations (Book, Author, Library)
- Krav 3: DbContext med SQLite og EF Core migreringer
- Krav 4: CRUD-funksjonalitet via scaffolding
- Krav 5: En-til-mange relasjon mellom Author og Book
- Krav 6: Tilpassede views med relaterte data og SelectList
- Krav 7: Tailwind CSS som frontend-rammeverk
- Krav 8: API-integrasjon mot Nasjonalbiblioteket
- Krav 9: Autentisering med ASP.NET Core Identity
- Krav 10: Enhetstester med xUnit

## Kom i gang

### Krav
- .NET 10 SDK
- Node.js med npm

### Installasjon

git clone https://github.com/Gruppe24-FullStack2026/TSD2491-Eksamen
cd TSD2491-Eksamen/BookApp
dotnet restore
dotnet ef database update
npm install
npm run css:build
dotnet run

## Innlogging

### Registrer ny bruker
Gå til /identity/Account/Register eller bruk "Register" knappen

### Testbruker
- Epost: admin@bookapp.no
- Passord: Admin123!

### Passordbegrensinger
- Minimum 8 tegn
- Minst én stor bokstav
- Minst ett tall

## Datamodell
Author (1) ──── (mange) Book

| Modell | Felter |
|---|---|
| Author | Id, Name, Nationality |
| Book | Id, Title, Isbn, PublishedYear, Genre, AuthorId |

## API-integrasjon
Applikasjonen henter bøker fra Nasjonalbibliotekets åpne API ved å sende
søkeforespørsler til følgende endepunkt:
https://api.nb.no/catalog/v1/items
Parametere som brukes:
- `q` – søkeordet brukeren skriver inn
- `mediatype=books` – filtrerer kun bøker

Importerte bøker lagres i databasen. Forfatter opprettes automatisk hvis
den ikke finnes fra før. Sjanger velges manuelt ved import.

## Git branch-struktur
main
└── dev (krav 1-6)
└── [merget til main]
└── extraFeature (krav 7-10)
└── [merget til main]
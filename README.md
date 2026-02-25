# Contacts Manager - Aplikacja obsługująca listę kontaktów

## Krótka specyfikacja techniczna

### 1. Sposób kompilacji i uruchomienia aplikacji
Dzięki pełnej konteneryzacji projektu, ręczna kompilacja kodu nie jest wymagana. Cały proces jest zautomatyzowany i odbywa się wewnątrz środowiska Docker.

* **Budowanie i uruchomienie środowiska:** W głównym katalogu projektu wystarczy wykonać polecenie:
`docker-compose up --build -d`

Po zakończeniu procesu budowania aplikacja będzie gotowa do użycia pod adresem `http://localhost:5173/`.

### Pierwsze logowanie
Aby ułatwić testowanie aplikacji, baza danych jest automatycznie seedowana początkowym kontaktem. Dane dostępowe dla tego konta są przekazywane przez zmienne środowiskowe zdefiniowane w pliku `docker-compose.yml`.

**Domyślne dane logowania:**
* **E-mail:** `admin@gmail.com`
* **Hasło:** `adminpassword123`


### 2. Wykorzystane główne biblioteki
**Backend (.NET 9.0):**
* `Microsoft.EntityFrameworkCore` & `Npgsql.EntityFrameworkCore.PostgreSQL` - ORM i dostawca bazy danych PostgreSQL.
* `BCrypt.Net-Next` - Hashowanie haseł
* `Microsoft.AspNetCore.Authentication.JwtBearer` - Weryfikacja i obsługa tokenów JWT.

**Frontend (React):**
* `react` & `react-dom` - Główny framework widoku.
* `react-router-dom` - Obsługa routingu (SPA)

### 3. Krótkie opisy i struktura backendu

Poniżej znajdziesz zwięzłe opisy głównych komponentów backendu oraz krótką strukturę katalogów.

- Kontrolery:
  - `AuthController` - logowanie i zwracanie wygenerowanego tokenu JWT.
  - `ContactController` - endpointy CRUD dla kontaktów.
  - `CategoryController` - pobieranie listy kategorii i ich podkategorii.

- Serwisy:
  - `JwtService` - generowanie podpisanych tokenów JWT.
  - `DbInitializer` - serwis insertujący do bazy pierwszy kontakt, z danymi logowania podanymi w docker-compose.yml

- Dane:
  - `AppDbContext` - konfiguracja EF, seeding danych słownikowych.
  - Encje: `Contact`, `Category`, `Subcategory`.
  - DTO: `CreateContactDto`, `UpdateContactDto`, `ContactDto`, `LoginDto`, etc., niektóre korzystają z annotacji wspomagających walidacje.

- Mapowanie:
  - `Mappers` - konwersja encja <--> DTO wykorzystująca mechanizm Extension Methods.

- Konfiguracja i uruchomienie:
  - `appsettings.json` / `appsettings.Development.json` - connection string, ustawienia JWT i frontend URL.
  - `Program.cs` - rejestracja usług, konfiguracja CORS, JWT, migracja bazy.

Struktura katalogów backendu:

```
backend/ContactsApp/
├─ Controllers/
│  ├─ AuthController.cs
│  ├─ ContactController.cs
│  └─ CategoryController.cs
├─ Services/
│  └─ DbInitializer.cs
│  └─ JwtService.cs
├─ Data/
│  └─ AppDbContext.cs
├─ Domain/
│  ├─ Entities/
│  └─ Dto/
├─ Mappers/
├─ Migrations/
└─ Properties/
```
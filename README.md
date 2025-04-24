# Social Media Manager - README

## Opis
Aplikacja konsolowa do zarządzania postami na Facebooku, Instagramie i Reddicie z różnymi poziomami dostępu dla użytkowników.

## Funkcje
- **Role użytkowników**: Admin, User, Guest
- **Operacje na postach**: dodawanie, edycja, usuwanie
- **Obsługa platform**: Facebook, Instagram, Reddit
- **Zapisywanie logów**

## Jak uruchomić
1. Pobierz i zainstaluj [.NET8.0+]
2. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/Komputerr/ProjektPO.git
   cd ProjektPO\krol_bartosz_kwaskiewicz_nataniel
   ```
3. Uruchom program:
   ```bash
   dotnet run
   ```

## Logowanie
- **Admin**: login `admin`, hasło `admin`
- **Guest**: zalogowanie w trybie gościa
- Możliwość rejestracji nowych użytkowników

## Pliki danych
- `[platforma].txt` - przechowuje posty
- `users.txt` - dane użytkowników
- `app_logs.txt` - przechowuje logi


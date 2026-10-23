# LifeManagementApp

LifeManagementApp on .NET MAUI -sovellus arjen tehtävien hallintaan. Se kokoaa
tärkeimmät tehtävät ja tämänhetkiset säätiedot yhteen näkymään ja tallentaa
tehtävät laitteen paikalliseen SQLite-tietokantaan.

## Ominaisuudet

- **Top 3:** etusivu näyttää enintään kolme tärkeintä tehtävää
  prioriteettijärjestyksessä. Saman prioriteetin tehtävät järjestetään
  eräpäivän mukaan.
- **Sää:** etusivulla näytetään Open-Meteosta haettu tämänhetkinen lämpötila ja
  tuulen nopeus.
- **Kitesurf-huomio:** sovellus näyttää kitesurf-huomion, kun tuulen nopeus on
  yli 5 m/s.
- **Todo-lista:** tehtäviä voi lisätä, merkitä tehdyiksi ja poistaa. Tehtävällä
  on otsikko, prioriteetti (1–5) ja valinnainen eräpäivä.
- **Lajittelu:** tehtävälistan voi järjestää prioriteetin tai eräpäivän mukaan.
  Eräpäivän mukaan järjestettäessä menneet eräpäivät rajataan pois.

## Vaatimukset ja käynnistys

- .NET 10 SDK
- .NET MAUI -työkuorma
- Androidin kehitykseen Android SDK sekä Android-emulaattori tai -laite.
  iOS-, Mac Catalyst- ja Windows-kohteet vaativat vastaavan alustan
  kehitystyökalut. Projektin Linux-kohteeksi on määritelty Android.

Kloonaa projekti ja asenna tarvittava MAUI-työkuorma:

```bash
git clone https://github.com/btekki/LifeManagementApp.git
cd LifeManagementApp
dotnet workload install maui
dotnet restore
```

Käynnistä Android-emulaattori tai yhdistä laite ja suorita projektin juuressa:

```bash
dotnet build -t:Run -f net10.0-android
```

Visual Studio -kehityksessä avaa `LifeManagementApp.slnx`, valitse sopiva
kohdealusta ja käynnistä sovellus IDE:stä.

## Rakenne

```text
LifeManagementApp/
├── Data/          # EF Core -tietokantakonteksti
├── Models/        # Tehtävä- ja säätietomallit
├── Services/      # Tehtävärepository ja sääpalvelu
├── ViewModels/    # Näkymien tila ja sovelluslogiikka
├── Views/         # MAUI-näkymät (XAML)
├── Platforms/     # Alustakohtainen käynnistys ja asetukset
├── Resources/     # Kuvakkeet, kuvat, fontit ja tyylit
├── App.xaml
├── AppShell.xaml
├── MauiProgram.cs
└── LifeManagementApp.csproj
```

## Luokkien suhteet

```mermaid
classDiagram
    class TodoItem
    class TodoRepository
    class ITodoRepository {
        <<interface>>
    }
    class AppDbContext
    class HomeViewModel
    class IWeatherService {
        <<interface>>
    }
    class OpenMeteoWeatherService
    class WeatherInfo

    TodoRepository ..|> ITodoRepository
    TodoRepository --> TodoItem : CRUD
    TodoRepository --> AppDbContext : käyttää
    AppDbContext --> TodoItem : tallentaa
    HomeViewModel --> ITodoRepository : hakee tehtävät
    HomeViewModel --> IWeatherService : hakee sään
    OpenMeteoWeatherService ..|> IWeatherService
    IWeatherService ..> WeatherInfo : palauttaa
```

## Säärajapinta

Sääpalvelu käyttää Open-Meteon ennusterajapintaa:

```text
https://api.open-meteo.com/v1/forecast
```

Kutsu sisältää seuraavat parametrit:

| Parametri | Arvo tai tarkoitus |
|---|---|
| `latitude` | Leveysaste |
| `longitude` | Pituusaste |
| `current` | `temperature_2m,wind_speed_10m` |
| `wind_speed_unit` | `ms` (metriä sekunnissa) |

Etusivun nykyiset koordinaatit ovat koodissa `0.0, 0.0` ja ne ovat
paikkamerkki. Aseta `HomeViewModel`-luokan koordinaateiksi halutun sijainnin
leveys- ja pituusaste ennen varsinaista käyttöä.

# geek_off_angular

Replacement for round 1 & 2 of the Geek Off at my employer. A work in progress.

# Initial setup

* Run dotnet restore in the /API folder to get the Nuget packages
* Run npm i in the /clientapp folder to get the Angular/Node packages
* Set up the following user secrets in the root folder:

| User secret | Description |
|--|--|
| ConnectionStrings:GeekOff | Postgres database connection |
| AzureAd:Domain | Azure AD domain |
| AzureAd:TenantId | Azure AD Tenant ID |
| AzureAd:ClientId | Azure AD Client ID |

* Install the required fonts
* Run the initial database migration

## Required Fonts

Fonts used in the scoreboard are not included for copyright reasons. These fonts can be found as follows:

| Font name | URL | Use | Changes required |
|--|--|--|--|
| eggcrate.ttf | http://tpirepguide.com/qwizx/tpirfonts/eggcrate.zip | Round 1 board | |
| sportstype.ttf | http://tpirepguide.com/qwizx/tpirfonts/sportstype.zip | Round 3 scores | Rename the file from "Sports Type Full.ttf" |
| fast-money-three.ttf | https://fontstruct.com/fontstructions/show/1181116/fast_money_three | Round 2 board | |

## Database creation

Standard EF migration processes are followed. To create the database locally from a blank server, run `dotnet ef database update 0` for the initial creation. Then run `dotnet ef database update` to get the most current version. 

The dotnet CLI tools must be installed for this, and a dotnet restore must be done to ensure all packages are downloaded before migration.

# Running the App

The API and UI must be run separately. Use ng serve to run the UI, and dotnet run for the API.

Swagger can be found at the endpoint `/swagger`. This only runs in the dev hosting environment.

# Authentication

Logins are handled via the standard Microsoft Authentication Library (MSAL). Logins are restricted to the Geekoff active directory - contact Kevin Trinkle to obtain a login.

# Automated tests

Automated testing is not enforced or required.

# Work to be completed

## Round 2

1. Store (ngrx) - library installed but that's it
2. Countdown timer screen/component
  * Supports countdown from 20 or 25 seconds
  * Import something and style?
  * Kicks off from control screen
3. Results component - Kristin (people see this - big screen)
  * Shows columns of answers and scores
  * API: /api/round2/bigBoard/{yEvent}/{teamNo}
  * Text should be animated
4. Control screen (behind the scenes) - Grant
  * Show list of questions and answers from database. API: /api/round2/allSurvey/{yEvent}
  * Enter answer that team gives for question and points. API: /api/round2/teamAnswer/text, /api/round2/teamAnswer/survey from button
  * Finalize the round. /api/round2/finalize/{yevent}
5. Host screen (host cellphone) - Dan
  * Show questions on the phone. API: /api/round2/allQuestions/{yEvent}
6. Scoreboard (lower priority)
  * API: /api/round2/scoreboard/{yEvent}

## Round 1

Work will be identified after round 2 is completed.

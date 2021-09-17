# geek_off_angular

Replacement for round 1 of the Geek Off at my employer. A work in progress.

# Initial setup

* Run npm i in the /clientapp folder to get the Angular/Node packages
* Set up the following user secrets in the root folder:

| User secret | Description |
|--|--|
| ConnectionStrings:GeekOff | Postgres database connection |

# Running the API

Run the app in debug mode in Visual Studio. This will run both the Angular and .NET core apps together.

Swagger can be found at the endpoint `/swagger`. This only runs in the dev hosting environment.

# Automated tests

Automated testing is not enforced or required.

# Work to be completed

## Round 2

1. Countdown timer screen/component
  * Supports countdown from 20 or 25 seconds
  * Import something and style?
  * Kicks off from control screen
2. Results component - Kristin (people see this - big screen)
  * Shows columns of answers and scores
  * API: /api/round2/bigBoard/{yEvent}/{teamNo}
  * Text should be animated
3. Control screen (behind the scenes) - Grant
  * Show list of questions and answers from database. API: /api/round2/allSurvey/{yEvent}
  * Enter answer that team gives for question and points. API: /api/round2/teamAnswer/text, /api/round2/teamAnswer/survey from button
  * Finalize the round. /api/round2/finalize/{yevent}
4. Host screen (host cellphone) - Dan
  * Show questions on the phone. API: /api/round2/allQuestions/{yEvent}
  * Needs API
5. Scoreboard (lower priority)
  * API: /api/round2/scoreboard/{yEvent}

  ## Round 1

Work will be identified after round 1 is completed.

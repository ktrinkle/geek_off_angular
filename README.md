# geek_off_angular

Replacement for round 1 of the Geek Off at my employer. A work in progress.

# Work to be completed

## Round 2
1. Countdown timer screen/component
  1. Supports countdown from 20 or 25 seconds
  2. Import something and style?
  3. Kicks off from control screen
2. Results component - Kristin (people see this - big screen)
  1. Shows columns of answers and scores
  2. API: /api/round2/Bigboard
  3. Text should be animated
3. Control screen (behind the scenes) - Grant
  1. Show list of questions and answers from database. API: /api/round2/allSurvey/{yEvent}
  2. Enter answer that team gives for question and points. API: /api/round2/teamAnswer
  3. Finalize the round. /api/round2/finalize
4. Host screen (host cellphone) - Dan
  1. Show questions on the phone
  2. Needs API

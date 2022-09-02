# Geek Off scoring system - Master Control Program

## Purpose

This is the master control program for the Geek-O-Matic. It runs in a loop basis, and communicates with the Arduino units to get responses and determine priority. It also accepts input from the end user to determine if the answer was correct or not.

This is written in Python 3.

### How it works

This is only used for the Jeopardy round. For all other rounds, response buttons are not used.

### Initializing the system

Once the serial devices are all connected, the controller will press "]" on the attached USB keyboard. This will look for the Arduino devices by serial number, and assign to the appropriate Linux serial ports.

Once the round begins, the teams are loaded into the system by pressing "[". This will assign teams to the appropriate color buzzers.

### Communication with Score-O-Matic

The person controlling the scoring system will select the question that the team picks from an attached USB keyboard (wireless). This tells the Geek-O-Matic what question is picked, and the appropriate point value. It also resets the Score-O-Matic at the same time.

The matrix is as follows:

| Points | Category 1 | Category 2 | Category 3 | Category 4 | Category 5 |
|--|--|--|--|--|--|
| 100 | 1 | 2 | 3 | 4 | 5 |
| 200 | q | w | e | r | t |
| 300 | a | s | d | f | g |
| 400 | z | x | c | v | b |

Once the question is asked, the controller will unlock the event loop by pressing ";".

The Geek-O-Matic will know based on the response buttons which team answered the question and is considered active. A five second lockout is applied after a team answers, and this can be changed in the script.

There is a wireless keyboard connected to the Geek-O-Matic that the person controlling the scoring system uses to tell if the team answered correctly or not. The keys that are looked for are the following:

- Success: "+"
- Failure: "-"

To lock the system from accepting teams, press ";" on the USB keyboard.

## Libraries

This requires the Psycopg2 and Pygame libraries.

## Running this yourself

Change the database connection string to your Postgres database, and the ard dictionary to the serial numbers of your Arduino units.

## Disclaimer

The work presented here does not reflect upon my employer and is not an official product of my employer.

There are probably better ways to code this, but I'm not a Python programmer by trade. As usual with code...if it works, run with it.

## Copyright

The code is Copyright © 2018-2020 Kevin Trinkle and is made available under the GPL v3 license.
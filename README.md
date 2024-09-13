# Mills

## Game description

#### What is Mills?
Mills is a strategy board game for two players. The game has many variations, such as **Three men's morris**, **Six men's morris**, **Nine men's morris**, **Twelve men's morris** or **Lasker morrris (Ten men's morris)**. Three main variations of the game are three, six, and nine men's morris, which are also the variations I've decided to implement. Each variation has its own board, consisting of different number of **men (pieces)** and number of **intersections (points)**.

#### Description of the Three main variations
1. Three men's morris (TMM) - consists of a grid with 9 intersections, or points. Each player has 3 men. Mills can be also formed diagonally.

<img src="https://github.com/user-attachments/assets/a40448cb-075a-4542-b96b-20711bf593da" alt="Three Men's Morris" width="300"/>

2. Six men's morris (SMM) - consists of grid with 16 intersections, or points. Each player has 6 men. 

<img src="https://github.com/user-attachments/assets/2503c23d-887f-4ef9-9065-594478507d8b" alt="Six Men's Morris" width="300"/>

3. Nine men's morris (NMM) - consists of grid with 24 intersections, or points. Each player has 9 men.

<img src="https://github.com/user-attachments/assets/1a049bd0-025d-4595-b6e8-229572c7f102" alt="Nine Men's Morris" width="300"/>

#### How to win
- Players try to form **_mills_** — three of their own men lined horizontally, vertically, or sometimes diagonally — allowing a player to remove an opponent's man from the game. 

- When a **_mill_** is formed, the player who formed it can only remove other team's men that aren't currently forming mills, unless there are no other options left. 

- A player wins by reducing the opponent to two men, as they can no longer form mills and thus are unable to win or when a player blocks all possible moves for the opponent, as they can no longer make a move.

- There is also a possibility that **two** mills can be formed **at the same time**, if that happens, the player who formed the mills can remove **two** of the other team's men (only in PvP gamemode).

#### Game phases
- Phase 1 - **Placing phase** - players place their men from their bench onto the board. Once all of the bench men have been placed, Phase 2 begins.

- Phase 2 - **Moving phase** - players move their men around the board to adjacent points to form mills and remove opponent's men.

- Phase 3 - **Flying phase** - once a team's number of men reaches 3, they can move their men to any free space on the board.

---
### Required installations

The program does not require any external libraries to be downloaded. However, to start the program via the command line, you must have the .NET SDK installed. If it's not already installed, you can download and install it by following the instructions on the .NET website.

---
### How to start the program?

1. Open a terminal or command prompt.
2. Navigate to the directory containing the Mills game’s `.csproj` file.
3. run `dotnet run` command 

---
### Possible improvements

Although I'm quite happy how the program turned out, there is always room for improvement. There are still many variations of the game, rules, and phases which can be implemented. One possible improvement could be adding a completely new game variation, such as twelve men's morris or tens men's morris. Another improvement could be adding threefold repetition to not repeat the same moves three times.

---
### Author
Oliver Tomáš Cenker

### Event
This game was created during the summer semester of 2023/24 as part of my studies at Charles University, as a credit program for Programming II. 

---
### For more information, check [user manual](docs/user%20manual.md) or [program manual](docs/program%20manual.md). 
---

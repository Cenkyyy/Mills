# Program manual

#### _Welcome to the program manual! This manual explains how the AI works, how the program is structured, and what each part of the structure does._

---
## Classes


1. `Menu (Form1)` - Handles gamemode selection and opponent choice.

2. `Game (Form2)` - Manages initialization of all buttons needed to play the game and their click events.

3. `Board` - Contains functions for checking and updating the board state.

4. `GameLogic` - Implements the logic and rules of the game.

5. `Computer` - Contains functions that computer needs to play.

6. `Player` - Defines player's actions and movements.

7. `BoardEvaluation` - Evaluates board.

## Methods

`class Menu` - methods that initialize menu are omitted.

- `UncheckGamemodeCheckBoxes()` - unchecks all other gamemode CheckBoxes when one is selected (checked).

- `CheckBox_CheckedChanged(object sender, EventArgs e)`- calls `UncheckGamemodeCheckBoxes()` when a CheckBox is clicked to ensure only one gamemode is selected (checked) at a time.

- `CreateGame()` - initializes and displays the `Game (Form2)`. Sets the background board image that is appropriate to selected gamemode.  Returns `true` if the game was successfully created, otherwise, tells user to choose game variation first.

- `Player_vs_Player_Button_Click(object sender, EventArgs e)` - sets the Form2's (Game's) text to `PvP` if `CreateGame()` is successful.

- `Player_vs_Computer_Button_Click(object sender, EventArgs e)` - sets the Form2's (Game's) text to `PvC` if `CreateGame()` is successful.

- `Rules_Button_Click(object sender, EventArgs e)` - displays the game rules to the user.

---
`class Game` - methods that initialize game are omitted.

- `GetMillsPatternsArray()` - returns an array of gamemode's mills patterns.

- `GetIntersectionArray()` - returns an array of gamemode's in-game buttons.

- `AskToPlayAgain()` - asks the user if he wants to play again after the game has ended.

- `ResetGame(bool ResetGame)` - resets the game if `ResetGame` is `true`, otherwise, returns to `Menu`.

- `Team1_Bench_Buttons_Clicked(object sender, EventArgs e)` - handles the click event for Team 1’s bench buttons during the **Placing phase**.

- `Team2_Bench_Buttons_Clicked(object sender, EventArgs e)` - handles the click event for Team 2’s bench buttons during the **Placing phase**.

- `InGameButtons_Clicked(object sender, EventArgs e)` - makes a move if the clicked button is valid and, if the opponent is computer, let's computer make a move after.

- `Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)` - if the opponent is computer, performs computer's move 1 second after player to make the game more clear. 

---
`class Board`

- `ChangeButtonsColor(Color color1, Color color2)` - changes button color from `color1` to `color2`. When `color2` is set to `eliminationColor`, shows only the non-forming mill buttons for elimination, unless no such buttons are available.  

- `GetNonMillButtons(Button[] gamemodeButtons)` - returns a list of current team's non-forming mill buttons, if none are available, returns all of the current team's buttons.

- `DisplayAvailableButtons(Button clickedButton, bool team2Turn)` - based on phase of the game, displays available buttons.

- `DisplayAdjacentButtons(Button clickedButton)` - turns only adjacent buttons to the clickedButton as available. 

- `IsCurrentlyEliminating()` - returns true if a player is currently eliminating a button from the board.

- `PlaceButton(Button clickedButton)` - places piece on the clicked button's position and checks if the mill was formed.

- `RememberButtonToMove(Button clickedButton)` - remembers the button to move during **Moving phase**.

- `HighlightRemovableButtons()` - highlights buttons that the player who formed a mill can eliminate. 

- `EliminateButton(Button clickedButton)` - eliminates the piece at the clicked button's position. 

---
`class GameLogic`

- `CheckForMill()` - returns numbers of newly formed mills for the current team and updates team's mill list.

- `IsMillNewlyFormed(List<List<Button>> currentMillButtonsTeam2dList, Button[,] millsPatterns, int i)` - returns `true` if the formed mill is new.

- `CheckExistingMills()` - verifies if the formed mills are still valid and removes any that aren't. 

- `IsFormingMillWithButton(Button button, Color color1)` - returns `true` if the `button` is part of mill formation.

- `CheckMillPatternColors(Button b1, Button b2, Button b3, Color color1, Color color2, Color color3)` - returns `true` if a given mill pattern is of a color combination of color1, color2 and color3.

- `IsPlayerBlocked(Color teamColor)` - returns `true` if current player is blocked.

- `CheckForWin()` - determines if the game has ended based on the remaining pieces of each team or a if one of the teams has been blocked.

---
`class Computer`

- `ChooseBenchButton()` - selects the first available bench button for computer for placing during the Placing phase.

- `ComputerPlacingPhase()` - makes a move in the placing phase.

- `ComputerMovingPhase()` - makes a move in the moving phase.

- `ComputerEliminate()` - after each move, check if mill has been formed and if so, eliminate one of opponent's buttons.

- `ComputerMove()` - makes a move and removes opponent's piece if a mill was formed 1 second after making move. It is the logic of the computer.

- `Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)` - eliminates button computer has chosen 1 second after making its move.

- `SimulatePlacing(Button buttonToPlace, Color currentTeamColor)` - used in the minimax algorithm, tries placing `buttonToPlace` based on `currentTeamColor` onto the board.

- `UndoPlacing(Button buttonToPlace, Color currentTeamColor)` - used in the minimax algorithm, undoes tried `buttonToPlace` based on `currentTeamColor` from the board.

- `SimulateMove(Button buttonToMove, Button buttonToPlace, Color currentTeamColor)` - used in the minimax algorithm, tries placing `buttonToPlace` and moving `buttonToMove` based on `currentTeamColor` onto the board.

- `UndoMove(Button buttonToMove, Button buttonToPlace, Color currentTeamColor)` - used in the minimax algorithm, undoes tried placement of `buttonToPlace` and returns `buttonToMove` back to its original color based on `currentTeamColor`.

- `SimulateElimination(Button buttonToEliminate, Color colorToEliminateFrom)` - used in the minimax algorithm, tries eliminated `buttonToEliminate` based on `colorToEliminateFrom` from the board.

- `UndoElimination(Button buttonToEliminate, Color colorToEliminateFrom)` - used in the minimax algorithm, returns eliminated `buttonToEliminate` back to the board based on `colorToEliminateFrom`.

- `FindBestMove()` - returns the best button to move, best button to place on and if the move formed a mill, best button to eliminate as well.

- `Minimax(int depth, bool isMaximizing, int alpha, int beta)` - implements the Minimax algorithm with alpha-beta pruning to decide the optimal move.

- `Maximize(int depth, int alpha, int beta)` - represents the computer's turn in the Minimax algorithm, trying to maximize the score.

- `Minimize(int depth, int alpha, int beta)` - represents the player's turn in the Minimax algorithm, trying to minimize the score.

---
`class Player`
    
- `PlayerMove(Button clickedButton)` - makes a move as a player based on the clickedButton's color.

---
`class BoardEvaluation`

- `EvaluateBoard()` - returns the overall score for the current board for both teams.

- `EvaluateBoardBasedOnTeam(bool isMaximizing)` - evaluates and returns the score of the board based on various scenarios.

- `EvaluateWinningStates(bool isMaximizing)` - checks if someone has won and returns score based on who did.

## Computer's algorithm

The computer uses Minimax algorithm with alpha-beta pruning and calculates 3 steps ahead in Moving phase and 1 step ahead in Placing phase.

### What's Minimax algorithm?

- it's a recursive algorithm typically used in two-player games where players take turns (e.g. chess, tic-tac-toe, and in this case, mills).
- the goal is to find the optimal move for the computer assuming that the player is also playing optimally.
- the algorithm builds a tree of possible game states and evaluates them to determine the best move.
- the evaluation is trying to maximize the score for the computer and minimize for the player. 

### What's alpha-beta pruning?

- it's an optimization for the Minimax algorithm that reduces the number of game states evaluated.
- it eliminates the branches in the tree that won't affect the outcome using `alpha` and `beta`.
- `alpha` represents the best value that the (maximizer) computer can guarantee.
- `beta` represents the best value that the (minimizer) player can guarantee.

### The reason why I chose Minimax algorithm

- It is ideal for two-player games where players take turns, which aligns with the Mills game perfectly.

### How does the Minimax algorithm work?

- The algorithm builds a tree based on the current game state.

- Based on **current phase** and **current team's turn** the Minimax algorithm:
    1. **Placing phase**
        - simulates placing a piece on the board for the current team.
        - checks if a mill has been formed, if so, simulates eliminating one of the possible buttons.
        - evaluates the game state if the depth has reached zero.
        - undoes the elimination after evaluating.
        - undoes the placing after evaluating.
    2. **Moving phase** and **Flying phase**
        - simulates moving a piece to a new position (adjacent if it's Moving phase, any position if it's Flying phase).
        - checks if a mill has been formed, if so, simulates eliminating one of the possible buttons.
        - evaluates the game state if the depth has reached zero.
        - undoes the elimination after evaluating.
        - undoes the move after evaluating.

- by this logic mentioned in points 1 and 2, computer tries all possible movements and evaluates each new game state to determine the best possible move by comparing scores and updating the best score when a better one is found.

### How is the board being evaluated?

The evaluation process goes like this:

- after each simulated move, board evalutes winning states (determines if someone has won).

- if the depth has reached zero, evaluates current board by this priority:
    
    1. Newly formed mill.  

    2. Blocking opponents mill.

    3. Being close to forming a mill (having two buttons and third is free). 

    The board also takes into account the mobility of the team (number of pieces player can move with) and the control of the game (difference between teams number of pieces).

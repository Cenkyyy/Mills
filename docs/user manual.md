# User manual

#### _Welcome to the user manual! This manual should help users imagine how the program works without needing to know the code behind it._

---
## What happens after user launches the game?

After the game is launched, a **menu** pops up with few options to interact with.

#### What can you interact with?
- **Check Boxes** - Interact by checking/unchecking them. There are three check boxes in total, each representing one of the game variations:

    1. `Three Men's Morris` - Represents Three Men's morris variation of the game.
    
    2. `Six Men's Morris` - Represents Six Men's morris variation of the game.
    
    3. `Nine Men's Morris` - Represents Nine Men's morris variation of the game.

- **Buttons** - Interact by clicking them.

    1. `Rules` button - Displays the rules of the game.
    
    2. `PvP` button - Loads the Player vs. Player gamemode.
    
    3. `PvC` button - Loads the Player vs. Computer gamemode.

---
**Note:** Before choosing your opponent (by clicking `PvP` or `PvC` buttons), you need to select a game variation by picking one of the three check boxes mentioned earlier. 

---
### What happens after I've chosen the game variantion and opponent?

After the user has selected their desired game variation and chose an opponent, the menu closes, and the **game starts**.

## What's the game structure?

- There are two teams: **Team 1** and **Team 2**. 
- User always plays as Team 1, however, at the start, he can choose if he would like to start or not.
- Players take turns.

#### What if I choose a **Computer** as my opponent?

If you choose Computer as your opponent, you will play as Team 1, and Computer as Team 2.

Once you make a move, the computer calculates the best possible move by analyzing **three steps ahead** in the Moving phase and **one step ahead** in Placing phase and then makes its move.

#### What if I choose a **Player** as my opponent?

If you choose another Player as your opponent, you will play as Team 1, and the other Player as Team 2.

## How does the game work?

During the game, the user can interact with three types of buttons by clicking them. 

1. `Team 1` bench buttons 

2. `Team 2` bench buttons 

3. `In-game (board)` buttons

Bench buttons are only visible during the **Placing phase (Phase 1)**. Once you click a bench button, free in-game buttons will turn **green**, indicating that you may **place** the button onto one of them. Once you place the button, the bench button is **removed**.

After you have placed all the bench buttons onto the board, **Moving phase (Phase 2)** will begin. In moving phase, the entire logic is based on the **color** of button you have clicked.

If the clicked button's color is:

- **Team 1's color** - Team 1 wants to move the clicked button to a new position. After the button is clicked, the free, adjacent, in-game buttons will turn green (see the second point in Green color).

- **Team 2's color** - Team 2 wants to move the clicked button to a new position. After the button is clicked, the free, adjacent, in-game buttons will turn green (see the second point in Green Color).

- **Green** - That means either:
    1. A bench button is being placed, if so, place the the new button onto the board and remove the bench button.

    2. An in-game team's button is being moved to new position.

- **Red** - That means that the other team has formed a mill, and the red buttons are the ones the user who formed mill can eliminate.

- **Board's color** - Nothing happens.

## What happens after someone wins? 

After one of the players wins, the game will end, and the winning team will be displayed. Once the winning team is shown, the game will ask the user to choose one of the following:

1. Reset the same gamemode against the same opponent

2. Go back to Menu

3. Exit the game

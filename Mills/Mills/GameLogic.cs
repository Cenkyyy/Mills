using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mills
{
    internal class GameLogic
    {
        // keep track of buttons forming mills for each team
        public List<List<Button>> millButtons2dListTeam1 = new List<List<Button>>();
        public List<List<Button>> millButtons2dListTeam2 = new List<List<Button>>();

        // special case - 2 mills formed at the same time
        public bool wereTwoMillsFormed = false;

        Game _game;
        Board _board;

        public GameLogic(Game game)
        {
            _game = game;
        }

        public void SetBoard(Board board)
        {
            _board = board;
        }

        // Returns number of newly formed mills for current team
        public int CheckForMill()
        {
            // remove mills from both teams that aren't forming mills anymore
            CheckExistingMills();

            // get mill-forming buttons
            Button[,] millsPatterns = _game.GetMillsPatternsArray();

            // decide current team's color and list to store formed mills
            Color currentTeamColor = _game.isTeam1Turn ? _game.colorTeam1 : _game.colorTeam2;
            List<List<Button>> currentMillButtonsTeamList = _game.isTeam1Turn ? millButtons2dListTeam1 : millButtons2dListTeam2;

            int millsFormed = 0;

            for (int i = 0; i < millsPatterns.GetLength(0); i++)
            {
                if (millsPatterns[i, 0].BackColor == currentTeamColor &&
                    millsPatterns[i, 1].BackColor == currentTeamColor &&
                    millsPatterns[i, 2].BackColor == currentTeamColor &&
                    IsMillNewlyFormed(currentMillButtonsTeamList, millsPatterns, i)) // add mill only if it's new
                {
                    currentMillButtonsTeamList.Add(new List<Button> { millsPatterns[i, 0], millsPatterns[i, 1], millsPatterns[i, 2] });
                    millsFormed++;
                }
            }
            return millsFormed;
        }

        // Returns true if formed mill is new
        public bool IsMillNewlyFormed(List<List<Button>> currentMillButtonsTeam2dList, Button[,] millsPatterns, int i)
        {
            foreach (var mill in currentMillButtonsTeam2dList)
            {
                // if mill was already formed before
                if (mill.Contains(millsPatterns[i, 0]) && mill.Contains(millsPatterns[i, 1]) && mill.Contains(millsPatterns[i, 2]))
                {
                    return false;
                }
            }
            return true;
        }

        // Removes mills that aren't forming mills anymore for both teams
        private void CheckExistingMills()
        {
            // check Team 1's formed mills
            List<List<Button>> storedMillsToRemoveTeam1 = new List<List<Button>>();

            foreach (var formedMill in millButtons2dListTeam1)
            {
                if (formedMill[0].BackColor != _game.colorTeam1 ||
                    formedMill[1].BackColor != _game.colorTeam1 ||
                    formedMill[2].BackColor != _game.colorTeam1)
                {
                    storedMillsToRemoveTeam1.Add(formedMill); // broken mill found
                }
            }
            foreach (var brokenMill in storedMillsToRemoveTeam1)
            {
                millButtons2dListTeam1.Remove(brokenMill); // remove the broken mills
            }

            // check Team 2's formed mills
            List<List<Button>> storedMillsToRemoveTeam2 = new List<List<Button>>();

            foreach (var formedMill in millButtons2dListTeam2)
            {
                if (formedMill[0].BackColor != _game.colorTeam2 ||
                    formedMill[1].BackColor != _game.colorTeam2 ||
                    formedMill[2].BackColor != _game.colorTeam2)
                {
                    storedMillsToRemoveTeam2.Add(formedMill); // broken mill found
                }
            }
            foreach (var brokenMill in storedMillsToRemoveTeam2)
            {
                millButtons2dListTeam2.Remove(brokenMill); // remove the broken mills
            }
        }

        // Checks if there are 2 buttons of color1 and the third is 'button'
        public int IsFormingMillWithButton(Button button, Color color1)
        {
            // counter of how many mills are satisfy the condition
            int howManyFound = 0;

            // get mill-forming buttons
            Button[,] millsPatterns = _game.GetMillsPatternsArray();

            for (int i = 0; i < millsPatterns.GetLength(0); i++)
            {
                Button button1 = millsPatterns[i, 0];
                Button button2 = millsPatterns[i, 1];
                Button button3 = millsPatterns[i, 2];

                // check if the button can form a mill and one of the buttons is 'button'
                if (button1.BackColor == color1 && button2.BackColor == color1 && button3 == button)
                {
                    howManyFound++;
                }
                if (button1.BackColor == color1 && button3.BackColor == color1 && button2 == button)
                {
                    howManyFound++;
                }
                if (button2.BackColor == color1 && button3.BackColor == color1 && button1 == button)
                {
                    howManyFound++;
                }
            }
            return howManyFound;
        }

        // Returns true if a given mill pattern is of a color combination of color1, color2, color3
        public bool CheckMillPatternColors(Button b1, Button b2, Button b3, Color color1, Color color2, Color color3)
        {
            // check all color possibilities
            if ((b1.BackColor == color1 && b2.BackColor == color2 && b3.BackColor == color3) ||
                (b1.BackColor == color1 && b2.BackColor == color3 && b3.BackColor == color2) ||
                (b1.BackColor == color2 && b2.BackColor == color1 && b3.BackColor == color3) ||
                (b1.BackColor == color2 && b2.BackColor == color3 && b3.BackColor == color1) ||
                (b1.BackColor == color3 && b2.BackColor == color1 && b3.BackColor == color2) ||
                (b1.BackColor == color3 && b2.BackColor == color2 && b3.BackColor == color1))
            {
                return true;
            }
            return false;
        }

        // Returns number of moveable pieces
        public int IsPlayerBlocked(Color teamColor)
        {
            Button[] gamemodeButtons = _game.GetIntersectionArray();
            int moveablePieces = 0;

            // if it's still Placing phase, player isn't blocked
            if (_game.isTeam1Turn && _game.benchMenCountTeam1 > 0)
            {
                return _game.menCountTeam1;
            }
            else if (!_game.isTeam1Turn && _game.benchMenCountTeam2 > 0)
            {
                return _game.menCountTeam2;
            }

            // check each team's buttons if they have a spot available
            foreach (Button button in gamemodeButtons)
            {
                if (button.BackColor == teamColor)
                {
                    // try to display available buttons
                    _board.DisplayAvailableButtons(button);

                    // check if 'button' has an adjacent spot
                    foreach (Button btn in gamemodeButtons)
                    {
                        if (btn.BackColor == _game.availableButtonColor)
                        {
                            // button is a moveable piece, because we have found one free spot
                            moveablePieces++;
                            break;
                        }
                    }
                    _board.ChangeButtonsColor(_game.availableButtonColor, _game.originalButtonBackgroundColor);
                }
            }

            return moveablePieces;
        }

        public void CheckForWin()
        {
            // Check if one of the team's are unable to form a mill (have 2 pieces left) 
            if (_game.menCountTeam1 < Game.MillLength)
            {
                MessageBox.Show("Team 2 has won!\nWinning was achieved by reducing opponent's pieces to two.", "Game over!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _game.AskToPlayAgain();
            }
            else if (_game.menCountTeam2 < Game.MillLength)
            {
                MessageBox.Show("Team 1 has won!\nWinning was achieved by reducing opponent's pieces to two.", "Game over!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _game.AskToPlayAgain();
            }
            // Check if one of the team's are unable to move because they are blocked
            else if (IsPlayerBlocked(_game.colorTeam1) == 0 && _game.isTeam1Turn && !_board.IsCurrentlyEliminating())
            {
                MessageBox.Show("Team 2 has won!\nWinning was achieved by blocking opponent's moves.", "Game over!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _game.AskToPlayAgain();
            }
            else if (IsPlayerBlocked(_game.colorTeam2) == 0 && !_game.isTeam1Turn && !_board.IsCurrentlyEliminating())
            {
                MessageBox.Show("Team 1 has won!\nWinning was achieved by blocking opponent's moves.", "Game over!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _game.AskToPlayAgain();
            }
        }

        public bool IsGameOver()
        {
            if (_game.menCountTeam1 < Game.MillLength || _game.menCountTeam2 < Game.MillLength ||
                IsPlayerBlocked(_game.colorTeam1) == 0 && _game.isTeam1Turn && !_board.IsCurrentlyEliminating() ||
                IsPlayerBlocked(_game.colorTeam2) == 0 && !_game.isTeam1Turn && !_board.IsCurrentlyEliminating())
            {
                return true;
            }
            return false;
        }
    }
}

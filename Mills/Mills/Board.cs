using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mills
{
    internal class Board
    {
        public Button buttonToDiscard; // for discarding placed buttons from bench
        public Button buttonToMove; // when button is placed to new position, turn moved button to board's color

        Game _game;
        GameLogic _gameLogic;

        public Board(Game game, GameLogic gameLogic)
        {
            _game = game;
            _gameLogic = gameLogic;
        }

        // Changes buttons color from color 1 to color 2
        public void ChangeButtonsColor(Color color1, Color color2)
        {
            Button[] gamemodeButtons = _game.GetIntersectionArray();

            // rule: only non-forming mill buttons can be eliminated, unless there is no other buttons to eliminate
            if ((color1 == _game.colorTeam1 || color1 == _game.colorTeam2) && color2 == _game.eliminationButtonColor)
            {
                List<Button> nonMillsButtons = GetNonMillsButtons(gamemodeButtons, color1);

                // if there are any non-mills buttons
                if (nonMillsButtons.Count > 0)
                {
                    // turn them to elimination color
                    foreach (var button in nonMillsButtons)
                    {
                        if (button.BackColor == color1)
                        {
                            button.BackColor = color2;
                        }
                    }
                    return;
                }
            }
            // turn all color1 buttons to color2
            foreach (var button in gamemodeButtons)
            {
                if (button.BackColor == color1)
                {
                    button.BackColor = color2;
                }
            }
        }

        // Returns a list of non-forming mill buttons, if there aren't any, returns all buttons
        private List<Button> GetNonMillsButtons(Button[] gamemodeButtons, Color colorToEliminateFrom)
        {
            // if colorTeam1 is the one to eliminate from, get theirs buttons
            List<List<Button>> currentMillButtonsTeamList = colorToEliminateFrom == _game.colorTeam1 ? _gameLogic.millButtons2dListTeam1 : _gameLogic.millButtons2dListTeam2;

            // get mills buttons to List<Button>
            List<Button> millsButtons = new List<Button>();
            foreach (var mill in currentMillButtonsTeamList)
            {
                foreach (var button in mill)
                {
                    if (!millsButtons.Contains(button)) // if the button has been already added
                    {
                        millsButtons.Add(button);
                    }
                }
            }

            // get non-forming mill buttons
            List<Button> nonMillsButtons = new List<Button>();
            foreach (var button in gamemodeButtons)
            {
                // if button belongs to current team and isn't in a mill-forming buttons
                if (!millsButtons.Contains(button) && button.BackColor == colorToEliminateFrom)
                {
                    nonMillsButtons.Add(button);
                }
            }

            return nonMillsButtons;
        }

        // Based on phase of the game, displays available buttons
        public void DisplayAvailableButtons(Button clickedButton)
        {
            // Phase 3 - Flying
            if (clickedButton.BackColor == _game.colorTeam1 && _game.menCountTeam1 == Game.MillLength)
            {
                ChangeButtonsColor(_game.originalButtonBackgroundColor, _game.availableButtonColor);
            }
            else if (clickedButton.BackColor == _game.colorTeam2 && _game.menCountTeam2 == Game.MillLength)
            {
                ChangeButtonsColor(_game.originalButtonBackgroundColor, _game.availableButtonColor);
            }
            // Phase 2 - Moving
            else
            {
                DisplayAdjacentButtons(clickedButton);
            }
        }

        // Turns only adjacent buttons to the clickedButton green
        private void DisplayAdjacentButtons(Button clickedButton)
        {
            Button[,] millsPatterns = _game.GetMillsPatternsArray();

            // check each mill pattern
            for (int i = 0; i < millsPatterns.GetLength(0); i++)
            {
                Button button1 = millsPatterns[i, 0];
                Button button2 = millsPatterns[i, 1];
                Button button3 = millsPatterns[i, 2];

                // change adjacent buttons to clickedButtons's mill pattern to green color
                if (clickedButton == button1)
                {
                    if (button2.BackColor == _game.originalButtonBackgroundColor) button2.BackColor = _game.availableButtonColor;
                }
                else if (clickedButton == button2)
                {
                    if (button1.BackColor == _game.originalButtonBackgroundColor) button1.BackColor = _game.availableButtonColor;
                    if (button3.BackColor == _game.originalButtonBackgroundColor) button3.BackColor = _game.availableButtonColor;

                    // edge case scenario - SMM has 8 buttons that are in the middle, but can only form one mill and because of that they aren't found as adjecent
                    if (_game.gameMenCount == Game.MenCountSMM)
                    {
                        if (i % 2 == 0)
                        {
                            if (millsPatterns[i + 1, 1].BackColor == _game.originalButtonBackgroundColor)
                            {
                                millsPatterns[i + 1, 1].BackColor = _game.availableButtonColor;
                            }
                        }
                        else
                        {
                            if (millsPatterns[i - 1, 1].BackColor == _game.originalButtonBackgroundColor)
                            {
                                millsPatterns[i - 1, 1].BackColor = _game.availableButtonColor;
                            }
                        }
                    }
                }
                else if (clickedButton == button3)
                {
                    if (button2.BackColor == _game.originalButtonBackgroundColor) button2.BackColor = _game.availableButtonColor;
                }
            }
        }

        // Returns true if a player is currently eliminating from the board
        public bool IsCurrentlyEliminating()
        {
            foreach (Button button in _game.GetIntersectionArray())
            {
                if (button.BackColor == _game.eliminationButtonColor)
                {
                    return true;
                }
            }
            return false;
        }

        public void PlaceButton(Button clickedButton)
        {
            // place button
            clickedButton.BackColor = _game.isTeam1Turn ? _game.colorTeam1 : _game.colorTeam2;

            // if it's Placing phase - discard bench button
            if (buttonToDiscard != null)
            {
                buttonToDiscard.Visible = false;
                buttonToDiscard = null;
                if (_game.isTeam1Turn)
                {
                    _game.benchMenCountTeam1--;
                }
                else _game.benchMenCountTeam2--;
            }

            // if it's Moving phase - change moved button previous position's color back to board's color
            if (buttonToMove != null)
            {
                buttonToMove.BackColor = _game.originalButtonBackgroundColor;
                buttonToMove = null;
            }

            ChangeButtonsColor(_game.availableButtonColor, _game.originalButtonBackgroundColor);

            // after each move, check how many mills were formed
            int formedMillsCount = _gameLogic.CheckForMill();
            if (formedMillsCount > 0)
            {
                // show button's to the user who formed the mill that he can eliminate
                _game.wasButtonEliminated = false;
                HighlightRemovableButtons();
                if (formedMillsCount > 1 && _game.Text == "PvP") _gameLogic.wereTwoMillsFormed = true;
            }
            else // if mill wasnt formed, change the turn
            {
                _game.isTeam1Turn = !_game.isTeam1Turn;
            }
        }

        // Remember button that needs to be moved to a new position
        public void RememberButtonToMove(Button clickedButton)
        {
            ChangeButtonsColor(_game.availableButtonColor, _game.originalButtonBackgroundColor);
            buttonToMove = clickedButton;
            DisplayAvailableButtons(clickedButton);
        }

        // Shows buttons to the user who formed the mill that he can eliminate
        public void HighlightRemovableButtons()
        {
            Color opponentColor = _game.isTeam1Turn ? _game.colorTeam2 : _game.colorTeam1;
            ChangeButtonsColor(opponentColor, _game.eliminationButtonColor);
        }

        // Eliminates button the current player has clicked on
        public void EliminateButton(Button clickedButton)
        {
            // eliminate clicked button
            clickedButton.BackColor = _game.originalButtonBackgroundColor;

            // take away one man from opponent's team
            if (_game.isTeam1Turn)
            {
                _game.menCountTeam2--;
                _game.isTeam1Turn = false;
            }
            else
            {
                _game.menCountTeam1--;
                _game.isTeam1Turn = true;
            }

            // turn current team's red color back to their team's color
            Color currentTeamColor = _game.isTeam1Turn ? _game.colorTeam1 : _game.colorTeam2;
            ChangeButtonsColor(_game.eliminationButtonColor, currentTeamColor);
        }
    }
}

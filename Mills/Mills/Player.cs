using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mills
{
    internal class Player
    {
        Game _game;
        GameLogic _gameLogic;
        Board _board;

        public Player(Game game, GameLogic gameLogic, Board board)
        {
            _game = game;
            _gameLogic = gameLogic;
            _board = board;
        }

        public void PlayerMove(Button clickedButton)
        {
            // if button is placed or moved to a new position
            if (clickedButton.BackColor == _game.availableButtonColor && _game.wasButtonEliminated)
            {
                _board.PlaceButton(clickedButton);
                _gameLogic.CheckForWin(); // check if player hasn't been blocked
            }
            // if team's button to move is clicked
            else if (((clickedButton.BackColor == _game.colorTeam1 && _game.benchMenCountTeam1 == 0 && _game.isTeam1Turn) ||
                     (clickedButton.BackColor == _game.colorTeam2 && _game.benchMenCountTeam2 == 0 && !_game.isTeam1Turn && _game.Text == "PvP")) &&
                     _game.wasButtonEliminated)
            {
                _board.RememberButtonToMove(clickedButton);
            }
            // if button to be eliminated is clicked
            else if (clickedButton.BackColor == _game.eliminationButtonColor)
            {
                _board.EliminateButton(clickedButton);
                _gameLogic.CheckForWin(); // check if player can still form a mill
                _game.wasButtonEliminated = true;

                // check if 2 mills havent been formed in one go, if so, remove another button for the same team
                if (_gameLogic.wereTwoMillsFormed && _game.Text == "PvP")
                {
                    _game.wasButtonEliminated = false;
                    _game.isTeam1Turn = !_game.isTeam1Turn;
                    _board.HighlightRemovableButtons();
                    _gameLogic.wereTwoMillsFormed = false;
                }
            }
        }
    }
}

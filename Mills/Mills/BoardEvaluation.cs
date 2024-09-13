using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mills
{
    internal class BoardEvaluation
    {
        public const int winningGuaranteedAmount = 10000;

        Game _game;
        GameLogic _gameLogic;
        Board _board;

        public BoardEvaluation(Game game, GameLogic gameLogic, Board board)
        {
            _game = game;
            _gameLogic = gameLogic;
            _board = board;
        }

        public int EvaluateBoard(Color computersColor, Color opponentColor)
        {
            int score = 0;

            bool isMaximizing = true;

            // evaluate both teams board 
            score += EvaluateBoardBasedOnTeam(isMaximizing, computersColor, opponentColor); // maximize score for ai
            score += EvaluateBoardBasedOnTeam(!isMaximizing, opponentColor, computersColor); // minimize score for player

            // who has the control of the game, number of piece on the board
            score += computersColor == _game.colorTeam2 ? (_game.menCountTeam2 - _game.menCountTeam1) * 10 : 
                                                          (_game.menCountTeam1 - _game.menCountTeam2) * 10;

            // mobility
            int computerMobility = _gameLogic.IsPlayerBlocked(computersColor);
            int opponentMobility = _gameLogic.IsPlayerBlocked(opponentColor);
            score += computersColor == _game.colorTeam2 ? (computerMobility * 5 - opponentMobility * 5) : 
                                                          (opponentMobility * 5 - computerMobility * 5);

            return score;
        }

        private int EvaluateBoardBasedOnTeam(bool isMaximizing, Color computersColor, Color opponentColor)
        {
            int score = 0;

            List<List<Button>> currentTeamMills = computersColor == _game.colorTeam2 ? _gameLogic.millButtons2dListTeam2 : _gameLogic.millButtons2dListTeam1;

            Button[,] millsPatterns = _game.GetMillsPatternsArray();

            for (int i = 0; i < millsPatterns.GetLength(0); i++)
            {
                Button button1 = millsPatterns[i, 0];
                Button button2 = millsPatterns[i, 1];
                Button button3 = millsPatterns[i, 2];

                // three team buttons - mill has been formed
                if (_gameLogic.CheckMillPatternColors(button1, button2, button3, computersColor, computersColor, computersColor))
                {
                    // check if the mill is newly formed
                    if (_gameLogic.IsMillNewlyFormed(currentTeamMills, millsPatterns, i))
                    {
                        score += isMaximizing ? 150 : -150;
                    }
                    else score += isMaximizing ? 20 : -20;
                }

                // two team buttons and one available button - team can form a mill in next go
                if (_gameLogic.CheckMillPatternColors(button1, button2, button3, computersColor, computersColor, _game.originalButtonBackgroundColor))
                {
                    score += isMaximizing ? 25 : -25;
                }

                // one team button and two opponent buttons - team has blocked opponent from forming mill
                if (_gameLogic.CheckMillPatternColors(button1, button2, button3, computersColor, opponentColor, opponentColor))
                {
                    score += isMaximizing ? 80 : -80;
                }

                // one team button and two available buttons - strategic position
                if (_gameLogic.CheckMillPatternColors(button1, button2, button3, computersColor, _game.originalButtonBackgroundColor, _game.originalButtonBackgroundColor))
                {
                    score += isMaximizing ? 10 : -10;
                }
            }

            return score;
        }

        public int EvaluateWinningStates(int depth, bool isMaximizing, Color computersColor)
        {
            // Check if one of the team's are unable to form a mill (have 2 pieces left) 
            if (_game.menCountTeam2 < Game.MillLength)
            {
                return (computersColor == _game.colorTeam1 ? winningGuaranteedAmount + depth : -winningGuaranteedAmount - depth);
            }
            else if (_game.menCountTeam1 < Game.MillLength)
            {
                return (computersColor == _game.colorTeam1 ? -winningGuaranteedAmount - depth : winningGuaranteedAmount + depth);
            }
            // Check if one of the team's are unable to move because they are blocked
            else if (_gameLogic.IsPlayerBlocked(_game.colorTeam2) == 0 && isMaximizing)
            {
                return (computersColor == _game.colorTeam1 ? winningGuaranteedAmount + depth : -winningGuaranteedAmount - depth);
            }
            else if (_gameLogic.IsPlayerBlocked(_game.colorTeam1) == 0 && !isMaximizing)
            {
                return (computersColor == _game.colorTeam1 ? -winningGuaranteedAmount - depth : winningGuaranteedAmount + depth);
            }
            return 0;
        }
    }
}

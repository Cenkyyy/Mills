using System;

namespace Mills
{
    internal class Computer
    {
        // computer's buttons he chooses
        Button bestButtonToPlace = null;
        Button bestButtonToMove = null;
        Button bestButtonToEliminate = null;

        const int EliminateButtonAfter = 1000; // after 1000ms eliminates button
        int stepsForwardPlacingPhase = 1;
        int stepsForwardMovingPhase = 3;

        private Color _computersColor;

        Game _game;
        GameLogic _gameLogic;
        Board _board;
        BoardEvaluation _evaluation;

        System.Timers.Timer timer;

        public Computer(Game game, GameLogic gameLogic, Board board, BoardEvaluation evaluation)
        {
            _game = game;
            _gameLogic = gameLogic;
            _board = board;
            _evaluation = evaluation;

            timer = new System.Timers.Timer();
            timer.Interval = EliminateButtonAfter;
            timer.Elapsed += Timer_Elapsed;
        }

        // Returns button from computer's bench during Placing phase
        public Button ChooseBenchButton()
        {
            Button[] benchButtons = _computersColor == _game.colorTeam2 ? _game.benchButtonsTeam2 : _game.benchButtonsTeam1;

            foreach (Button button in benchButtons)
            {
                if (button.Visible)
                {
                    return button;
                }
            }
            return null;
        }

        private void ComputerPlacingPhase()
        {
            // choose button from bench in order
            _board.buttonToDiscard = ChooseBenchButton();

            (bestButtonToMove, bestButtonToPlace, bestButtonToEliminate) = FindBestMove();

            // place button
            if (bestButtonToPlace != null)
            {
                bestButtonToPlace.BackColor = _computersColor;
                bestButtonToPlace = null;
            }

            // remove the button from team 2's bench
            if (_board.buttonToDiscard != null)
            {
                _board.buttonToDiscard.Visible = false;
                if (_computersColor == _game.colorTeam1)
                {
                    _game.benchMenCountTeam1--;
                }
                else
                {
                    _game.benchMenCountTeam2--;
                }
                _board.buttonToDiscard = null;
            }
        }

        private void ComputerMovingPhase()
        {
            // find best move using minimax algorithm
            (bestButtonToMove, bestButtonToPlace, bestButtonToEliminate) = FindBestMove();

            // make move
            if (bestButtonToPlace != null)
            {
                bestButtonToPlace.BackColor = _computersColor;
                bestButtonToPlace = null;
            }
            if (bestButtonToMove != null)
            {
                bestButtonToMove.BackColor = _game.originalButtonBackgroundColor;
                bestButtonToMove = null;
            }
        }

        private void ComputerEliminate()
        {
            // eliminate button and check if the game has ended
            if (bestButtonToEliminate != null)
            {
                bestButtonToEliminate.BackColor = _game.originalButtonBackgroundColor;
                if (_computersColor == _game.colorTeam1)
                {
                    _game.menCountTeam2--;
                }
                else
                {
                    _game.menCountTeam1--;
                }
            }
        }

        public void ComputerMove(Color computersColor)
        {
            _computersColor = computersColor;

            Color opponentColor = _computersColor == _game.colorTeam1 ? _game.colorTeam2 : _game.colorTeam1;

            // Placing phase
            if ((_computersColor == _game.colorTeam2 && _game.benchMenCountTeam2 > 0) ||
                (_computersColor == _game.colorTeam1 && _game.benchMenCountTeam1 > 0))
            {
                ComputerPlacingPhase();
            }
            // Moving phase
            else
            {
                ComputerMovingPhase();
            }

            // update mills for team2 (in millButtons2dListTeam2) and eliminate if mills were found after 1 second
            int millsFormed = _gameLogic.CheckForMill();
            if (millsFormed > 0)
            {
                timer.Start();
            }

            // check if team 1 is blocked
            if (_gameLogic.IsPlayerBlocked(opponentColor) == 0)
            {
                string winningTeam = _computersColor == _game.colorTeam2 ? "Team 2" : "Team 1";

                MessageBox.Show($"{winningTeam} has won!\nWinning was achieved by blocking opponent's moves.", "Game over!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _game.AskToPlayAgain();
                return;
            }

            _game.isTeam1Turn = !_game.isTeam1Turn;
        }

        // Eliminates button computer has chosen 1 second after making his move
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            ComputerEliminate();
            _gameLogic.CheckForWin();
        }

        private void SimulatePlacing(Button buttonToPlace, Color currentTeamColor)
        {
            // place button and remove one piece from the bench
            buttonToPlace.BackColor = currentTeamColor;
            if (currentTeamColor == _game.colorTeam1)
            {
                _game.benchMenCountTeam1--;
            }
            else
            {
                _game.benchMenCountTeam2--;
            }
        }

        private void UndoPlacing(Button buttonToPlace, Color currentTeamColor)
        {
            // revert button to original color and add one piece back to the bench
            buttonToPlace.BackColor = _game.originalButtonBackgroundColor;
            if (currentTeamColor == _game.colorTeam1)
            {
                _game.benchMenCountTeam1++;
            }
            else
            {
                _game.benchMenCountTeam2++;
            }
        }

        private void SimulateMove(Button buttonToMove, Button buttonToPlace, Color currentTeamColor)
        {
            // move piece from buttonToMove to buttonToPlace
            buttonToMove.BackColor = _game.originalButtonBackgroundColor;
            buttonToPlace.BackColor = currentTeamColor;
            _board.ChangeButtonsColor(_game.availableButtonColor, _game.originalButtonBackgroundColor);
        }

        private void UndoMove(Button buttonToMove, Button buttonToPlace, Color currentTeamColor)
        {
            // revert move
            buttonToMove.BackColor = currentTeamColor;
            buttonToPlace.BackColor = _game.originalButtonBackgroundColor;
        }

        private void SimulateElimination(Button buttonToEliminate, Color colorToEliminateFrom)
        {
            // eliminate button and reduce the number of men
            buttonToEliminate.BackColor = _game.originalButtonBackgroundColor;
            if (colorToEliminateFrom == _game.colorTeam1)
            {
                _game.menCountTeam1--;
            }
            else
            {
                _game.menCountTeam2--;
            }
            _board.ChangeButtonsColor(_game.eliminationButtonColor, colorToEliminateFrom);
        }

        private void UndoElimination(Button buttonToEliminate, Color colorToEliminateFrom)
        {
            // revert elimination and add man back
            buttonToEliminate.BackColor = colorToEliminateFrom;
            if (colorToEliminateFrom == _game.colorTeam1)
            {
                _game.menCountTeam1++;
            }
            else
            {
                _game.menCountTeam2++;
            }
            _board.ChangeButtonsColor(colorToEliminateFrom, _game.eliminationButtonColor);
        }

        // Returns the best button to move and best button to place on
        public (Button, Button, Button) FindBestMove()
        {
            Color opponentColor = _computersColor == _game.colorTeam2 ? _game.colorTeam1 : _game.colorTeam2;

            Button[] gamemodeButtons = _game.GetIntersectionArray();

            int bestScore = int.MinValue;
            Button bestButtonToPlaceOn = null;
            Button bestButtonToMove = null;
            Button bestButtonToEliminate = null;

            int depth;

            if ((_game.benchMenCountTeam2 > 0 && _computersColor == _game.colorTeam2) ||
                (_game.benchMenCountTeam1 > 0 && _computersColor == _game.colorTeam1)) // placing phase
            {
                depth = stepsForwardPlacingPhase;

                foreach (Button buttonToPlace in gamemodeButtons) // check each button
                {
                    if (buttonToPlace.BackColor == _game.originalButtonBackgroundColor) // if can be placed on
                    {
                        SimulatePlacing(buttonToPlace, _computersColor);

                        int millsFormed = _gameLogic.IsFormingMillWithButton(buttonToPlace, _computersColor); // check for mill and simulate eliminating
                        for (int i = 0; i < millsFormed; i++)
                        {
                            _board.ChangeButtonsColor(opponentColor, _game.eliminationButtonColor);

                            foreach (Button buttonToEliminate in gamemodeButtons) // check each button
                            {
                                if (buttonToEliminate.BackColor == _game.eliminationButtonColor) // if can be eliminated
                                {
                                    SimulateElimination(buttonToEliminate, opponentColor);

                                    int eliminatingScore = Minimax(depth - 1, false, int.MinValue, int.MaxValue, _computersColor, opponentColor);
                                    //MessageBox.Show(eliminatingScore.ToString() + "\nBest score so far: " + bestScore.ToString());
                                    
                                    UndoElimination(buttonToEliminate, opponentColor);

                                    if (eliminatingScore > bestScore) // update scores
                                    {
                                        bestScore = eliminatingScore;
                                        bestButtonToPlaceOn = buttonToPlace;
                                        bestButtonToEliminate = buttonToEliminate;
                                    }
                                }
                            }

                            _board.ChangeButtonsColor(_game.eliminationButtonColor, opponentColor);
                        }

                        // no mill found
                        if (millsFormed == 0)
                        {
                            int score = Minimax(depth - 1, false, int.MinValue, int.MaxValue, _computersColor, opponentColor);
                            //MessageBox.Show(score.ToString() + "\nBest score so far: " + bestScore.ToString());
                            
                            if (score > bestScore) // update scores
                            {
                                bestScore = score;
                                bestButtonToPlaceOn = buttonToPlace;
                                bestButtonToEliminate = null;
                            }
                        }
                        UndoPlacing(buttonToPlace, _computersColor);
                    }
                }
            }
            else // moving phase
            {
                depth = stepsForwardMovingPhase;

                foreach (Button buttonToMove in gamemodeButtons) // check each button
                {
                    if (buttonToMove.BackColor == _computersColor) // if can be moved
                    {
                        _board.DisplayAvailableButtons(buttonToMove);

                        foreach (Button buttonToPlace in gamemodeButtons) // check each button
                        {
                            if (buttonToPlace.BackColor == _game.availableButtonColor) // if can be moved to
                            {
                                SimulateMove(buttonToMove, buttonToPlace, _computersColor);

                                int millsFormed = _gameLogic.IsFormingMillWithButton(buttonToPlace, _computersColor); // check for mill and simulate eliminating
                                for (int i = 0; i < millsFormed; i++)
                                {
                                    _board.ChangeButtonsColor(opponentColor, _game.eliminationButtonColor);

                                    foreach (Button buttonToEliminate in gamemodeButtons) // check each button
                                    {
                                        if (buttonToEliminate.BackColor == _game.eliminationButtonColor) // if can be eliminated
                                        {
                                            SimulateElimination(buttonToEliminate, opponentColor);

                                            int eliminatingScore = Minimax(depth - 1, false, int.MinValue, int.MaxValue, _computersColor, opponentColor);
                                            //MessageBox.Show(eliminatingScore.ToString() + "\nBest score so far: " + bestScore.ToString());
                                            
                                            UndoElimination(buttonToEliminate, opponentColor);

                                            if (eliminatingScore > bestScore) // update scores
                                            {
                                                bestScore = eliminatingScore;
                                                bestButtonToPlaceOn = buttonToPlace;
                                                bestButtonToMove = buttonToMove;
                                                bestButtonToEliminate = buttonToEliminate;
                                            }
                                        }
                                    }

                                    _board.ChangeButtonsColor(_game.eliminationButtonColor, opponentColor);
                                }

                                if (millsFormed == 0) // no mill found
                                {
                                    int score = Minimax(depth - 1, false, int.MinValue, int.MaxValue, _computersColor, opponentColor);
                                    //MessageBox.Show(score.ToString() + "\nBest score so far: " + bestScore.ToString());

                                    if (score > bestScore) // update scores
                                    {
                                        bestScore = score;
                                        bestButtonToPlaceOn = buttonToPlace;
                                        bestButtonToMove = buttonToMove;
                                        bestButtonToEliminate = null;
                                    }
                                }
                                 UndoMove(buttonToMove, buttonToPlace, _computersColor);
                                _board.DisplayAvailableButtons(buttonToMove);
                            }
                        }

                        _board.ChangeButtonsColor(_game.availableButtonColor, _game.originalButtonBackgroundColor);
                    }
                }
            }

            return (bestButtonToMove, bestButtonToPlaceOn, bestButtonToEliminate);
        }

        // Recursively simulate the game 3 steps ahead and return the highest evaluated score
        private int Minimax(int depth, bool isMaximizing, int alpha, int beta, Color computersColor, Color opponentColor)
        {
            // if someone has won, return the result
            int result = _evaluation.EvaluateWinningStates(depth, isMaximizing, computersColor);
            if (result != 0)
            {
                return result;
            }
            // evaluate current board state
            if (depth == 0)
            {
                return _evaluation.EvaluateBoard(computersColor, opponentColor);
            }

            if (isMaximizing) // computer's turn
            {
                return Maximize(depth, alpha, beta, computersColor, opponentColor);
            }
            else // player's turn
            {
                return Minimize(depth, alpha, beta, computersColor, opponentColor);
            }
        }

        // Part of minimax algorthm playing as computer when simulating the game
        private int Maximize(int depth, int alpha, int beta, Color computersColor, Color opponentColor)
        {
            Button[] gamemodeButtons = _game.GetIntersectionArray();
            int maxScore = int.MinValue;

            if ((_game.benchMenCountTeam2 > 0 && _computersColor == _game.colorTeam2) ||
                (_game.benchMenCountTeam1 > 0 && _computersColor == _game.colorTeam1)) // placing phase
            {
                foreach (Button buttonToPlace in gamemodeButtons)
                {
                    if (buttonToPlace.BackColor == _game.originalButtonBackgroundColor)
                    {
                        SimulatePlacing(buttonToPlace, computersColor);

                        int millsFormed = _gameLogic.IsFormingMillWithButton(buttonToPlace, computersColor);
                        for (int i = 0; i < millsFormed; i++)
                        {
                            _board.ChangeButtonsColor(opponentColor, _game.eliminationButtonColor);

                            foreach (Button buttonToEliminate in gamemodeButtons)
                            {
                                if (buttonToEliminate.BackColor == _game.eliminationButtonColor)
                                {
                                    SimulateElimination(buttonToEliminate, opponentColor);
                                    int eliminatingScore = Minimax(depth - 1, false, alpha, beta, computersColor, opponentColor);
                                    UndoElimination(buttonToEliminate, opponentColor);

                                    maxScore = Math.Max(eliminatingScore, maxScore);
                                    alpha = Math.Max(alpha, eliminatingScore);

                                    if (beta <= alpha)
                                    {
                                        break;
                                    }
                                }
                            }

                            _board.ChangeButtonsColor(_game.eliminationButtonColor, opponentColor);

                            if (beta <= alpha)
                            {
                                break;
                            }
                        }

                        if (millsFormed == 0)
                        {
                            int score = Minimax(depth - 1, false, alpha, beta, computersColor, opponentColor);

                            maxScore = Math.Max(score, maxScore);
                            alpha = Math.Max(alpha, score);
                        }
                        UndoPlacing(buttonToPlace, computersColor);

                        if (beta <= alpha)
                        {
                            return maxScore;
                        }
                    }
                }
            }
            else // moving phase
            {
                foreach (Button buttonToMove in gamemodeButtons)
                {
                    if (buttonToMove.BackColor == computersColor)
                    {
                        _board.DisplayAvailableButtons(buttonToMove);

                        foreach (Button buttonToPlace in gamemodeButtons)
                        {
                            if (buttonToPlace.BackColor == _game.availableButtonColor)
                            {
                                SimulateMove(buttonToMove, buttonToPlace, computersColor);

                                int millsFormed = _gameLogic.IsFormingMillWithButton(buttonToPlace, computersColor);
                                for (int i = 0; i < millsFormed; i++)
                                {
                                    _board.ChangeButtonsColor(opponentColor, _game.eliminationButtonColor);

                                    foreach (Button buttonToEliminate in gamemodeButtons)
                                    {
                                        if (buttonToEliminate.BackColor == _game.eliminationButtonColor)
                                        {
                                            SimulateElimination(buttonToEliminate, opponentColor);
                                            int eliminatingScore = Minimax(depth - 1, false, alpha, beta, computersColor, opponentColor);
                                            UndoElimination(buttonToEliminate, opponentColor);

                                            maxScore = Math.Max(eliminatingScore, maxScore);
                                            alpha = Math.Max(alpha, eliminatingScore);

                                            if (beta <= alpha)
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    _board.ChangeButtonsColor(_game.eliminationButtonColor, opponentColor);

                                    if (beta <= alpha)
                                    {
                                        break;
                                    }
                                }

                                if (millsFormed == 0)
                                {
                                    int score = Minimax(depth - 1, false, alpha, beta, computersColor, opponentColor);

                                    maxScore = Math.Max(score, maxScore);
                                    alpha = Math.Max(alpha, score);
                                }
                                UndoMove(buttonToMove, buttonToPlace, computersColor);
                                _board.DisplayAvailableButtons(buttonToMove);

                                if (beta <= alpha)
                                {
                                    break;
                                }
                            }
                        }

                        _board.ChangeButtonsColor(_game.availableButtonColor, _game.originalButtonBackgroundColor);

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }

            return maxScore;
        }

        // Part of minimax algorthm playing as player when simulating the game
        private int Minimize(int depth, int alpha, int beta, Color computersColor, Color opponentColor)
        {
            Button[] gamemodeButtons = _game.GetIntersectionArray();
            int minScore = int.MaxValue;

            if ((_game.benchMenCountTeam2 > 0 && _computersColor == _game.colorTeam2) ||
                (_game.benchMenCountTeam1 > 0 && _computersColor == _game.colorTeam1)) // placing phase
            {
                foreach (Button buttonToPlace in gamemodeButtons)
                {
                    if (buttonToPlace.BackColor == _game.originalButtonBackgroundColor)
                    {
                        SimulatePlacing(buttonToPlace, opponentColor);
                        
                        int millsFormed = _gameLogic.IsFormingMillWithButton(buttonToPlace, opponentColor);
                        for (int i = 0; i < millsFormed; i++)
                        {
                            _board.ChangeButtonsColor(computersColor, _game.eliminationButtonColor);

                            foreach (Button buttonToEliminate in gamemodeButtons)
                            {
                                if (buttonToEliminate.BackColor == _game.eliminationButtonColor)
                                {
                                    SimulateElimination(buttonToEliminate, computersColor);
                                    int eliminatingScore = Minimax(depth - 1, true, alpha, beta, computersColor, opponentColor);
                                    UndoElimination(buttonToEliminate, computersColor);

                                    minScore = Math.Min(eliminatingScore, minScore);
                                    beta = Math.Min(beta, eliminatingScore);

                                    if (beta <= alpha)
                                    {
                                        break;
                                    }
                                }
                            }

                            _board.ChangeButtonsColor(_game.eliminationButtonColor, computersColor);

                            if (beta <= alpha)
                            {
                                break;
                            }
                        }

                        if (millsFormed == 0)
                        {
                            int score = Minimax(depth - 1, true, alpha, beta, computersColor, opponentColor);

                            minScore = Math.Min(score, minScore);
                            beta = Math.Min(beta, score);
                        }
                        UndoPlacing(buttonToPlace, opponentColor);

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }
            else // moving phase
            {
                foreach (Button buttonToMove in gamemodeButtons)
                {
                    if (buttonToMove.BackColor == opponentColor)
                    {
                        _board.DisplayAvailableButtons(buttonToMove);

                        foreach (Button buttonToPlace in gamemodeButtons)
                        {
                            if (buttonToPlace.BackColor == _game.availableButtonColor)
                            {
                                SimulateMove(buttonToMove, buttonToPlace, opponentColor);

                                int millsFormed = _gameLogic.IsFormingMillWithButton(buttonToPlace, opponentColor);
                                for (int i = 0; i < millsFormed; i++)
                                {
                                    _board.ChangeButtonsColor(computersColor, _game.eliminationButtonColor);

                                    foreach (Button buttonToEliminate in gamemodeButtons)
                                    {
                                        if (buttonToEliminate.BackColor == _game.eliminationButtonColor)
                                        {
                                            SimulateElimination(buttonToEliminate, computersColor);
                                            int eliminatingScore = Minimax(depth - 1, true, alpha, beta, computersColor, opponentColor);
                                            UndoElimination(buttonToEliminate, computersColor);

                                            minScore = Math.Min(eliminatingScore, minScore);
                                            beta = Math.Min(beta, eliminatingScore);

                                            if (beta <= alpha)
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    _board.ChangeButtonsColor(_game.eliminationButtonColor, computersColor);

                                    if (beta <= alpha)
                                    {
                                        break;
                                    }
                                }

                                if (millsFormed == 0)
                                {
                                    int score = Minimax(depth - 1, true, alpha, beta, computersColor, opponentColor);

                                    minScore = Math.Min(score, minScore);
                                    beta = Math.Min(beta, score);
                                }
                                UndoMove(buttonToMove, buttonToPlace, opponentColor);
                                _board.DisplayAvailableButtons(buttonToMove);

                                if (beta <= alpha)
                                {
                                    break;
                                }
                            }
                        }

                        _board.ChangeButtonsColor(_game.availableButtonColor, _game.originalButtonBackgroundColor);

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }

            return minScore;
        }
    }
}

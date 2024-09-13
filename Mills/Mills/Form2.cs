namespace Mills
{
    public partial class Game : Form
    {
        /* TMM -> Three Mens Morris
         * SMM -> Six Mens Morris
         * NMM -> Nine Mens Morris
         */

        private const int ComputerMakeMoveAfter = 1000; // computer makes move after 1000ms

        public const int MillLength = 3;
        
        // number of in-game buttons
        public const int IntersectionsCountTMM = 9;
        public const int IntersectionsCountSMM = 16;
        public const int IntersectionsCountNMM = 24;
        
        // number of in-game team's buttons
        public const int MenCountTMM = 3;
        public const int MenCountSMM = 6;
        public const int MenCountNMM = 9;
        
        // number of patterns that can form a mill
        public const int MillsPatternsCountTMM = 8;
        public const int MillsPatternsCountSMM = 8;
        public const int MillsPatternsCountNMM = 16;

        // button patterns forming a mill
        Button[,] millsPatternsTMM;
        Button[,] millsPatternsSMM;
        Button[,] millsPatternsNMM;
        
        // in-game buttons
        Button[] intersectionButtonsTMM;
        Button[] intersectionButtonsSMM;
        Button[] intersectionButtonsNMM;
        
        // team bench buttons
        public Button[] benchButtonsTeam1;
        public Button[] benchButtonsTeam2;

        public int gameMenCount;
        public int benchMenCountTeam1;
        public int benchMenCountTeam2;
        public int menCountTeam1;
        public int menCountTeam2;
        public bool isTeam1Turn = true; // by default, team1 starts
        public bool wasButtonEliminated = true;

        // possible button colors
        public Color availableButtonColor = Color.YellowGreen;
        public Color originalButtonBackgroundColor = Color.PapayaWhip;
        public Color colorTeam1 = Color.BurlyWood;
        public Color colorTeam2 = Color.Chocolate;
        public Color eliminationButtonColor = Color.Red;

        Board board;
        GameLogic gameLogic;
        Computer computer;
        Player player;
        BoardEvaluation evaluation;

        public System.Timers.Timer timer;

        public Game()
        {
            InitializeComponent();
            InitializeGame();

            // Ensure the application exits fully when this form is closed
            this.FormClosing += Form2_FormClosing;
        }

        private void InitializeGame()
        {
            InitializeButtons();

            gameLogic = new GameLogic(this);
            board = new Board(this, gameLogic);
            gameLogic.SetBoard(board);
            evaluation = new BoardEvaluation(this, gameLogic, board);
            computer = new Computer(this, gameLogic, board, evaluation);
            player = new Player(this, gameLogic, board);

            timer = new System.Timers.Timer();
            timer.Interval = ComputerMakeMoveAfter;
            timer.Elapsed += Timer_Elapsed;
        }

        private void InitializeButtons()
        {
            intersectionButtonsTMM = InitializeIntersectionButtonsTMM();
            intersectionButtonsSMM = InitializeIntersectionButtonsSMM();
            intersectionButtonsNMM = InitializeIntersectionButtonsNMM();

            benchButtonsTeam1 = InitializeBenchButtonsTeam1();
            benchButtonsTeam2 = InitializeBenchButtonsTeam2();

            millsPatternsTMM = InitializeMillsPatternsTMM();
            millsPatternsSMM = InitializeMillsPatternsSMM();
            millsPatternsNMM = InitializeMillsPatternsNMM();
        }

        private Button[] InitializeIntersectionButtonsTMM()
        {
            return intersectionButtonsTMM = new Button[IntersectionsCountTMM]
            {
                TMM1, TMM2, TMM3,
                TMM4, TMM5, TMM6,
                TMM7, TMM8, TMM9
            };
        }

        private Button[] InitializeIntersectionButtonsSMM()
        {
            return intersectionButtonsSMM = new Button[IntersectionsCountSMM]
            {
                NMM1, SMM9,  NMM3,  SMM1,
                SMM2, SMM3,  NMM10, SMM4,
                SMM5, NMM15, SMM6,  SMM7,
                SMM8, NMM22, SMM10, NMM24
            };
        }

        private Button[] InitializeIntersectionButtonsNMM()
        {
            return intersectionButtonsNMM = new Button[IntersectionsCountNMM]
            {
                NMM1,  NMM2,  NMM3,  NMM4,
                NMM5,  NMM6,  NMM7,  NMM8,
                NMM9,  NMM10, NMM11, NMM12,
                NMM13, NMM14, NMM15, NMM16,
                NMM17, NMM18, NMM19, NMM20,
                NMM21, NMM22, NMM23, NMM24
            };
        }

        private Button[] InitializeBenchButtonsTeam1()
        {
            return benchButtonsTeam1 = new Button[MenCountNMM]
            {
                bench1, bench2, bench3,
                bench4, bench5, bench6,
                bench7, bench8, bench9
            };
        }

        private Button[] InitializeBenchButtonsTeam2()
        {
            return benchButtonsTeam2 = new Button[MenCountNMM]
            {
                bench2_1, bench2_2, bench2_3,
                bench2_4, bench2_5, bench2_6,
                bench2_7, bench2_8, bench2_9
            };
        }

        private Button[,] InitializeMillsPatternsTMM()
        {
            return millsPatternsTMM = new Button[MillsPatternsCountTMM, MillLength]
            {
                { TMM1, TMM2, TMM3 }, { TMM4, TMM5, TMM6 },
                { TMM7, TMM8, TMM9 }, { TMM1, TMM4, TMM7 },
                { TMM2, TMM5, TMM8 }, { TMM3, TMM6, TMM9 },
                { TMM1, TMM5, TMM9 }, { TMM7, TMM5, TMM3 }
            };
        }

        private Button[,] InitializeMillsPatternsSMM()
        {
            return millsPatternsSMM = new Button[MillsPatternsCountSMM, MillLength]
            {
                { NMM1, SMM9,  NMM3  }, { SMM1,  SMM2,  SMM3  }, 
                { SMM6, SMM7,  SMM8  }, { NMM22, SMM10, NMM24 },
                { NMM1, NMM10, NMM22 }, { SMM1,  SMM4,  SMM6  },
                { SMM3, SMM5,  SMM8  }, { NMM3,  NMM15, NMM24 }
            };
        }

        private Button[,] InitializeMillsPatternsNMM()
        {
            return millsPatternsNMM = new Button[MillsPatternsCountNMM, MillLength]
            {
                { NMM1,  NMM2,  NMM3  }, { NMM4,  NMM5,  NMM6  },
                { NMM7,  NMM8,  NMM9  }, { NMM10, NMM11, NMM12 },
                { NMM13, NMM14, NMM15 }, { NMM16, NMM17, NMM18 },
                { NMM19, NMM20, NMM21 }, { NMM22, NMM23, NMM24 },
                { NMM1,  NMM10, NMM22 }, { NMM4,  NMM11, NMM19 },
                { NMM7,  NMM12, NMM16 }, { NMM2,  NMM5,  NMM8  },
                { NMM17, NMM20, NMM23 }, { NMM9,  NMM13, NMM18 },
                { NMM6,  NMM14, NMM21 }, { NMM3,  NMM15, NMM24 }
            };
        }

        // Set starting variables to correct number based on gamemode
        private void InitializeGameMen(int gamemodeIndex)
        {
            int gamemodeMenCount = GetGameMenCount(gamemodeIndex);

            gameMenCount = menCountTeam1 = menCountTeam2 = benchMenCountTeam1 = benchMenCountTeam2 = gamemodeMenCount;
        }

        public void InitializeStartingPlayer()
        {
            DialogResult startingPlayer = MessageBox.Show("You play as Team 1.\nWould you like to start?",
                                                          "Choose starting player",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question);

            if (startingPlayer == DialogResult.Yes)
            {
                isTeam1Turn = true;
            }
            else
            {
                isTeam1Turn = false;
                if (Text == "PvC") // make first move if computer starts
                {
                    computer.ComputerMove(colorTeam2);
                }
            }
        }


        public int GetGameMenCount(int gamemodeIndex)
        {
            switch (gamemodeIndex)
            {
                case 0:
                    return MenCountTMM;
                case 1:
                    return MenCountSMM;
                case 2:
                    return MenCountNMM;
                default:
                    throw new ArgumentException("Invalid gamemode");
            }
        }

        // Turns gamemode's buttons visible
        private void ShowIntersectionButtons()
        {
            // get gamemode's in-game buttons
            Button[] intersectionButtonsArray = GetIntersectionArray();

            foreach (var intersectionButton in intersectionButtonsArray)
            {
                intersectionButton.Visible = true;
            }
        }

        // Sets correct amount of bench buttons visible
        private void HideExtraBenchMen()
        {
            for (int i = MenCountNMM - 1; i >= 0; i--)
            {
                if (i >= gameMenCount)
                {
                    benchButtonsTeam1[i].Visible = false;
                    benchButtonsTeam2[i].Visible = false;
                }
                else break;
            }
        }

        public void InitializeGamemode(int gamemodeIndex)
        {
            InitializeGameMen(gamemodeIndex);

            // show gamemode's in-game and bench buttons
            ShowIntersectionButtons();
            HideExtraBenchMen();
        }

        // Returns an array of gamemode's mills formations
        public Button[,] GetMillsPatternsArray()
        {
            switch (gameMenCount)
            {
                case MenCountTMM:
                    return millsPatternsTMM;
                case MenCountSMM:
                    return millsPatternsSMM;
                case MenCountNMM:
                    return millsPatternsNMM;
                default:
                    throw new ArgumentException("Invalid gamemode");
            }
        }

        // Returns an array of gamemode's in-game buttons
        public Button[] GetIntersectionArray()
        {
            switch (gameMenCount)
            {
                case MenCountTMM:
                    return intersectionButtonsTMM;
                case MenCountSMM:
                    return intersectionButtonsSMM;
                case MenCountNMM:
                    return intersectionButtonsNMM;
                default:
                    throw new ArgumentException("Invalid gamemode");
            }
        }

        // After the game has finished, ask if the user wants to play again
        public void AskToPlayAgain()
        {
            DialogResult result = MessageBox.Show(
                "What would you like to do?\n" +
                "\n- Reset game (Yes)\n" +
                "- Go back to Menu (No)\n" +
                "- Exit the game (Cancel)",
                "Game Options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ResetGame(true); // reset the same gamemode, same opponent
            }
            else if (result == DialogResult.No)
            {
                ResetGame(false); // go back to menu
            }
            else if (result == DialogResult.Cancel)
            {
                Application.Exit(); // close the application
            }
        }

        private void ResetGame(bool resetGame)
        {
            // perform computer move on a UI thread to prevent thread-crossing error
            Invoke((MethodInvoker)delegate
            {
                if (resetGame) // restart the same gamemmode and opponent
                {
                    // reset in-game buttons
                    foreach (var button in GetIntersectionArray())
                    {
                        button.BackColor = originalButtonBackgroundColor;
                        button.Visible = true;
                    }

                    // reset bench buttons
                    for (int i = 0; i < benchButtonsTeam1.Length; i++)
                    {
                        benchButtonsTeam1[i].Visible = true;
                        benchButtonsTeam2[i].Visible = true;
                    }

                    // show correct number of bench men based on last gamemode
                    HideExtraBenchMen();

                    // reset all variables
                    gameLogic.millButtons2dListTeam1.Clear();
                    gameLogic.millButtons2dListTeam2.Clear();
                    gameLogic.wereTwoMillsFormed = false;
                    benchMenCountTeam1 = gameMenCount;
                    benchMenCountTeam2 = gameMenCount;
                    menCountTeam1 = gameMenCount;
                    menCountTeam2 = gameMenCount;
                    isTeam1Turn = true;
                    board.buttonToMove = null;
                    board.buttonToDiscard = null;

                    if (Text != "CvC")
                    {
                        InitializeStartingPlayer();
                    }
                    else timer.Start();
                }
                else // go back to menu
                {
                    this.Hide(); // hide the current Game form
                    Menu menu = new Menu();
                    menu.Show(); // show the Menu form
                }
            });
        }

        private void Team1_Bench_Buttons_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            board.ChangeButtonsColor(availableButtonColor, originalButtonBackgroundColor);
            if (clickedButton.Visible && isTeam1Turn && wasButtonEliminated)
            {
                board.buttonToDiscard = clickedButton;
                board.ChangeButtonsColor(originalButtonBackgroundColor, availableButtonColor);
            }
        }

        private void Team2_Bench_Buttons_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            board.ChangeButtonsColor(availableButtonColor, originalButtonBackgroundColor);
            if (clickedButton.Visible && !isTeam1Turn && wasButtonEliminated)
            {
                board.buttonToDiscard = clickedButton;
                board.ChangeButtonsColor(originalButtonBackgroundColor, availableButtonColor);
            }
        }

        private void InGameButtons_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // check if the clicked button is valid
            if (!clickedButton.Visible)
            {
                return;
            }

            player.PlayerMove(clickedButton);

            // computer makes move if he is chosen as the oppponent
            if (Text == "PvC" && !isTeam1Turn)
            {
                timer.Start();
            }
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            // perform computer move on a UI thread to prevent thread-crossing error
            Invoke((MethodInvoker)delegate
            {
                if (Text == "PvC")
                {
                    computer.ComputerMove(colorTeam2);
                }
            });
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

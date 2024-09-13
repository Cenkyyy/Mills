
namespace Mills
{
    public partial class Menu : Form
    {
        /* TMM -> Three Mens Morris
         * SMM -> Six Mens Morris
         * NMM -> Nine Mens Morris
         */

        const int GamemodeIndexTMM = 0;
        const int GamemodeIndexSMM = 1;
        const int GamemodeIndexNMM = 2;

        const int GamemodesCount = 3;

        CheckBox[] gamemodesArray;

        Game game = new Game();

        public Menu()
        {
            InitializeComponent();
            gamemodesArray = InitializeGamemodes();

            // Ensure the application exits fully when this form is closed
            this.FormClosing += Menu_FormClosing;
        }

        private CheckBox[] InitializeGamemodes()
        {
            return gamemodesArray = new CheckBox[GamemodesCount] { checkBoxTMM, checkBoxSMM, checkBoxNMM };
        }

        // Uncheck other gamemodes if one of the gamemodes is chosen (checked)
        private void UncheckGamemodeCheckBoxes(int gamemodeIndex)
        {
            for (int i = 0; i < gamemodesArray.Length; i++)
            {
                if (i != gamemodeIndex && gamemodesArray[gamemodeIndex].Checked)
                {
                    gamemodesArray[i].Checked = false;
                }
            }
        }

        // Based on clicked gamemode (checkBox), uncheck other gamemodes (checkBoxes)
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox clickedCheckBox = (CheckBox)sender;

            int gamemodeIndex;

            switch (clickedCheckBox.Name)
            {
                case "checkBoxTMM":
                    gamemodeIndex = GamemodeIndexTMM;
                    break;
                case "checkBoxSMM":
                    gamemodeIndex = GamemodeIndexSMM;
                    break;
                case "checkBoxNMM":
                    gamemodeIndex = GamemodeIndexNMM;
                    break;
                default:
                    throw new ArgumentException("Invalid gamemode");
            }

            UncheckGamemodeCheckBoxes(gamemodeIndex);
        }

        // Return true if one of the gamemodes has been chosen and display gamemode's background image and buttons
        private bool CreateGame()
        {
            for (int i = 0; i < gamemodesArray.Length; i++)
            {
                if (gamemodesArray[i].Checked)
                {
                    this.Hide(); // hide current form
                    game.Show(); // display game form

                    int gamemodeIndex;

                    switch (i)
                    {
                        case GamemodeIndexTMM:
                            game.BackgroundImage = Properties.Resources.three_man_morris_board;
                            gamemodeIndex = GamemodeIndexTMM;
                            break;
                        case GamemodeIndexSMM:
                            game.BackgroundImage = Properties.Resources.six_man_morris_board;
                            gamemodeIndex = GamemodeIndexSMM;
                            break;
                        case GamemodeIndexNMM:
                            game.BackgroundImage = Properties.Resources.nine_man_morris_board;
                            gamemodeIndex = GamemodeIndexNMM;
                            break;
                        default:
                            throw new ArgumentException("Invalid gamemode");
                    }
                    // initialize buttons based on gamemode
                    game.InitializeGamemode(gamemodeIndex);

                    game.BackgroundImageLayout = ImageLayout.Stretch;
                    return true; // return that gamemode has been chosen
                }
            }
            MessageBox.Show("Please choose one of the gamemodes first.", "Menu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false; // return that gamemode has not been chosen
        }

        private void Player_vs_Player_Button_Click(object sender, EventArgs e)
        {
            if (CreateGame())
            {
                game.Text = "PvP";
                game.InitializeStartingPlayer();
            }
        }

        private void Player_vs_Computer_Button_Click(object sender, EventArgs e)
        {
            if (CreateGame())
            {
                game.Text = "PvC";
                game.InitializeStartingPlayer();
            }
        }

        private void Rules_Button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Description:" +
                            "\n- Mills is a strategy board game for two players.\n" +
                            "- The board consists of a grid with some number of intersections, or points.\n" +
                            "- Each player has some number of pieces, or men, based on the game variation.\n" +
                            "- Men are usually coloured black and white, but this version is coloured brown and orange.\n" +
                            "\n" +
                            "How to win" +
                            "\n- Players try to form 'mills' — three of their own men lined horizontally or " +
                            "vertically (sometimes diagonally too) — allowing a player to remove an opponent's man from the game.\n" +
                            "- When a mill is formed, the player who formed a mill can only remove other team's men " +
                            "that aren't currently forming mills, unless there aren't any other options left.\n" +
                            "- A player wins by reducing the opponent to two men " +
                            "(whereupon they can no longer form mills and thus are unable to win) or by blocking opponent's " +
                            "movements (whereupon they can no longer make a move).\n" +
                            "\n" +
                            "Game phases" +
                            "\nPhase 1 - Placing phase - players place their men from their bench on the board. " +
                            "Once all of the bench men have been placed, phase 2 starts." +
                            "\n" +
                            "\nPhase 2 - Moving phase - players move their men around the board to adjacent points," +
                            "forming mills, removing opponent's men..." +
                            "\n" +
                            "\nPhase 3 - Flying phase - men can be moved around to any free space if they have 3 men left " +
                            "on board.\n" +
                            "\n" +
                            "Game variations:" +
                            "\n1) Three Men's Morris (TMM) - consists of a grid with 9 intersections, or points. " +
                            "Each player has 3 men. Mills can be also formed diagonally." +
                            "\n" +
                            "\n2) Six Men's Morris (SMM) - consists of grid with 16 intersections, or points. " +
                            "Each player has 6 men." +
                            "\n" +
                            "\n3) Nine Men's Morris (NMM) - consists of grid with 24 intersections, or points. " +
                            "Each player has 9 men.", "Rules");
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

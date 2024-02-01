using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersLogic;
using static CheckersLogic.Move;

namespace CheckersUI
{
    public class GameForm: Form
    {
        private const int k_ButtonSize = 40;
        private const char k_Player1ManSymbol = 'X';
        private const char k_Player2ManSymbol = 'O';
        private const char k_Player1KingSymbol = 'Z';
        private const char k_Player2KingSymbol = 'Q';
        private readonly Game r_Game;
        private readonly int r_BoardSize;
        private Button[,] m_Board;
        private Label m_Player1Label = new Label();
        private Label m_Player2Label = new Label();
        private bool m_FromTileIsChosen = false;
        private Button m_StartOfMoveButton;

        public GameForm(InitializeGameForm i_InitializeGameForm)
        {
            Text = "Checkers";
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            Player player1 = new Player(Player.ePlayerType.Human, i_InitializeGameForm.Player1Name, Tile.eTileType.Player1Man);
            Player player2 = new Player(i_InitializeGameForm.Player2CheckBoxIsChecked ? Player.ePlayerType.Human : Player.ePlayerType.Computer, i_InitializeGameForm.Player2CheckBoxIsChecked ? i_InitializeGameForm.Player2Name : "Computer", Tile.eTileType.Player2Man);

            if (i_InitializeGameForm.BoardSize6X6IsChecked)
            {
                r_BoardSize = 6;
            }
            else if (i_InitializeGameForm.BoardSize8X8IsChecked)
            {
                r_BoardSize = 8;
            }
            else
            {
                r_BoardSize = 10;
            }

            r_Game = new Game(r_BoardSize, player1, player2);
            r_Game.m_GameOver += game_GameOver;
            r_Game.m_BoardChanged += game_BoardChange;
            m_Board = new Button[r_BoardSize, r_BoardSize];
            Size = new Size((r_BoardSize * k_ButtonSize) + 100, (r_BoardSize * k_ButtonSize) + 100);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            initializeControls();
            displayBoard();
        }

        private void initializeControls()
        {
            m_Player1Label.Size = new Size(100, 20);
            m_Player1Label.Text = r_Game.Player1.Name + ": " + r_Game.Player1.Score;
            m_Player1Label.Location = new Point(50 + k_ButtonSize, 20);
            m_Player2Label.Size = new Size(100, 20);
            m_Player2Label.Text = r_Game.Player2.Name + ": " + r_Game.Player2.Score;
            m_Player2Label.Location = new Point(50 + ((r_BoardSize - 2) * k_ButtonSize), m_Player1Label.Height);
            this.Controls.AddRange(
               new Control[]
               {
                 m_Player1Label,
                 m_Player2Label
               });

            updatePlayersLabel();
            int startX = 50;
            int startY = m_Player1Label.Height + 30;
            int currentX;
            int currentY;

            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    m_Board[row, col] = new Button();
                    m_Board[row, col].Size = new Size(k_ButtonSize, k_ButtonSize);
                    currentX = startX + (row * k_ButtonSize);
                    currentY = startY + (col * k_ButtonSize);
                    m_Board[row, col].Location = new Point(currentX, currentY);
                    if (((row + col) % 2) == 0)
                    {
                        m_Board[row, col].Enabled = false;
                        m_Board[row, col].BackColor = Color.Gray;
                    }

                    Controls.Add(m_Board[row, col]);
                    m_Board[row, col].Click += new EventHandler(button_Click);
                }
            }
        }

        private void updatePlayersLabel()
        {
            r_Game.Player1.UpdatePlayerScore(r_Game.Board);
            r_Game.Player2.UpdatePlayerScore(r_Game.Board);
            m_Player1Label.Text = r_Game.Player1.Name + ": " + r_Game.Player1.Score;
            m_Player2Label.Text = r_Game.Player2.Name + ": " + r_Game.Player2.Score;
        }

        private void displayBoard()
        {
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    m_Board[col, row].Text = Tile.GetDescription(r_Game.Board.GetBoard[row, col]);
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (!m_FromTileIsChosen)
            {
                m_StartOfMoveButton = sender as Button;
                handleStartOfMove(m_StartOfMoveButton);
            }
            else
            {
                Button endOfMove = sender as Button;
                handleEndOfMove(m_StartOfMoveButton, endOfMove);
                if (!r_Game.Player1Turn && r_Game.Player2.PlayerType == Player.ePlayerType.Computer)
                {
                    r_Game.DoComputerMove(r_Game.Board);
                }
            }
        }

        private void handleStartOfMove(Button i_ChosenButton)
        {
            bool chosenButtonIsValid = false;
            int rowOfChosenButton = (i_ChosenButton.Location.Y - m_Board[0, 0].Location.Y) / k_ButtonSize;
            int colOfChosenButton = (i_ChosenButton.Left - m_Board[0, 0].Location.X) / k_ButtonSize;
            if (r_Game.Player1Turn)
            {
                if (r_Game.Board.GetBoard[rowOfChosenButton, colOfChosenButton] == Tile.eTileType.Player1Man || r_Game.Board.GetBoard[rowOfChosenButton, colOfChosenButton] == Tile.eTileType.Player1King)
                {
                    chosenButtonIsValid = true;
                }
            }
            else
            {
                if (r_Game.Board.GetBoard[rowOfChosenButton, colOfChosenButton] == Tile.eTileType.Player2Man || r_Game.Board.GetBoard[rowOfChosenButton, colOfChosenButton] == Tile.eTileType.Player2King)
                {
                    chosenButtonIsValid = true;
                }
            }

            if (chosenButtonIsValid)
            {
                i_ChosenButton.BackColor = Color.PowderBlue;
                m_StartOfMoveButton = i_ChosenButton;
                m_FromTileIsChosen = true;
            }
        }

        private void handleEndOfMove(Button i_StartOfMoveButton, Button i_ChosenButton)
        {
            int rowOStartButton = (i_StartOfMoveButton.Location.Y - m_Board[0, 0].Location.Y) / k_ButtonSize;
            int colOfStartButton = (i_StartOfMoveButton.Left - m_Board[0, 0].Location.X) / k_ButtonSize;
            int rowOfChosenButton = (i_ChosenButton.Location.Y - m_Board[0, 0].Location.Y) / k_ButtonSize;
            int colOfChosenButton = (i_ChosenButton.Left - m_Board[0, 0].Location.X) / k_ButtonSize;
            if (i_StartOfMoveButton.Equals(i_ChosenButton))
            {
                i_StartOfMoveButton.BackColor = Color.White;
                m_FromTileIsChosen = false;
            }
            else
            {
                Move move = new Move(rowOStartButton, colOfStartButton, rowOfChosenButton, colOfChosenButton);
                eMoveStatus moveStatus = r_Game.CheckMoveStatus(move);
                switch (moveStatus)
                {
                    case eMoveStatus.InvalidMove:
                        showTryAgainMessage("Invalid move");
                        break;

                    case eMoveStatus.MustJump:
                        showTryAgainMessage("Must jump");
                        break;

                    case eMoveStatus.MoveSuccessfull:
                        r_Game.DoMove(r_Game.Board, move);
                        break;

                    default:
                        break;
                }

                i_StartOfMoveButton.BackColor = Color.White;
                m_FromTileIsChosen = false;
            }
        }

        private void game_BoardChange(object sender, Move i_move)
        {
            m_Board[i_move.FromTileCol, i_move.FromTileRow].Text = string.Empty;
            m_Board[i_move.ToTileCol, i_move.ToTileRow].Text = Tile.GetDescription(r_Game.Board.GetBoard[i_move.ToTileRow, i_move.ToTileCol]);
            if (i_move.IsJump())
            {
                m_Board[(i_move.FromTileCol + i_move.ToTileCol) / 2, (i_move.FromTileRow + i_move.ToTileRow) / 2].Text = string.Empty;
            }

            updatePlayersLabel();
        }

        private void game_GameOver(object sender, EventArgs e)
        {
            bool playAgain = false;
            switch (r_Game.GameOverStatus)
            {
                case Game.eGameOverStatus.Draw:
                    playAgain = anotherRoundMessage("Draw!");
                    break;

                case Game.eGameOverStatus.Player1Won:
                    playAgain = anotherRoundMessage(r_Game.Player1.Name + " Won!");
                    break;

                case Game.eGameOverStatus.Player2Won:
                    playAgain = anotherRoundMessage(r_Game.Player2.Name + " Won!");
                    break;

                default:
                    break;
            }

            if (playAgain)
            {
                m_FromTileIsChosen = false;
                r_Game.Player1Turn = true;
                updatePlayersLabel();
                displayBoard();
            }
            else
            {
                Close();
            }
        }

        private void showTryAgainMessage(string i_message)
        {
            MessageBox.Show(i_message, "Try again", MessageBoxButtons.OK);
        }

        private bool anotherRoundMessage(string i_message)
        {
            String message = String.Format(@"{0}
Play another round?", i_message);
            return MessageBox.Show(message, "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /*private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GameForm
            // 
            this.ClientSize = new System.Drawing.Size(685, 381);
            this.Name = "GameForm";
            this.ResumeLayout(false);
        

        }*/
    }
}

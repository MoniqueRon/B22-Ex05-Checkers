using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CheckersLogic.Tile;
using static CheckersLogic.Board;
using static CheckersLogic.Move;


namespace CheckersLogic
{
    public class Game
    {
        public enum eGameOverStatus
        {
            Draw,
            Player1Won,
            Player2Won,
        }

        public delegate void BoardChangeEventHandler(object sender, Move i_Move);
        public delegate void GameOverEventHandler(object sender, EventArgs e);

        public event BoardChangeEventHandler m_BoardChanged;
        public event GameOverEventHandler m_GameOver;

        private readonly Board r_Board;
        private readonly int r_BoardSize;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private bool m_Player1Turn;
        private eGameOverStatus m_GameOverStatus;

        public Game(int i_BoardSize, Player i_Player1, Player i_Player2)
        {
            r_BoardSize = i_BoardSize;
            r_Board = new Board(i_BoardSize);
            r_Player1 = i_Player1;
            r_Player2 = i_Player2;
            m_Player1Turn = true;
            r_Board.InitializeBoard();
        }

        public void DoMove(Board i_Board, Move i_Move)
        {
            Player actingPlayer = GetActingPlayer();
            eTileType[,] board = i_Board.GetBoard;

            board[i_Move.ToTileRow, i_Move.ToTileCol] = board[i_Move.FromTileRow, i_Move.FromTileCol];
            board[i_Move.FromTileRow, i_Move.FromTileCol] = eTileType.Empty;
            if ((board[i_Move.ToTileRow, i_Move.ToTileCol] == eTileType.Player1Man) && (i_Move.ToTileRow == 0))
            {
                board[i_Move.ToTileRow, i_Move.ToTileCol] = eTileType.Player1King;
            }

            if ((board[i_Move.ToTileRow, i_Move.ToTileCol] == eTileType.Player2Man) && (i_Move.ToTileRow == i_Board.BoardSize - 1))
            {
                board[i_Move.ToTileRow, i_Move.ToTileCol] = eTileType.Player2King;
            }

            if (i_Move.IsJump())
            {
                int middleRow = (i_Move.FromTileRow + i_Move.ToTileRow) / 2;
                int middleCol = (i_Move.FromTileCol + i_Move.ToTileCol) / 2;
                board[middleRow, middleCol] = eTileType.Empty;
                if (!i_Move.HasMoreJumps(i_Board, actingPlayer, i_Move.ToTileRow, i_Move.ToTileCol))
                {
                    Player1Turn = !Player1Turn;
                }
            }
            else 
            {
                Player1Turn = !Player1Turn;
            }

            BoardChanged(i_Move);
            updateGameOverStatus();
        }

        public void DoComputerMove(Board i_Board)
        {
            Move computerMove = null;
            List<Move> moves;
            bool isFollowUpJump = false;
            Random random = new Random();
            if (!isFollowUpJump)
            {
                moves = GetAllValidMoves(i_Board, r_Player2);
                int randomNumber = random.Next(0, moves.Count);
                computerMove = moves[randomNumber];
                DoMove(i_Board, computerMove);
                if (computerMove.IsJump() && computerMove.HasMoreJumps(i_Board, r_Player2, computerMove.ToTileRow, computerMove.ToTileCol))
                {
                    isFollowUpJump = true;
                }
            }
            while (isFollowUpJump)
            {
                moves = GetValidJumpsFromTile(i_Board, r_Player2, computerMove.ToTileRow, computerMove.ToTileCol);
                int randomNumber = random.Next(0, moves.Count);
                computerMove = moves[randomNumber];
                DoMove(i_Board, computerMove);
                if (!computerMove.HasMoreJumps(i_Board, r_Player2, computerMove.ToTileRow, computerMove.ToTileCol))
                {
                    isFollowUpJump = false;
                }
            }
        }

        public eMoveStatus CheckMoveStatus(Move i_Move)
        {
            eMoveStatus moveStatus = eMoveStatus.MoveSuccessfull;
            Player actingPlayer = GetActingPlayer();
            List<Move> moves = GetAllValidMoves(r_Board, actingPlayer);

            if (moves[0].IsJump() && !i_Move.IsJump())
            {
                moveStatus = eMoveStatus.MustJump;
            }
            else if (i_Move.IsJump())
            {
                if (!CanJump(r_Board, i_Move.FromTileRow, i_Move.FromTileCol, i_Move.ToTileRow, i_Move.ToTileCol))
                {
                    moveStatus = eMoveStatus.InvalidMove;
                }
            }
            else
            {
                if (!CanMove(r_Board, i_Move.FromTileRow, i_Move.FromTileCol, i_Move.ToTileRow, i_Move.ToTileCol))
                {
                    moveStatus = eMoveStatus.InvalidMove;
                }
            }

            return moveStatus;
        }

        private bool gameIsOver()
        {
            bool gameOver = false;
            if (GetAllValidMoves(r_Board, Player1) == null || GetAllValidMoves(r_Board, Player2) == null)
            {
                gameOver = true;
            }

            return gameOver;
        }

        private void updateGameOverStatus()
        {
            EventArgs e = new EventArgs();
            if (gameIsOver())
            {
                Player1.UpdatePlayerScore(r_Board);
                Player2.UpdatePlayerScore(r_Board);
                if (Player1.Score > Player2.Score)
                {
                    m_GameOverStatus = eGameOverStatus.Player1Won;
                }
                else if(Player1.Score < Player2.Score)
                {
                    m_GameOverStatus = eGameOverStatus.Player2Won;
                }
                else
                {
                    m_GameOverStatus = eGameOverStatus.Draw;
                }

                r_Board.InitializeBoard();
                OnGameOver(e);
            }
        }

        private void BoardChanged(Move i_Move)
        {
            OnBoardChange(i_Move);
        }

        protected virtual void OnGameOver(EventArgs e)
        {
            m_GameOver?.Invoke(this, e);
        }

        protected virtual void OnBoardChange(Move i_Move)
        {
            m_BoardChanged?.Invoke(this, i_Move);
        }

        public Board Board
        {
            get { return r_Board; }
        }

        public Player Player1
        {
            get { return r_Player1; }
        }

        public Player Player2
        {
            get { return r_Player2; }
        }

        public eGameOverStatus GameOverStatus
        {
            get { return m_GameOverStatus; }
            set { m_GameOverStatus = value; }
        }

        public bool Player1Turn
        {
            get { return m_Player1Turn; }
            set { m_Player1Turn = value; }
        }

        public Player GetActingPlayer()
        {
            return m_Player1Turn ? r_Player1 : r_Player2;
        }
    }
}

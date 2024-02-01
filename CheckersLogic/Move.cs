using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CheckersLogic.Tile;
using static CheckersLogic.Game;

namespace CheckersLogic
{
    public class Move
    {
        public enum eMoveStatus
        {
            MoveSuccessfull,
            InvalidMove,
            MustJump,
        }

        private int m_FromTileRow;
        private int m_FromTileCol;
        private int m_ToTileRow;
        private int m_ToTileCol;
        private eMoveStatus m_MoveStatus;

        public Move(int i_FromTileRow, int i_FromTileCol, int i_ToTileRow, int i_ToTileCol)
        {
            m_FromTileRow = i_FromTileRow;
            m_FromTileCol = i_FromTileCol;
            m_ToTileRow = i_ToTileRow;
            m_ToTileCol = i_ToTileCol;
        }

        public eMoveStatus MoveStatus
        {
            get { return m_MoveStatus; }
            set { m_MoveStatus = value; }
        }

        public int FromTileRow
        {
            get { return m_FromTileRow; }
            set { m_FromTileRow = value; }
        }

        public int FromTileCol
        {
            get { return m_FromTileCol; }
            set { m_FromTileCol = value; }
        }

        public int ToTileRow
        {
            get { return m_ToTileRow; }
            set { m_ToTileRow = value; }
        }

        public int ToTileCol
        {
            get { return m_ToTileCol; }
            set { m_ToTileCol = value; }
        }

        public static List<Move> GetAllValidMoves(Board i_Board, Player i_Player)
        {
            List<Move> moves = new List<Move>();
            moves = GetValidJumpMoves(i_Board, i_Player);

            if (moves.Count == 0)
            {
                moves = GetValidNonJumpMoves(i_Board, i_Player);
            }

            if (moves.Count == 0)
            {
                moves = null;
            }

            return moves;
        }

        public static List<Move> GetValidJumpMoves(Board i_Board, Player i_Player)
        {
            List<Move> jumpMoves = new List<Move>();
            eTileType[,] board = i_Board.GetBoard;

            for (int row = 0; row < i_Board.BoardSize; row++)
            {
                for (int col = 0; col < i_Board.BoardSize; col++)
                {
                    if (board[row, col] == i_Player.ManSymbol || board[row, col] == i_Player.KingSymbol)
                    {
                        if (CanJump(i_Board, row, col, row + 2, col + 2))
                        {
                            jumpMoves.Add(new Move(row, col, row + 2, col + 2));
                        }

                        if (CanJump(i_Board, row, col, row + 2, col - 2))
                        {
                            jumpMoves.Add(new Move(row, col, row + 2, col - 2));
                        }

                        if (CanJump(i_Board, row, col, row - 2, col + 2))
                        {
                            jumpMoves.Add(new Move(row, col, row - 2, col + 2));
                        }

                        if (CanJump(i_Board, row, col, row - 2, col - 2))
                        {
                            jumpMoves.Add(new Move(row, col, row - 2, col - 2));
                        }
                    }
                }
            }

            return jumpMoves;
        }

        public static List<Move> GetValidNonJumpMoves(Board i_Board, Player i_Player)
        {
            List<Move> nonJumpMoves = new List<Move>();
            eTileType[,] board = i_Board.GetBoard;

            for (int row = 0; row < i_Board.BoardSize; row++)
            {
                for (int col = 0; col < i_Board.BoardSize; col++)
                {
                    if (board[row, col] == i_Player.ManSymbol || board[row, col] == i_Player.KingSymbol)
                    {
                        if (CanMove(i_Board, row, col, row + 1, col + 1))
                        {
                            nonJumpMoves.Add(new Move(row, col, row + 1, col + 1));
                        }

                        if (CanMove(i_Board, row, col, row + 1, col - 1))
                        {
                            nonJumpMoves.Add(new Move(row, col, row + 1, col - 1));
                        }

                        if (CanMove(i_Board, row, col, row - 1, col + 1))
                        {
                            nonJumpMoves.Add(new Move(row, col, row - 1, col + 1));
                        }

                        if (CanMove(i_Board, row, col, row - 1, col - 1))
                        {
                            nonJumpMoves.Add(new Move(row, col, row - 1, col - 1));
                        }
                    }
                }
            }

            return nonJumpMoves;
        }

        public static bool CanJump(Board i_Board, int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            bool canJump = true;
            eTileType[,] board = i_Board.GetBoard;
            int middleRow = (i_FromRow + i_ToRow) / 2;
            int middleCol = (i_FromCol + i_ToCol) / 2;

            if (i_ToRow < 0 || i_ToRow >= i_Board.BoardSize || i_ToCol < 0 || i_ToCol >= i_Board.BoardSize)
            {
                canJump = false;
            }
            else if (Math.Abs(i_FromRow - i_ToRow) != 2 || Math.Abs(i_FromCol - i_ToCol) != 2)
            {
                canJump = false;
            }
            else if (board[i_ToRow, i_ToCol] != eTileType.Empty)
            {
                canJump = false;
            }
            else if (board[middleRow, middleCol] == eTileType.Empty)
            {
                canJump = false;
            }
            else if ((board[i_FromRow, i_FromCol] == eTileType.Player1Man || board[i_FromRow, i_FromCol] == eTileType.Player1King) && (board[middleRow, middleCol] == eTileType.Player1Man || board[middleRow, middleCol] == eTileType.Player1King))
            {
                canJump = false;
            }
            else if ((board[i_FromRow, i_FromCol] == eTileType.Player2Man || board[i_FromRow, i_FromCol] == eTileType.Player2King) && (board[middleRow, middleCol] == eTileType.Player2Man || board[middleRow, middleCol] == eTileType.Player2King))
            {
                canJump = false;
            }
            else if (board[i_FromRow, i_FromCol] == eTileType.Player1Man && i_FromRow <= i_ToRow)
            {
                canJump = false;
            }
            else if (board[i_FromRow, i_FromCol] == eTileType.Player2Man && i_FromRow >= i_ToRow)
            {
                canJump = false;
            }

            return canJump;
        }


        public static bool CanMove(Board i_Board, int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            bool canMove = true;
            eTileType[,] board = i_Board.GetBoard;

            if (i_ToRow < 0 || i_ToRow >= i_Board.BoardSize || i_ToCol < 0 || i_ToCol >= i_Board.BoardSize)
            {
                canMove = false;
            }
            else if (board[i_ToRow, i_ToCol] != eTileType.Empty)
            {
                canMove = false;
            }
            else if (Math.Abs(i_FromRow - i_ToRow) != 1 || Math.Abs(i_FromCol - i_ToCol) != 1)
            {
                canMove = false;
            }
            else if (board[i_FromRow, i_FromCol] == eTileType.Player1Man && i_FromRow <= i_ToRow)
            {
                canMove = false;
            }
            else if (board[i_FromRow, i_FromCol] == eTileType.Player2Man && i_FromRow >= i_ToRow)
            {
                canMove = false;
            }

            return canMove;
        }

        public bool IsJump()
        {
            bool isJump = (Math.Abs(m_FromTileRow - m_ToTileRow) == 2 && Math.Abs(m_FromTileCol - m_ToTileCol) == 2);

            return isJump;
        }

        public static List<Move> GetValidJumpsFromTile(Board i_Board, Player i_Player, int i_TileRow, int i_TileCol)
        {
            List<Move> allJumps = GetValidJumpMoves(i_Board, i_Player);
            List<Move> jumpsFromTile = new List<Move>();
            foreach (Move move in allJumps)
            {
                if (move.FromTileRow == i_TileRow && move.FromTileCol == i_TileCol)
                {
                    jumpsFromTile.Add(move);
                }
            }

            return jumpsFromTile;
        }

        public bool HasMoreJumps(Board i_Board, Player i_Player, int i_TileRow, int i_TileCol)
        {
            bool hasMoreJumps = false;
            List<Move> jumps = GetValidJumpsFromTile(i_Board, i_Player, i_TileRow, i_TileCol);
            if (jumps.Count > 0)
            {
                hasMoreJumps = true;
            }

            return hasMoreJumps;
        }
    }
}

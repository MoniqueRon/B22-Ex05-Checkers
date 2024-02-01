using static CheckersLogic.Tile;

namespace CheckersLogic
{
    public class Board
    {
        private readonly eTileType[,] r_Board;
        private readonly int r_BoardSize;

        public Board(int i_BoardSize)
        {
            r_BoardSize = i_BoardSize;
            r_Board = new eTileType[r_BoardSize, r_BoardSize];
        }

        public eTileType[,] GetBoard
        {
            get { return r_Board; }
        }

        public int BoardSize
        {
            get { return r_BoardSize; }
        }

        public void InitializeBoard()
        {
            int numberOfRowsToFillPerPlayer = (r_BoardSize / 2) - 1;

            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        if (row < numberOfRowsToFillPerPlayer)
                        {
                            r_Board[row, col] = eTileType.Player2Man;
                        }
                        else if (row > r_BoardSize - numberOfRowsToFillPerPlayer - 1)
                        {
                            r_Board[row, col] = eTileType.Player1Man;
                        }
                        else
                        {
                            r_Board[row, col] = eTileType.Empty;
                        }
                    }
                    else
                    {
                        r_Board[row, col] = eTileType.Empty;
                    }
                }
            }
        }
    }
}

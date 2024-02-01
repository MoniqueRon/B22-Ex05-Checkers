using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CheckersLogic.Tile;

namespace CheckersLogic
{
    public class Player
    {
        public enum ePlayerType
        {
            Computer,
            Human,
        }

        private ePlayerType m_PlayerType;
        private string m_Name;
        private int m_Score;
        private eTileType m_ManSymbol;
        private eTileType m_KingSymbol;

        public Player(ePlayerType i_PlayerType, string i_Name, eTileType i_Symbol)
        {
            m_Name = i_Name;
            m_PlayerType = i_PlayerType;
            m_Score = 0;
            m_ManSymbol = i_Symbol;
            m_KingSymbol = i_Symbol + 2;
        }

        public string Name
        {
            get { return m_Name; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }
        }

        public eTileType ManSymbol
        {
            get { return m_ManSymbol; }
        }

        public eTileType KingSymbol
        {
            get { return m_KingSymbol; }
        }

        public void UpdatePlayerScore(Board i_Board)
        {
            int playerScore = 0;

            for (int row = 0; row < i_Board.BoardSize; row++)
            {
                for (int col = 0; col < i_Board.BoardSize; col++)
                {
                    if (i_Board.GetBoard[row, col] == ManSymbol)
                    {
                        playerScore++;
                    }

                    else if (i_Board.GetBoard[row, col] == KingSymbol)
                    {
                        playerScore += 4;
                    }
                }
            }

            Score = playerScore;
        }
    }
}

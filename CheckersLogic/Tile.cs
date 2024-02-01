using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    public class Tile
    {
        public enum eTileType
        {
            [Description(" ")]
            Empty,
            [Description("X")]
            Player1Man,
            [Description("O")]
            Player2Man,
            [Description("Z")]
            Player1King,
            [Description("Q")]
            Player2King,
        }

        public eTileType m_TileType;
        public int m_Column;
        public int m_Row;

        public Tile(int i_Row, int i_Column, eTileType i_TileType)
        {
            m_Column = i_Column;
            m_Row = i_Row;
            m_TileType = i_TileType;
        }

        public eTileType TileType
        {
            get { return m_TileType; }
            set { m_TileType = value; }
        }

        public int Column
        {
            get { return m_Column; }
        }

        public int Row
        {
            get { return m_Row; }
        }

        public static string GetDescription(Enum i_Value)
        {
            FieldInfo fi = i_Value.GetType().GetField(i_Value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0 && attributes != null)
                return attributes[0].Description;
            else
                return i_Value.ToString();
        }
    }
}

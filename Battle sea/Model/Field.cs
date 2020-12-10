using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Model;

namespace Battle_sea.Model
{
    class Field : ICloneable
    {
        public Cell[,] field = new Cell[10,10];

        public int fourCellShips { get; set; } 
        public int threeCellShips { get; set; } 
        public int twoCellShips { get; set; } 
        public int oneCellShips { get; set; }
        public ShipType selectedShipType { get; set; }

        public Field () { }

        public Field (String[] stringRows)
        {
            for (int y = 0; y < field.GetLength(1); y++)
            {
                String row = stringRows[y];
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    field[y, x] = (Cell) int.Parse(row[x].ToString());
                }
            }
        }

        public object Clone()
        {
            Field result = new Field();
            result.fourCellShips = fourCellShips;
            result.threeCellShips = threeCellShips;
            result.twoCellShips = twoCellShips;
            result.oneCellShips = oneCellShips;
            Cell[,] resultField = new Cell[10, 10];
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    resultField[x, y] = field[x, y];
                }
            }
            result.field = resultField;
            return result;
        }

        public override String ToString()
        {
            String result = "";
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    result += (int) field[y, x];
                }
                result += "\n";
            }
            return result.Trim();
            
        }
        
    }
}

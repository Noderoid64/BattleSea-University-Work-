using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Model;

namespace Battle_sea.Services.BL
{
    class ShipTypeBL
    {

        #region
        private static ShipTypeBL instance;

        public static ShipTypeBL getInstance()
        {
            if (instance == null)
            {
                instance = new ShipTypeBL();
            }
            return instance;
        }

        private ShipTypeBL() { }
        #endregion

        public ShipType getNextShipType(ShipType currentType, int fourCellShipCount, int threeCellShipCount, int twoCellShipCount, int oneCellShipCount)
        {
            ShipType result = currentType;
            switch(currentType)
            {
                case ShipType.FourCells:
                    {
                        if (threeCellShipCount > 0)
                            result = ShipType.ThreeCells;
                        else if (twoCellShipCount > 0)
                            result = ShipType.TwoCells;
                        else if (oneCellShipCount > 0)
                            result = ShipType.OneCell;
                            
                    } break;
                case ShipType.ThreeCells:
                    {
                        if (twoCellShipCount > 0)
                            result = ShipType.TwoCells;
                        else if (oneCellShipCount > 0)
                            result = ShipType.OneCell;
                        else if (fourCellShipCount > 0)
                            result = ShipType.FourCells;
                    }
                    break;
                case ShipType.TwoCells:
                    {
                        if (oneCellShipCount > 0)
                            result = ShipType.OneCell;
                        else if (fourCellShipCount > 0)
                            result = ShipType.FourCells;
                        else if (threeCellShipCount > 0)
                            result = ShipType.ThreeCells;
                    }
                    break;
                case ShipType.OneCell:
                    {
                        if (fourCellShipCount > 0)
                            result = ShipType.FourCells;
                        else if (threeCellShipCount > 0)
                            result = ShipType.ThreeCells;
                        else if (twoCellShipCount > 0)
                            result = ShipType.TwoCells;
                    }
                    break;
            }
            return result;
        }
    }
}

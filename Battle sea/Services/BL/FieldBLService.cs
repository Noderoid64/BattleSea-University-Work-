using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Model;

namespace Battle_sea.Services.BL
{
    class FieldBLService
    {
        private static FieldBLService instance;
        private FieldBLService() { }

        public static FieldBLService getInstance()
        { 
            if (instance == null)
            {
                instance = new FieldBLService();
            }
            return instance;
        }

        public bool isAllShipPlaces(Field field)
        {
            return field.fourCellShips == 0 &&
                field.threeCellShips == 0 &&
                field.twoCellShips == 0 &&
                field.oneCellShips == 0;
        }
        public Field placeShip(Field field, Point cursor, ShipType shipType, bool isHorizontal = false)
        {
            Field result = (Field) field.Clone();
            switch (shipType)
            {
                case ShipType.OneCell:
                    {
                        placeOneCellShip(result, cursor.x, cursor.y);
                        result.oneCellShips--;
                    } break;
                case ShipType.TwoCells:
                    {
                        placeTwoCellShip(result, cursor.x, cursor.y, Cell.Ship, isHorizontal);
                        result.twoCellShips--;
                    } break;
                case ShipType.ThreeCells:
                    {
                        placeThreeCellShip(result, cursor.x, cursor.y, Cell.Ship, isHorizontal);
                        result.threeCellShips--;
                    }
                    break;
                case ShipType.FourCells:
                    {
                        placeFourCellShip(result, cursor.x, cursor.y, Cell.Ship, isHorizontal);
                        result.fourCellShips--;
                    }
                    break;

            }
            return result;
        }
        public Field placeShipGhost (Field field, Point cursor, ShipType shipType, bool isHorizontal = false)
        {
            Field result = (Field)field.Clone();
            if (canPlaceShip(field, cursor, shipType, isHorizontal))
            {
                switch (shipType)
                {
                    case ShipType.OneCell: placeOneCellShip(result, cursor.x, cursor.y, Cell.GhostGood); break;
                    case ShipType.TwoCells: placeTwoCellShip(result, cursor.x, cursor.y, Cell.GhostGood, isHorizontal); break;
                    case ShipType.ThreeCells: placeThreeCellShip(result, cursor.x, cursor.y, Cell.GhostGood, isHorizontal); break;
                    case ShipType.FourCells: placeFourCellShip(result, cursor.x, cursor.y, Cell.GhostGood, isHorizontal); break;

                }
            } else
            {
                switch (shipType)
                {
                    case ShipType.OneCell: placeOneCellShip(result, cursor.x, cursor.y, Cell.GhostBad); break;
                    case ShipType.TwoCells: placeTwoCellShip(result, cursor.x, cursor.y, Cell.GhostBad, isHorizontal); break;
                    case ShipType.ThreeCells: placeThreeCellShip(result, cursor.x, cursor.y, Cell.GhostBad, isHorizontal); break;
                    case ShipType.FourCells: placeFourCellShip(result, cursor.x, cursor.y, Cell.GhostBad, isHorizontal); break;

                }
            }

            
            return result;
        }
        public bool canPlaceShip(Field field, Point cursor, ShipType shipType, bool isHorizontal = false)
        {
            switch(shipType)
            {
                case ShipType.OneCell: return canPlaceOneCellShip(field, cursor.x, cursor.y);
                case ShipType.TwoCells: return canPlaceTwoCellShip(field, cursor.x, cursor.y, isHorizontal);
                case ShipType.ThreeCells: return canPlaceThreeCellShip(field, cursor.x, cursor.y, isHorizontal);
                case ShipType.FourCells: return canPlaceFourCellShip(field, cursor.x, cursor.y, isHorizontal);

            }
            return false;
        }
        public Field placeTarget(Field myEnemyField, Point cursor)
        {
            Field result = (Field) myEnemyField.Clone();
            if (canPlaceTarget(myEnemyField, cursor))
            {
                result.field[cursor.y, cursor.x] = Cell.TargetGood;
            } else
            {
                result.field[cursor.y, cursor.x] = Cell.TargetBad;
            }
            return result;
        }
        public bool canPlaceTarget(Field field, Point cursor)
        {
            return field.field[cursor.y, cursor.x] == Cell.Unknow;
        }
        public void makeShot(Field myEnemyField, Field enemyField, Point cursor, out bool isSuccess)
        {
            isSuccess = false;
            if (canPlaceTarget(myEnemyField, cursor))
            {
                switch(enemyField.field[cursor.y, cursor.x])
                {
                    case Cell.Empty:
                        {
                            enemyField.field[cursor.y, cursor.x] = Cell.Missed;
                            myEnemyField.field[cursor.y, cursor.x] = Cell.Missed;
                        }
                        break;
                    case Cell.Ship:
                        {
                            enemyField.field[cursor.y, cursor.x] = Cell.FireShip;
                            myEnemyField.field[cursor.y, cursor.x] = Cell.FireShip;
                            isSuccess = true;
                            if (this.isDestroy(enemyField, cursor, new Point(-1, -1)))
                            {
                                propagateDestroy(enemyField, myEnemyField, cursor, new Point(-1, -1));
                            }
                        }
                        break;
                }
            }
            
        }

        bool isDestroy (Field enemyField, Point cursor, Point cursorFrom)
        {
            bool result = true;
            if (cursor.x <= 8 && !((cursorFrom.x == cursor.x + 1) && (cursorFrom.y == cursor.y)))
            {
                if (enemyField.field[cursor.y, cursor.x + 1] == Cell.FireShip)
                {
                    result &= isDestroy(enemyField, new Point(cursor.x + 1, cursor.y), cursor);
                }
                else if (enemyField.field[cursor.y, cursor.x + 1] == Cell.Ship)
                {
                    return false;
                }
            }
            if (cursor.x >= 1 && !((cursorFrom.x == cursor.x - 1) && (cursorFrom.y == cursor.y)))
            {
                if (enemyField.field[cursor.y, cursor.x - 1] == Cell.FireShip)
                {
                    result &= isDestroy(enemyField, new Point(cursor.x - 1, cursor.y), cursor);
                }
                else if (enemyField.field[cursor.y, cursor.x - 1] == Cell.Ship)
                {
                    return false;
                }
            }
            if (cursor.y <= 8 && !((cursorFrom.y == cursor.y + 1) && (cursorFrom.x == cursor.x)))
            {
                if (enemyField.field[cursor.y + 1, cursor.x] == Cell.FireShip)
                {
                    result &= isDestroy(enemyField, new Point(cursor.x + 1, cursor.y), cursor);
                }
                else if (enemyField.field[cursor.y + 1, cursor.x] == Cell.Ship)
                {
                    return false;
                }
            }
            if (cursor.y >= 1 && !((cursorFrom.y == cursor.y - 1) && (cursorFrom.x == cursor.x)))
            {
                if (enemyField.field[cursor.y - 1, cursor.x] == Cell.FireShip)
                {
                    result &= isDestroy(enemyField, new Point(cursor.x , cursor.y - 1), cursor);
                }
                else if (enemyField.field[cursor.y - 1, cursor.x] == Cell.Ship)
                {
                    return false;
                }
            }
            return result;



        }

        void propagateDestroy (Field enemyField, Field myEnemyField, Point cursor, Point cursorFrom)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        if (cursor.x + x >= 0 && cursor.x + x <= 9 && cursor.y + y >= 0 && cursor.y + y <= 9)
                        {
                            if (enemyField.field[cursor.y + y, cursor.x + x] == Cell.FireShip &&
                                cursorFrom.x != cursor.x + x &&
                                cursorFrom.y != cursor.y + y)
                            {
                                propagateDestroy(enemyField, myEnemyField, new Point(cursor.x + x, cursor.y + y), cursor);
                            } else if (enemyField.field[cursor.y + y, cursor.x + x] != Cell.FireShip)
                            {
                                enemyField.field[cursor.y + y, cursor.x + x] = Cell.Missed;
                                myEnemyField.field[cursor.y + y, cursor.x + x] = Cell.Missed;
                            }
                        }
                    }
                }
            }
        }

        #region placeFunctions

        private void placeOneCellShip(Field field, int x, int y, Cell type = Cell.Ship)
        {
            field.field[y, x] = type;
        }

        private void placeTwoCellShip(Field field, int x, int y, Cell type = Cell.Ship, bool isHorizontal = false)
        {
            field.field[y, x] = type;
            if (isHorizontal)
            {
                if (checkShipCell(y, x + 1, field.field))
                field.field[y, x +1 ] = type;
            } else
            {
                if (checkShipCell(y + 1, x, field.field))
                    field.field[y + 1, x] = type;
            }
        }

        private void placeThreeCellShip(Field field, int x, int y, Cell type = Cell.Ship, bool isHorizontal = false)
        {
            field.field[y, x] = type;
            if (isHorizontal)
            {
                if (checkShipCell(y, x + 1, field.field))
                    field.field[y, x + 1] = type;
                if (checkShipCell(y, x + 2, field.field))
                    field.field[y, x + 2] = type;
            }
            else
            {
                if (checkShipCell(y + 1, x, field.field))
                    field.field[y + 1, x] = type;
                if (checkShipCell(y + 2, x, field.field))
                    field.field[y + 2, x] = type;
            }
        }

        private void placeFourCellShip(Field field, int x, int y, Cell type = Cell.Ship, bool isHorizontal = false)
        {
            field.field[y, x] = type;
            if (isHorizontal)
            {
                if (checkShipCell(y, x + 1, field.field))
                    field.field[y, x + 1] = type;
                if (checkShipCell(y, x + 2, field.field))
                    field.field[y, x + 2] = type;
                if (checkShipCell(y, x + 3, field.field))
                    field.field[y, x + 3] = type;
            }
            else
            {
                if (checkShipCell(y + 1, x, field.field))
                    field.field[y + 1, x] = type;
                if (checkShipCell(y + 2, x, field.field))
                    field.field[y + 2, x] = type;
                if (checkShipCell(y + 3, x, field.field))
                    field.field[y + 3, x] = type;
            }
        }

        #endregion

        #region canPlaceFunctions
        private bool canPlaceOneCellShip(Field field, int x, int y)
        {
            Cell[,] cells = field.field;
            bool result = cells[y, x] == Cell.Empty;

            result &= checkCell(y + 1, x - 1, cells);
            result &= checkCell(y + 1, x, cells);
            result &= checkCell(y + 1, x + 1, cells);

            result &= checkCell(y, x - 1, cells);
            result &= checkCell(y, x + 1, cells);

            result &= checkCell(y - 1, x - 1, cells);
            result &= checkCell(y - 1, x, cells);
            result &= checkCell(y - 1, x + 1, cells);

            result &= field.oneCellShips > 0;

            return result;
        }

        private bool canPlaceTwoCellShip(Field field, int x, int y, bool isHorizontal)
        {
            Cell[,] cells = field.field;
            bool result = checkShipCell(y, x, cells);

            if (isHorizontal)
            {
                result &= checkShipCell(y, x + 1, cells);

                result &= checkCell(y - 1, x - 1, cells);
                result &= checkCell(y - 1, x, cells);
                result &= checkCell(y - 1, x + 1, cells);
                result &= checkCell(y - 1, x + 2, cells);

                result &= checkCell(y, x - 1, cells);
                result &= checkCell(y, x + 1, cells);

                result &= checkCell(y + 1, x - 1, cells);
                result &= checkCell(y + 1, x, cells);
                result &= checkCell(y + 1, x + 1, cells);
                result &= checkCell(y + 1, x + 2, cells);

            } else
            {
                result &= checkShipCell(y + 1, x, cells);

                result &= checkCell(y - 1, x - 1, cells);
                result &= checkCell(y - 1, x, cells);
                result &= checkCell(y - 1, x + 1, cells);

                result &= checkCell(y, x - 1, cells);
                result &= checkCell(y, x + 1, cells);

                result &= checkCell(y + 1, x - 1, cells);
                result &= checkCell(y + 1, x + 1, cells);

                result &= checkCell(y + 2, x - 1, cells);
                result &= checkCell(y + 2, x, cells);
                result &= checkCell(y + 2, x + 1, cells);
            }

            result &= field.twoCellShips > 0;

            return result;
        }

        private bool canPlaceThreeCellShip(Field field, int x, int y, bool isHorizontal)
        {
            Cell[,] cells = field.field;
            bool result = checkShipCell(y, x, cells);

            if (isHorizontal)
            {
                result &= checkShipCell(y, x + 1, cells);

                result &= checkCell(y - 1, x - 1, cells);
                result &= checkCell(y - 1, x, cells);
                result &= checkCell(y - 1, x + 1, cells);
                result &= checkCell(y - 1, x + 2, cells);
                result &= checkCell(y - 1, x + 3, cells);

                result &= checkCell(y, x - 1, cells);
                result &= checkCell(y, x + 3, cells);

                result &= checkCell(y + 1, x - 1, cells);
                result &= checkCell(y + 1, x, cells);
                result &= checkCell(y + 1, x + 1, cells);
                result &= checkCell(y + 1, x + 2, cells);
                result &= checkCell(y + 1, x + 3, cells);

            }
            else
            {
                result &= checkShipCell(y + 1, x, cells);

                result &= checkCell(y - 1, x - 1, cells);
                result &= checkCell(y - 1, x, cells);
                result &= checkCell(y - 1, x + 1, cells);

                result &= checkCell(y, x - 1, cells);
                result &= checkCell(y, x + 1, cells);

                result &= checkCell(y + 1, x - 1, cells);
                result &= checkCell(y + 1, x + 1, cells);

                result &= checkCell(y + 2, x - 1, cells);
                result &= checkCell(y + 2, x + 1, cells);

                result &= checkCell(y + 3, x - 1, cells);
                result &= checkCell(y + 3, x, cells);
                result &= checkCell(y + 3, x + 1, cells);
            }

            result &= field.threeCellShips > 0;

            return result;
        }

        private bool canPlaceFourCellShip(Field field, int x, int y, bool isHorizontal)
        {
            Cell[,] cells = field.field;
            bool result = checkShipCell(y, x, cells);

            if (isHorizontal)
            {
                result &= checkShipCell(y, x + 1, cells);

                result &= checkCell(y - 1, x - 1, cells);
                result &= checkCell(y - 1, x, cells);
                result &= checkCell(y - 1, x + 1, cells);
                result &= checkCell(y - 1, x + 2, cells);
                result &= checkCell(y - 1, x + 3, cells);
                result &= checkCell(y - 1, x + 4, cells);

                result &= checkCell(y, x - 1, cells);
                result &= checkCell(y, x + 4, cells);

                result &= checkCell(y + 1, x - 1, cells);
                result &= checkCell(y + 1, x, cells);
                result &= checkCell(y + 1, x + 1, cells);
                result &= checkCell(y + 1, x + 2, cells);
                result &= checkCell(y + 1, x + 3, cells);
                result &= checkCell(y + 1, x + 4, cells);

            }
            else
            {
                result &= checkShipCell(y + 1, x, cells);

                result &= checkCell(y - 1, x - 1, cells);
                result &= checkCell(y - 1, x, cells);
                result &= checkCell(y - 1, x + 1, cells);

                result &= checkCell(y, x - 1, cells);
                result &= checkCell(y, x + 1, cells);

                result &= checkCell(y + 1, x - 1, cells);
                result &= checkCell(y + 1, x + 1, cells);

                result &= checkCell(y + 2, x - 1, cells);
                result &= checkCell(y + 2, x + 1, cells);

                result &= checkCell(y + 3, x - 1, cells);
                result &= checkCell(y + 3, x + 1, cells);

                result &= checkCell(y + 4, x - 1, cells);
                result &= checkCell(y + 4, x, cells);
                result &= checkCell(y + 4, x + 1, cells);
            }

            result &= field.fourCellShips > 0;

            return result;
        }
        #endregion
        private bool checkCell(int y, int x, Cell[,] cells)
        {
            return (x >= 0 && x < 10 && y >= 0 && y < 10) ? cells[y, x] == Cell.Empty : true;
        }
        private bool checkShipCell(int y, int x, Cell[,] cells)
        {
            return (x >= 0 && x < 10 && y >= 0 && y < 10) ? cells[y, x] == Cell.Empty : false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Model;

namespace Battle_sea.Drivers.FieldDrawer
{
    class FieldDrawer
    {
        private static FieldDrawer instance;
        public static FieldDrawer getInstance()
        {
            if (instance == null)
            {
                instance = new FieldDrawer();
            }
            return instance;
        }
        public void draw(Field field)
        {
            drawField(field);
            drawShips(field);

            Console.SetCursorPosition(0, 11);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void draw(Field myField, Field enemyField)
        {
            drawField(myField);
            drawField(enemyField, 13);
        }

        private void drawField(Field field, int cursorXPosition = 0)
        {
            Console.SetCursorPosition(cursorXPosition, 2);
            for (int x = 0; x < field.field.GetLength(0); x++)
            {
                for (int y = 0; y < field.field.GetLength(1); y++)
                {
                    switch (field.field[x, y])
                    {
                        case Cell.Empty:
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write('▓');
                            }
                            break;
                        case Cell.Ship:
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write('█');
                            }
                            break;
                        case Cell.GhostBad:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write('█');
                            }
                            break;
                        case Cell.GhostGood:
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write('█');
                            }
                            break;
                        case Cell.Missed:
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write('o');
                            }
                            break;
                        case Cell.FireShip:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write('x');
                            }
                            break;
                        case Cell.Unknow:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write('▓');
                            }
                            break;
                        case Cell.TargetGood:
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write('+');
                            }
                            break;
                        case Cell.TargetBad:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write('+');
                            }
                            break;
                    }
                }
                Console.SetCursorPosition(cursorXPosition, x + 3);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void drawShips(Field field, bool showSelectedShip = true)
        {
            Console.SetCursorPosition(11, 1);
            checkColor(field.fourCellShips, ShipType.FourCells, field.selectedShipType, showSelectedShip);
            Console.Write(field.fourCellShips);
            Console.Write(" ████");
            Console.ForegroundColor = ConsoleColor.Gray;

            checkColor(field.threeCellShips, ShipType.ThreeCells, field.selectedShipType, showSelectedShip);
            Console.SetCursorPosition(11, 3);
            Console.Write(field.threeCellShips);
            Console.Write(" ███");
            Console.ForegroundColor = ConsoleColor.Gray;

            checkColor(field.twoCellShips, ShipType.TwoCells, field.selectedShipType, showSelectedShip);
            Console.SetCursorPosition(11, 5);
            Console.Write(field.twoCellShips);
            Console.Write(" ██");
            Console.ForegroundColor = ConsoleColor.Gray;

            checkColor(field.oneCellShips, ShipType.OneCell, field.selectedShipType, showSelectedShip);
            Console.SetCursorPosition(11, 7);
            Console.Write(field.oneCellShips);
            Console.Write(" █");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public int[,] getTestField()
        {
            return new int[10, 10] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 1, 0, 0, 2, 1, 1, 1, 0},
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 1, 0, 1, 0, 2, 0, 0, 0, 0, 0},
                { 0, 0, 1, 0, 1, 3, 3, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 1, 2, 0, 2, 0, 1, 0},
                { 0, 1, 0, 1, 0, 1, 1, 0, 1, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0}
            };
        }

        private void checkColor(int shipCells, ShipType shipType, ShipType currentShipType, bool showSelectedShip)
        {
            if (showSelectedShip)
            if (shipCells <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (currentShipType == shipType)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
    }
}

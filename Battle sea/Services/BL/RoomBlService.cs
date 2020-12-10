using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Battle_sea.Model;

namespace Battle_sea.Services.BL
{
    class RoomBlService
    {
        private static RoomBlService instance;

        private RoomBlService() { }

        public static RoomBlService getInstance()
        {
            if (instance == null)
            {
                instance = new RoomBlService();
            }
            return instance;
        }
        public bool isRoomAlreadyExist(IList<string> rooms, string roomName)
        {
            string roomWithExistedName = rooms.FirstOrDefault(q => q.Equals(roomName));
            return roomWithExistedName == null;
        }
        public Room createNewRoom(String roomName, String hostPlayerName)
        {
            if (roomName != null && hostPlayerName != null)
            {
                Room result = new Room();
                result.name = roomName;
                result.firstPlayer = new Player();
                result.firstPlayer.name = hostPlayerName;
                result.firstPlayer.myField = getDefaultMap();
                result.firstPlayer.enemyField = getEnemyMap();
                result.secondPlayer = new Player();
                result.secondPlayer.myField = getSecondMap();
                result.secondPlayer.enemyField = getEnemyMap();

                return result;
            } else
            {
                throw new Exception();
            }
        }

        public bool isTheHostFirstTime()
        {
            return new Random().Next(0, 1) == 0 ? true : false;
        }

        public bool isMyTurn(Room room, bool iamFirst)
        {
            return (iamFirst && room.isFirstPlayerTime) || !(iamFirst || room.isFirstPlayerTime);
        }

        public Field getMyField(Room room, bool iamFirstPlayer)
        {
            return iamFirstPlayer ? room.firstPlayer.myField : room.secondPlayer.myField;
        }

        public Field getMyEnemyField(Room room, bool iamFirstPlayer)
        {
            return iamFirstPlayer ? room.firstPlayer.enemyField : room.secondPlayer.enemyField;
        }

        public Field getEnemyField(Room room, bool iamFirstPlayer)
        {
            return iamFirstPlayer ? room.secondPlayer.myField : room.firstPlayer.myField;
        }

        public void setNotMyTurn(Room room, bool iamFirstPlayer)
        {
            if (iamFirstPlayer)
            {
                room.isFirstPlayerTime = false;
            } else
            {
                room.isFirstPlayerTime = true;
            }
        }

        private Field getDefaultMap()
        {
            Field result = new Field();
            result.field = new Cell[10, 10] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            result.fourCellShips = 1;
            result.threeCellShips = 2;
            result.twoCellShips = 3;
            result.oneCellShips = 4;
            return result;
        }

        private Field getEnemyMap()
        {
            Field result = new Field();
            result.field = new Cell[10, 10] {
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow},
                { Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow, Cell.Unknow}
            };

            result.fourCellShips = 1;
            result.threeCellShips = 2;
            result.twoCellShips = 3;
            result.oneCellShips = 4;
            return result;
        }

        public Field getFirstMap()
        {
            Field result = new Field();
            result.field = new Cell[10, 10] {
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Ship, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Ship, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Ship, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Ship, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Ship, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Ship, Cell.Ship, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship}
            };

            result.fourCellShips = 0;
            result.threeCellShips = 0;
            result.twoCellShips = 0;
            result.oneCellShips = 0;
            return result;
        }

        public Field getSecondMap()
        {
            Field result = new Field();
            result.field = new Cell[10, 10] {
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Ship, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Ship, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Ship, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Ship, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship},
                { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty},
                { Cell.Ship, Cell.Ship, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship, Cell.Empty, Cell.Ship, Cell.Ship}
            };

            result.fourCellShips = 0;
            result.threeCellShips = 0;
            result.twoCellShips = 0;
            result.oneCellShips = 1;
            return result;
        }
    }
}

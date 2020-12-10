using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Model;
using Battle_sea.Services.FileDataGateway;
using Battle_sea.Services.BL;

namespace Battle_sea.Services.Application
{
    class FieldArrangeApplicationService
    {
        public event Action<Field> fieldReadyToRender;
        public event Action allShipsPlaced;

        private ShipType currentShipType = ShipType.OneCell;
        private bool isHorizontalMode = false;

        private ApplicationStorage storage = ApplicationStorage.getInstance();
        private FileRoomProvider roomProvider = FileRoomProvider.getInstance();
        private FieldBLService fieldBL = FieldBLService.getInstance();
        private ShipTypeBL shipTypeBL = ShipTypeBL.getInstance();

        private bool isFirstPlayer;

        public FieldArrangeApplicationService(bool isFirstPlayer = true)
        {
            this.isFirstPlayer = isFirstPlayer;
        }
        public void changeShipPosition(Point cursor)
        {
            Room room = roomProvider.getRoomByName(storage.getCurrentRoomName());
            Field field = getField(room);
            Field fieldToRender = fieldBL.placeShipGhost(field, cursor, currentShipType, isHorizontalMode);
            field = fieldToRender;
            field.selectedShipType = currentShipType;
            invokeIvents(fieldToRender);
        }

        public void placeShip(Point cursor)
        {
            Room room = roomProvider.getRoomByName(storage.getCurrentRoomName(), true);
            Field field = getField(room);
            bool canPlaceShip = fieldBL.canPlaceShip(field, cursor, currentShipType, isHorizontalMode);
            if (canPlaceShip)
            {
                Field fieldToRender = fieldBL.placeShip(field, cursor, currentShipType, isHorizontalMode);
                if (isFirstPlayer)
                {
                    room.firstPlayer.myField = fieldToRender;
                } else
                {
                    room.secondPlayer.myField = fieldToRender;
                }
                fieldToRender.selectedShipType = currentShipType;
                roomProvider.saveRoom(room);
                invokeIvents(fieldToRender);
            }
        }

        public void changeShipType()
        {
            Room room = roomProvider.getRoomByName(storage.getCurrentRoomName());
            Field field = getField(room);
            currentShipType = shipTypeBL.getNextShipType(currentShipType, field.fourCellShips, field.threeCellShips, field.twoCellShips, field.oneCellShips);
            field.selectedShipType = currentShipType;
        }

        public void changeShipOrientation()
        {
            isHorizontalMode = !isHorizontalMode;
        }

        private Field getField(Room room)
        {
            return isFirstPlayer ? room.firstPlayer.myField : room.secondPlayer.myField;
        }

        private void invokeIvents(Field fieldToRender)
        {
            if (fieldBL.isAllShipPlaces(fieldToRender))
            {
                allShipsPlaced?.Invoke();
            }
            else
            {
                fieldReadyToRender?.Invoke(fieldToRender);
            }
        }
    }
}

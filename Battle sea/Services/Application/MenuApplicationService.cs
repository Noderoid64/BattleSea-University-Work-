using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Battle_sea.Services.BL;
using Battle_sea.Core;
using Battle_sea.Model;
using Battle_sea.Services.FileDataGateway;

namespace Battle_sea.Services.Application
{
    class MenuApplicationService
    {
        private ApplicationStorage storage;
        private RoomBlService roomBlService = RoomBlService.getInstance();
        private FileRoomProvider roomProvider = FileRoomProvider.getInstance();

        public MenuApplicationService(ApplicationStorage storage)
        {
            this.storage = storage;
        }
        public bool initNewRoom(String roomName)
        {
            bool operationResult = true;

            String hostPlayerName = storage.getPlayerName();
            bool canCreateRoom = roomBlService.isRoomAlreadyExist(roomProvider.getAllRoomNames(), roomName);
            if (canCreateRoom)
            {
                Room room = roomBlService.createNewRoom(roomName, hostPlayerName);
                roomProvider.saveRoom(room);
                storage.setCurrentRoomName(roomName);
            } else
            {
                operationResult = false;
            }

            return operationResult;
        }

        public bool connectToRoom(String roomName)
        {
            bool operationResult = false;
            Room roomToConnect = roomProvider.getRoomByName(roomName);
            bool canConnect = roomToConnect.secondPlayer.name != null;
            if (canConnect)
            {
                storage.setCurrentRoomName(roomName);
                operationResult = true;
            }
            return operationResult;
        }

        public bool isGameStarted(String roomName)
        {
            bool operationResult = false;
            Room roomToConnect = roomProvider.getRoomByName(roomName);
            bool canConnect = FieldBLService.getInstance().isAllShipPlaces(roomToConnect.secondPlayer.myField);
            if (canConnect)
            {
                operationResult = true;
            }
            return operationResult;
        }

        public Field getWaitingInfo(bool isFirstPlayer)
        {
            Room roomInfo = roomProvider.getRoomByName(storage.getCurrentRoomName());
            
            Field enemyFieldInfo = isFirstPlayer ? roomInfo.secondPlayer.myField : roomInfo.firstPlayer.myField;

            if (FieldBLService.getInstance().isAllShipPlaces(enemyFieldInfo))
            {
                this.storage.isWaitingForOtherPlayer = false;
            }
            return enemyFieldInfo;
        }
    }
}

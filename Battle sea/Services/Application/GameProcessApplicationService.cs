using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Battle_sea.Model;
using Battle_sea.Services.BL;
using Battle_sea.Services.FileDataGateway;

namespace Battle_sea.Services.Application
{
    class GameProcessApplicationService
    {

        public event Action<Field, Field> renderMyTurn;
        public event Action<Field, Field> renderEnemyTurn;

        private ApplicationStorage storage = ApplicationStorage.getInstance();
        private RoomBlService roomBl = RoomBlService.getInstance();
        private FileRoomProvider roomProvider = FileRoomProvider.getInstance();
        private FieldBLService fieldBL = FieldBLService.getInstance();
        public void initGameProcess(bool isHost)
        {
            determinateWhoTheFirst(isHost);
        }

        private void determinateWhoTheFirst(bool isHost)
        {
            Room room;
            if (isHost)
            {
                bool isFirstPlayerTime = roomBl.isTheHostFirstTime();
                room = roomProvider.getRoomByName(storage.getCurrentRoomName(), true);
                room.isFirstPlayerTime = isFirstPlayerTime;
                roomProvider.saveRoom(room);
            } else
            {
                Thread.Sleep(200);
                room = roomProvider.getRoomByName(storage.getCurrentRoomName());
            }
            

            storage.iamFirstPlayer = isHost;
            if (roomBl.isMyTurn(room, storage.iamFirstPlayer))
            {
                renderMyTurn?.Invoke(room.firstPlayer.myField, room.firstPlayer.enemyField);
            } else
            {
                renderEnemyTurn?.Invoke(room.secondPlayer.myField, room.secondPlayer.enemyField);
            }
        }
    
        public void changeTargetPosition(Point cursor)
        {
            Room room = roomProvider.getRoomByName(storage.getCurrentRoomName());
            Field myField = roomBl.getMyField(room, storage.iamFirstPlayer);
            Field enemyField = roomBl.getMyEnemyField(room, storage.iamFirstPlayer);
            Field enemyFieldResult = fieldBL.placeTarget(enemyField, cursor);
            renderMyTurn?.Invoke(myField, enemyFieldResult);
        }
        
        public void makeShot(Point cursor)
        {
            Room room = roomProvider.getRoomByName(storage.getCurrentRoomName(), true);
            Field myField = roomBl.getMyField(room, storage.iamFirstPlayer);
            Field myEnemyField = roomBl.getMyEnemyField(room, storage.iamFirstPlayer);
            Field enemyField = roomBl.getEnemyField(room, storage.iamFirstPlayer);
            if (fieldBL.canPlaceTarget(myEnemyField, cursor))
            {
                bool isDestroy;
                fieldBL.makeShot(myEnemyField, enemyField, cursor, out isDestroy);
                if (isDestroy)
                {
                    roomProvider.saveRoom(room);
                    renderMyTurn?.Invoke(myField, myEnemyField);
                } else
                {
                    roomBl.setNotMyTurn(room, storage.iamFirstPlayer);
                    roomProvider.saveRoom(room);
                    renderEnemyTurn?.Invoke(myField, myEnemyField);
                } 
            } else
            {
                roomProvider.saveRoom(room);
                renderMyTurn?.Invoke(myField, myEnemyField);
            }
            
        }

        public void waitForPlayerShot()
        {
            Room room = roomProvider.getRoomByName(storage.getCurrentRoomName());
            Field myField = roomBl.getMyField(room, storage.iamFirstPlayer);
            Field myEnemyField = roomBl.getMyEnemyField(room, storage.iamFirstPlayer);
            Field enemyField = roomBl.getEnemyField(room, storage.iamFirstPlayer);
            while(!roomBl.isMyTurn(room, storage.iamFirstPlayer))
            {
                Thread.Sleep(500);
                room = roomProvider.getRoomByName(storage.getCurrentRoomName());
            }
            renderMyTurn?.Invoke(myField, myEnemyField);
        }
    }
}

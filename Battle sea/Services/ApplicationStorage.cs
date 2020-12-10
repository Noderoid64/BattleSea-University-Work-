using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Model;

namespace Battle_sea.Services
{
    class ApplicationStorage
    {
        #region

        private static ApplicationStorage instanse;

        public static ApplicationStorage getInstance()
        {
            if (instanse == null)
            {
                instanse = new ApplicationStorage();
            }
            return instanse;
        }

        private ApplicationStorage() { }

        #endregion

        private String playerName;
        private String currentRoomName;
        public bool isWaitingForOtherPlayer = false;
        public bool iamFirstPlayer;

        public string getPlayerName()
        {
            return playerName;
        }

        public void setPlayerName(String playerName)
        {
            this.playerName = playerName;
        }

        public string getCurrentRoomName()
        {
            return currentRoomName;
        }

        public void setCurrentRoomName(String roomName)
        {
            this.currentRoomName = roomName;
        }
    }
}

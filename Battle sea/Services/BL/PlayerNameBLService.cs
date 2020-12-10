using System;
using System.Collections.Generic;
using System.Text;

namespace Battle_sea.Core
{
    class PlayerNameBLService
    {
        private static PlayerNameBLService instance;

        private PlayerNameBLService() { }

        public static PlayerNameBLService getInstance()
        {
            if (instance == null)
            {
                instance = new PlayerNameBLService();
            }
            return instance;
        }
        public bool isPlayerNameValid(String playerName)
        {
            return playerName != null && playerName.Trim().Length > 4 && playerName.Trim().Length < 10;
        }
    }
}

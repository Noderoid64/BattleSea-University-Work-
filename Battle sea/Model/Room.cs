using System;
using System.Collections.Generic;
using System.Text;

namespace Battle_sea.Model
{
    class Room
    {
        public String name { get; set; }
        public Boolean isFirstPlayerTime { get; set; }
        public Player firstPlayer { get; set; }
        public Player secondPlayer { get; set; }
    }
}

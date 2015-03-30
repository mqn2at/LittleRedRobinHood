using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.Component
{
    class Shackle
    {
        public readonly int entityID;
        public readonly int firstPointID;
        public readonly int secondPointID;
        public bool playerOnShackle;
        public int timer;

        public Shackle(int myID, int fpID, int spID)
        {
            this.entityID = myID;
            this.firstPointID = fpID;
            this.secondPointID = spID;
            this.playerOnShackle = false;
            this.timer = 3; //seconds?
        }

        public Shackle(int myID, int fpID, int spID, int duration)
        {
            this.entityID = myID;
            this.firstPointID = fpID;
            this.secondPointID = spID;
            this.playerOnShackle = false;
            this.timer = duration; //seconds?
        }
    }
}

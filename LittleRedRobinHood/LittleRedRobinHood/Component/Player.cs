using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.Component
{
    class Player
    {
        public readonly int entityID;
        public readonly int MAX_ARROWS = 3;
        public readonly int MAX_SHACKLES = 3;
        public int health = 3;
        public int arrows;
        public int shackles;
        public bool grounded;
        public bool running;
        public bool jumping;
        public int dy;
        public Player(int id)
        {
            this.entityID = id;
            this.arrows = MAX_ARROWS;
            this.shackles = MAX_SHACKLES;
            this.dy = 0;
            this.grounded = true;
            this.running = false;
            this.jumping = false;
        }
    }
}

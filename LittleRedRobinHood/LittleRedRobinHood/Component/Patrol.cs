using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LittleRedRobinHood.Component
{
    class Patrol
    {
        public readonly int entityID;
        public readonly List<Vector2> waypoint;
        public readonly int speed;
        public int currentDest;
        public bool is_right = false;
        public bool isCyclical;
        public int? dx = null;
        public int? dy = null;

        public Patrol(int id, List<Vector2> path, int spd)
        {
            this.entityID = id;
            this.waypoint = path;
            this.speed = spd;
            this.currentDest = 0;
            isCyclical = true;
            //prevLoc = new Vector2(-1, -1);
        }
        public Patrol(int id, List<Vector2> path, int spd, bool cycle)
        {
            this.entityID = id;
            this.waypoint = path;
            this.speed = spd;
            this.currentDest = 0;
            isCyclical = cycle;
            //prevLoc = new Vector2(-1, -1);
        }
        public void setCurrentDest(int dest)
        {
            this.currentDest = dest;
            //prevLoc = new Vector2(-1, -1);
        }
    }
}

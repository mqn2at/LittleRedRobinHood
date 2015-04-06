﻿using System;
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

        public Patrol(int id, List<Vector2> path, int spd)
        {
            this.entityID = id;
            this.waypoint = path;
            this.speed = spd;
            this.currentDest = 0;
            isCyclical = true;
        }
        public Patrol(int id, List<Vector2> path, int spd, bool cycle)
        {
            this.entityID = id;
            this.waypoint = path;
            this.speed = spd;
            this.currentDest = 0;
            isCyclical = cycle;
        }
        public void setCurrentDest(int dest)
        {
            this.currentDest = dest;
        }
    }
}

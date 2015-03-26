﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LittleRedRobinHood.Component
{
    class Patrol
    {
        public readonly List<Vector2> waypoint;
        public readonly int speed;
        public int currentDest;

        public Patrol(List<Vector2> path, int spd)
        {
            this.waypoint = path;
            this.speed = spd;
            this.currentDest = 0;
        }
    }
}

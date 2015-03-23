using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LittleRedRobinHood.Component
{
    class Patrol
    {
        public readonly List<Vector2> waypoint;
        public int currentDest;

        public Patrol(List<Vector2> path)
        {
            this.waypoint = path;
            this.currentDest = 0;
        }
    }
}

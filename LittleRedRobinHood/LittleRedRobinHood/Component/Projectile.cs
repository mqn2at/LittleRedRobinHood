using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.Component
{
    class Projectile
    {
        public readonly bool isArrow;
        public readonly int vx;
        public readonly int vy;

        public Projectile(bool arrow, int v_x, int v_y)
        {
            this.isArrow = arrow;
            this.vx = v_x;
            this.vy = v_y;
        }
    }
}

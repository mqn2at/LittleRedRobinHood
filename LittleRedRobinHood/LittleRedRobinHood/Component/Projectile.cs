using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.Component
{
    class Projectile
    {
        public readonly bool isArrow;
        public readonly double angle;
        public readonly int speed;

        public Projectile(bool arrow, double ang, int spd)
        {
            this.isArrow = arrow;
            this.angle = ang;
            this.speed = spd;
        }
        
        //public readonly int vx;
        //public readonly int vy;
        /*
        public Projectile(bool arrow, int v_x, int v_y)
        {
            this.isArrow = arrow;
            this.vx = v_x;
            this.vy = v_y;
        }
         * */

    }
}

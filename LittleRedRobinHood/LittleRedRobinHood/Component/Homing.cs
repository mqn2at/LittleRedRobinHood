using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.Component
{
    class Homing
    {
        public readonly bool isFlying;
        public readonly string myLogic;

        public Homing(bool fly, string commands)
        {
            this.isFlying = fly;
            this.myLogic = commands;
        }
    }
}

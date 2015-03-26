using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.System
{
    class ProjectileSystem
    {
        public void Update(ComponentManager componentManager)
        {
            for (int i = 0; i < componentManager.getEntities().Count; i++)
            {
                //Ignore if not projectile
                if (!componentManager.getEntities()[i].isProjectile)
                {
                    continue;
                }

                int entityID = componentManager.getEntities()[i].entityID;

                //Remove if out of bounds


                //Move Projectile
                double dx = Math.Cos(componentManager.getProjectiles()[entityID].angle) * componentManager.getProjectiles()[entityID].speed;
                double dy = Math.Sin(componentManager.getProjectiles()[entityID].angle) * componentManager.getProjectiles()[entityID].speed;
                componentManager.getCollides()[entityID].hitbox.X += (int)dx;
                componentManager.getCollides()[entityID].hitbox.Y += (int)dy;
            }
        }
    }
}

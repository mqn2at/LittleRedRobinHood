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
                componentManager.getCollides()[entityID].hitbox.X += componentManager.getProjectiles()[entityID].vx;
                componentManager.getCollides()[entityID].hitbox.Y += componentManager.getProjectiles()[entityID].vy;
            }
        }
    }
}

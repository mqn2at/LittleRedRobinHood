using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleRedRobinHood.Entities;

namespace LittleRedRobinHood.System
{
    class ProjectileSystem
    {
        public void Update(ComponentManager componentManager, GraphicsDevice gd)
        {
            
            foreach (KeyValuePair<int, Entity> ent in componentManager.getEntities())
            {
                
                //Ignore if not projectile
                if (!ent.Value.isProjectile) 
                {
                    continue;
                }

                int entityID = ent.Value.entityID;

                //Remove if out of bounds
                Rectangle rectangle = componentManager.getCollides()[entityID].hitbox;
                if (rectangle.X > gd.Viewport.Width || rectangle.Y > gd.Viewport.Height ||
                    rectangle.X + rectangle.Width < 0 || rectangle.Y + rectangle.Height < 0)
                {
                    componentManager.getProjectiles().Remove(entityID);
                    componentManager.getSprites().Remove(entityID);
                    componentManager.getCollides().Remove(entityID);
                    componentManager.getEntities().Remove(entityID);
                    //did not remove from shacklesPlatforms, players, and patrols
                }
                //Move Projectile
                else
                {
                    double dx = Math.Cos(componentManager.getProjectiles()[entityID].angle) * componentManager.getProjectiles()[entityID].speed;
                    double dy = Math.Sin(componentManager.getProjectiles()[entityID].angle) * componentManager.getProjectiles()[entityID].speed;
                    componentManager.getCollides()[entityID].hitbox.X += (int)dx;
                    componentManager.getCollides()[entityID].hitbox.Y += (int)dy;
                }
            }
            
        }
    }
}

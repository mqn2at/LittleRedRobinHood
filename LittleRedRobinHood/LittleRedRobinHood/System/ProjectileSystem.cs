using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleRedRobinHood.Entities;
using LittleRedRobinHood.Component;

namespace LittleRedRobinHood.System
{
    class ProjectileSystem
    {
        public void Update(ComponentManager componentManager, GraphicsDevice gd)
        {
            List<int> toBeRemoved = new List<int>();
            foreach (KeyValuePair<int, Entity> ent in componentManager.getEntities())
            {
                
                //Ignore if not projectile
                if (!ent.Value.isProjectile) 
                {
                    continue;
                }

                int entityID = ent.Value.entityID;

                //Remove if out of bounds IF NOT PINECONE
                Rectangle rectangle = componentManager.getCollides()[entityID].hitbox;
                if ((rectangle.X > gd.Viewport.Width || rectangle.Y > gd.Viewport.Height ||
                    rectangle.X + rectangle.Width < 0 || rectangle.Y + rectangle.Height < 0)
                    && !componentManager.getEntities()[entityID].isPatrol)
                {
                    toBeRemoved.Add(entityID);
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
            foreach (int id in toBeRemoved)
            {
                Player player = componentManager.getPlayers()[componentManager.playerID];
                if (componentManager.getProjectiles()[id].isArrow)
                {
                    if (player.MAX_ARROWS > player.arrows)
                    {
                        //Console.WriteLine("+1 arrow");
                        player.arrows += 1;
                    }
                }
                else
                {
                    if (player.MAX_SHACKLES > player.shackles)
                    {
                        //Console.WriteLine("+1 shackle");
                        player.shackles += 1;
                    }
                }
                componentManager.getProjectiles().Remove(id);
                componentManager.getSprites().Remove(id);
                componentManager.getCollides().Remove(id);
                componentManager.getEntities().Remove(id);
            }
        }
    }
}

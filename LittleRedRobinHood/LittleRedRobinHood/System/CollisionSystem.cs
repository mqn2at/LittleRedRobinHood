using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleRedRobinHood.Component;
using LittleRedRobinHood.Entities;
using Microsoft.Xna.Framework;

namespace LittleRedRobinHood.System
{
    class CollisionSystem
    {
        public bool Update(ComponentManager componentManager)
        {
            
            int[] entityList = componentManager.getEntities().Keys.ToArray();
            for (int i = 0; i < entityList.Count(); i++)
            {
                for (int j = i + 1; j < entityList.Count(); j++)
                {
                    //Check to see if two entities are collidable
                    if (!componentManager.getEntities()[entityList[i]].isCollide || !componentManager.getEntities()[entityList[j]].isCollide)
                    {
                        continue;
                    }

                    Collide collide1 = componentManager.getCollides()[componentManager.getEntities()[entityList[i]].entityID];
                    Collide collide2 = componentManager.getCollides()[componentManager.getEntities()[entityList[j]].entityID];
                    
                    bool COLLIDED = collide1.hitbox.Intersects(collide2.hitbox);

                    if (COLLIDED)
                    {
                        //Player Collision
                        if (componentManager.getEntities()[entityList[i]].isPlayer || componentManager.getEntities()[entityList[j]].isPlayer)
                        {
                            int playerID = 0;
                            int objectID = 0;
                            int objectIndex = 0;

                            if (componentManager.getEntities()[entityList[i]].isPlayer)
                            {
                                playerID = componentManager.getEntities()[entityList[i]].entityID;
                                objectID = componentManager.getEntities()[entityList[j]].entityID;
                                objectIndex = entityList[j];
                            }
                            else
                            {
                                playerID = componentManager.getEntities()[entityList[j]].entityID;
                                objectID = componentManager.getEntities()[entityList[i]].entityID;
                                objectIndex = entityList[i];
                            }

                            //Player - Endpoint Collision
                            if (componentManager.getCollides()[objectID].isEndPoint)
                            {
                                Console.WriteLine("FINISHED STAGE!!!");
                                return true;
                            }

                            //Player - Enemy Collision
                            else if (componentManager.getCollides()[objectID].isEnemy)
                            {
                                componentManager.getPlayers()[playerID].health--;
                                //ADD SOME SORT OF PUSHING
                            }

                            //Player - Shackle Collision
                            else if (componentManager.getEntities()[objectIndex].isShackle)
                            {
                                int firstPointID = componentManager.getShackles()[objectID].firstPointID;
                                int secondPointID = componentManager.getShackles()[objectID].secondPointID;

                                Vector2 bottomPoint;
                                Vector2 topPoint;


                                //Vertical


                                //Horizontal

                            }

                            //Player - Object Collision
                            else if (!componentManager.getEntities()[objectIndex].isShackle)
                            {
                                Dictionary<int, Collide> collideables = componentManager.getCollides();
                                
                                //Y-collision
                                //bottom side of object
                                if (collideables[playerID].hitbox.Y < collideables[objectID].hitbox.Y + collideables[objectID].hitbox.Height
                                    && collideables[playerID].hitbox.Y > collideables[objectID].hitbox.Y
                                    && collideables[playerID].hitbox.Y + (int)(0.5 * collideables[playerID].hitbox.Height) > collideables[objectID].hitbox.Y + collideables[objectID].hitbox.Height)
                                {
                                    Console.WriteLine("collide BOTTOM " + collideables[playerID].hitbox.Y + " | " + collideables[objectID].hitbox.Y + " | " + collideables[objectID].hitbox.Height);
                                    componentManager.getCollides()[playerID].hitbox.Y = collideables[objectID].hitbox.Y + collideables[objectID].hitbox.Height;
                                }
                                //top side of object
                                else if (collideables[playerID].hitbox.Y + collideables[playerID].hitbox.Height > collideables[objectID].hitbox.Y
                                    && collideables[playerID].hitbox.Y + collideables[playerID].hitbox.Height < collideables[objectID].hitbox.Y + collideables[objectID].hitbox.Height
                                    && collideables[playerID].hitbox.Y + (int)(0.5 * collideables[playerID].hitbox.Height) < collideables[objectID].hitbox.Y)
                                {
                                    componentManager.getPlayers()[playerID].grounded = true;
                                    Console.WriteLine("collide TOP");
                                    componentManager.getCollides()[playerID].hitbox.Y = collideables[objectID].hitbox.Y - collideables[playerID].hitbox.Height;
                                }
                                
                                //X-collision
                                //left side of object
                                else if (collideables[playerID].hitbox.X + collideables[playerID].hitbox.Width > collideables[objectID].hitbox.X
                                    && collideables[playerID].hitbox.X + collideables[playerID].hitbox.Width < collideables[objectID].hitbox.X + collideables[objectID].hitbox.Width)
                                {
                                    Console.WriteLine("collide LEFT");
                                    componentManager.getCollides()[playerID].hitbox.X = collideables[objectID].hitbox.X - collideables[playerID].hitbox.Width;
                                }
                                //right side of object
                                else if (collideables[playerID].hitbox.X < collideables[objectID].hitbox.X + collideables[objectID].hitbox.Width
                                    && collideables[playerID].hitbox.X > collideables[objectID].hitbox.X)
                                {
                                    Console.WriteLine("collide RIGHT");
                                    componentManager.getCollides()[playerID].hitbox.X = collideables[objectID].hitbox.X + collideables[objectID].hitbox.Width;
                                }
                            }
                        }

                        //Projectile Collision
                        else if (componentManager.getEntities()[entityList[i]].isProjectile || componentManager.getEntities()[entityList[j]].isProjectile)
                        {
                            int projectileID = 0;
                            int objectID = 0;
                            int objectIndex = 0;

                            if (componentManager.getEntities()[entityList[i]].isProjectile)
                            {
                                projectileID = componentManager.getEntities()[entityList[i]].entityID;
                                objectID = componentManager.getEntities()[entityList[j]].entityID;
                                objectIndex = entityList[j];
                            }
                            else
                            {
                                projectileID = componentManager.getEntities()[entityList[j]].entityID;
                                objectID = componentManager.getEntities()[entityList[i]].entityID;
                                objectIndex = entityList[i];
                            }

                            //Arrow Collision
                            if (componentManager.getProjectiles()[projectileID].isArrow)
                            {
                                //Arrow - Damageable Enemy Collision NEED TO ADD DAMAGE
                                ///if (componentManager.getCollides()[objectID].)

                                //Arrow - Shackle Platform Collision
                                if (componentManager.getEntities()[objectIndex].isShackle)
                                {
                                    //Detect Collision


                                    //Remove shackle
                                }
                            }

                            //Shackle - Shackleable Collision
                            else if (!componentManager.getProjectiles()[projectileID].isArrow && componentManager.getCollides()[objectID].isShackleable)
                            {
                                //Check to see if 
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}

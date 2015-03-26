using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleRedRobinHood.Component;
using LittleRedRobinHood.Entities;

namespace LittleRedRobinHood.System
{
    class CollisionSystem
    {
        public void collide(ComponentManager componentManager)
        {
            for (int i = 0; i < componentManager.getEntities().Count; i++)
            {
                for (int j = i + 1; j < componentManager.getEntities().Count; j++)
                {
                    //Check to see if two entities are collidable
                    if (!componentManager.getEntities()[i].isCollide || !componentManager.getEntities()[j].isCollide)
                    {
                        break;
                    }

                    Collide collide1 = componentManager.getCollides()[componentManager.getEntities()[i].entityID];
                    Collide collide2 = componentManager.getCollides()[componentManager.getEntities()[j].entityID];
                    
                    bool COLLIDED = collide1.hitbox.Intersects(collide2.hitbox);

                    if (COLLIDED)
                    {
                        //Player Collision
                        if (componentManager.getEntities()[i].isPlayer || componentManager.getEntities()[j].isPlayer )
                        {
                            int playerID = 0;
                            int objectID = 0;
                            int objectIndex = 0;

                            if (componentManager.getEntities()[i].isPlayer)
                            {
                                playerID = componentManager.getEntities()[i].entityID;
                                objectID = componentManager.getEntities()[j].entityID;
                                objectIndex = j;
                            }
                            else
                            {
                                playerID = componentManager.getEntities()[j].entityID;
                                objectID = componentManager.getEntities()[i].entityID;
                                objectIndex = i;
                            }

                            //Player - Enemy Collision
                            if (componentManager.getCollides()[objectID].isEnemy)
                            {
                                componentManager.getPlayers()[playerID].health--;
                            }

                            //Player - Shackle Collision


                            //Player - Object Collision
                            else if (!componentManager.getEntities()[objectIndex].isShackle)
                            {
                                Dictionary<int, Collide> collideables = componentManager.getCollides();
                                //X-collision
                                //left side of object
                                if (collideables[playerID].hitbox.X + collideables[playerID].hitbox.Width > collideables[objectID].hitbox.X)
                                {
                                    componentManager.getCollides()[playerID].hitbox.X = collideables[objectID].hitbox.X - collideables[playerID].hitbox.Width;
                                }
                                //right side of object
                                else if (collideables[playerID].hitbox.X < collideables[objectID].hitbox.X + collideables[objectID].hitbox.Width)
                                {
                                    componentManager.getCollides()[playerID].hitbox.X = collideables[objectID].hitbox.X + collideables[objectID].hitbox.Width;
                                }

                                //Y-collision
                                //bottom side of object
                                else if (collideables[playerID].hitbox.Y < collideables[objectID].hitbox.Y + collideables[objectID].hitbox.Height)
                                {
                                    componentManager.getCollides()[playerID].hitbox.Y = collideables[objectID].hitbox.Y + collideables[objectID].hitbox.Height;
                                }
                                //top side of object
                                else if (collideables[playerID].hitbox.Y + collideables[playerID].hitbox.Height > collideables[objectID].hitbox.Y)
                                {
                                    componentManager.getCollides()[playerID].hitbox.Y = collideables[objectID].hitbox.Y - collideables[playerID].hitbox.Height;
                                }
                            }
                        }

                        //Projectile Collision
                        else if (componentManager.getEntities()[i].isProjectile || componentManager.getEntities()[j].isProjectile)
                        {
                            int projectileID = 0;
                            int objectID = 0;
                            int objectIndex = 0;

                            if (componentManager.getEntities()[i].isProjectile)
                            {
                                projectileID = componentManager.getEntities()[i].entityID;
                                objectID = componentManager.getEntities()[j].entityID;
                                objectIndex = j;
                            }
                            else
                            {
                                projectileID = componentManager.getEntities()[j].entityID;
                                objectID = componentManager.getEntities()[i].entityID;
                                objectIndex = i;
                            }
                            
                            
                            //Arrow - Enemy Collision


                            //Arrow - Shackle Platform Collision
                            

                            //Shackle - Shackleable Collision

                        }
                    }
                }
            }                
        }
    }
}

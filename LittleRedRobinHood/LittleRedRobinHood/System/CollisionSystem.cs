using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleRedRobinHood.Component;
using LittleRedRobinHood.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LittleRedRobinHood.System
{
    class CollisionSystem
    {
        public bool Update(ComponentManager manager, GraphicsDevice gd)
        {
            
            int[] entityList = manager.getEntities().Keys.ToArray();
            List<int> toBeRemoved = new List<int>();
            for (int i = 0; i < entityList.Count(); i++)
            {
                for (int j = i + 1; j < entityList.Count(); j++)
                {
                    //Check to see if two entities are collidable
                    if (!manager.getEntities()[entityList[i]].isCollide || !manager.getEntities()[entityList[j]].isCollide)
                    {
                        continue;
                    }

                    Collide collide1 = manager.getCollides()[manager.getEntities()[entityList[i]].entityID];
                    Collide collide2 = manager.getCollides()[manager.getEntities()[entityList[j]].entityID];
                    
                    bool COLLIDED = collide1.hitbox.Intersects(collide2.hitbox);

                    if (COLLIDED)
                    {
                        //Player Collision
                        if (manager.getEntities()[entityList[i]].isPlayer || manager.getEntities()[entityList[j]].isPlayer)
                        {
                            int playerID = 0;
                            int objectID = 0;
                            int objectIndex = 0;

                            if (manager.getEntities()[entityList[i]].isPlayer)
                            {
                                playerID = manager.getEntities()[entityList[i]].entityID;
                                objectID = manager.getEntities()[entityList[j]].entityID;
                                objectIndex = entityList[j];
                            }
                            else
                            {
                                playerID = manager.getEntities()[entityList[j]].entityID;
                                objectID = manager.getEntities()[entityList[i]].entityID;
                                objectIndex = entityList[i];
                            }

                            //Player - Endpoint Collision
                            if (manager.getCollides()[objectID].isEndPoint)
                            {
                                Console.WriteLine("FINISHED STAGE!!!");
                                return true;
                            }

                            //Player - Enemy Collision
                            else if (manager.getCollides()[objectID].isEnemy)
                            {
                                manager.getPlayers()[playerID].health--;
                                //ADD SOME SORT OF PUSHING
                            }

                            //Player - Shackle Collision WILL NEED TO BE IMPROVED
                            //Currently not accounting for shackles as walls
                            else if (manager.getEntities()[objectIndex].isShackle)
                            {
                                int firstPointID = manager.getShackles()[objectID].firstPointID;
                                int secondPointID = manager.getShackles()[objectID].secondPointID;

                                Vector2 topPoint;
                                Vector2 bottomPoint;

                                if(manager.getCollides()[firstPointID].hitbox.Y > manager.getCollides()[secondPointID].hitbox.Y){
                                    Rectangle topRect = manager.getCollides()[secondPointID].hitbox;
                                    Rectangle bottomRect = manager.getCollides()[firstPointID].hitbox;
                                    topPoint = new Vector2(topRect.X + (int)(topRect.Width / 2.0), topRect.Y + (int)(topRect.Height / 2.0));
                                    bottomPoint = new Vector2(bottomRect.X + (int)(bottomRect.Width / 2.0), bottomRect.Y + (int)(bottomRect.Height / 2.0));
                                }
                                else
                                {
                                    Rectangle topRect = manager.getCollides()[firstPointID].hitbox;
                                    Rectangle bottomRect = manager.getCollides()[secondPointID].hitbox;
                                    topPoint = new Vector2(topRect.X + (int)(topRect.Width / 2.0), topRect.Y + (int)(topRect.Height / 2.0));
                                    bottomPoint = new Vector2(bottomRect.X + (int)(bottomRect.Width / 2.0), bottomRect.Y + (int)(bottomRect.Height / 2.0));
                                }
                                //To translate from image coordinates to normal coordinates
                                topPoint.Y = gd.Viewport.Height - topPoint.Y;
                                bottomPoint.Y = gd.Viewport.Height - bottomPoint.Y;

                                double slope = (double)(topPoint.Y - bottomPoint.Y) / (double)(topPoint.X - bottomPoint.X);
                                Rectangle playerCollide = manager.getCollides()[playerID].hitbox;
                                int playerMidX = playerCollide.X + (int)(playerCollide.Width / 2.0);
                                Vector2 playerBottom = new Vector2(playerMidX, gd.Viewport.Height - playerCollide.Y);
                                Vector2 playerTop = new Vector2(playerMidX, gd.Viewport.Height - playerCollide.Y + playerCollide.Height);

                                //Vertical shackle platform


                                //Horizontal shackle platform
                                if (Math.Min(topPoint.X, bottomPoint.X) < playerMidX && playerMidX < Math.Max(topPoint.X, bottomPoint.X)
                                    && manager.getPlayers()[manager.playerID].dy >= 0)
                                {
                                    //Find point on shackle platform that is at the middle of the player's width
                                    double shackleY = slope * (double)(playerMidX - bottomPoint.X) + bottomPoint.Y;
                                    double playerMidY = (double)(playerTop.Y + playerBottom.Y) / 2.0;

                                    Console.WriteLine(shackleY + " | " + playerMidY + " | " + playerBottom.Y);
                                    //Player on top of shackle
                                    if (shackleY < playerMidY)
                                    {
                                        manager.getCollides()[playerID].hitbox.Y = (gd.Viewport.Height - (int)shackleY) - playerCollide.Height;
                                        manager.getPlayers()[playerID].grounded = true;
                                    }
                                    //Player on bottom of shackle
                                    else if (shackleY > playerMidY)
                                    {
                                        manager.getCollides()[playerID].hitbox.Y = gd.Viewport.Height - (int)shackleY;
                                    }
                                }
                            }

                            //Player - Object Collision
                            else if (!manager.getEntities()[objectIndex].isShackle && !manager.getEntities()[objectIndex].isProjectile)
                            {
                                Dictionary<int, Collide> collideables = manager.getCollides();
                                Rectangle playerHitbox = collideables[playerID].hitbox;
                                Rectangle objectHitbox = collideables[objectID].hitbox;
                                //Y-collision
                                //bottom side of object
                                if (playerHitbox.Y < objectHitbox.Y + objectHitbox.Height
                                    && playerHitbox.Y > objectHitbox.Y
                                    && playerHitbox.Y + (int)(0.3 * playerHitbox.Height) > objectHitbox.Y + objectHitbox.Height)
                                {
                                    manager.getCollides()[playerID].hitbox.Y = objectHitbox.Y + objectHitbox.Height;
                                }
                                //top side of object
                                else if (playerHitbox.Y + playerHitbox.Height > objectHitbox.Y
                                    && playerHitbox.Y + playerHitbox.Height < objectHitbox.Y + objectHitbox.Height
                                    && playerHitbox.Y + (int)(0.73 * playerHitbox.Height) < objectHitbox.Y)
                                {
                                    manager.getPlayers()[playerID].grounded = true;
                                    manager.getCollides()[playerID].hitbox.Y = objectHitbox.Y - playerHitbox.Height;
                                }

                                //X-collision
                                //left side of object
                                else if (playerHitbox.X + playerHitbox.Width > objectHitbox.X
                                    && playerHitbox.X + playerHitbox.Width < objectHitbox.X + collideables[objectID].hitbox.Width)
                                {
                                    ///Console.WriteLine("collide LEFT");
                                    manager.getCollides()[playerID].hitbox.X = objectHitbox.X - playerHitbox.Width;
                                }
                                //right side of object
                                else if (playerHitbox.X < objectHitbox.X + objectHitbox.Width
                                    && playerHitbox.X > objectHitbox.X)
                                {
                                    ///Console.WriteLine("collide RIGHT");
                                    manager.getCollides()[playerID].hitbox.X = objectHitbox.X + objectHitbox.Width;
                                }
                            }
                        }

                        //Projectile Collision
                        else if (manager.getEntities()[entityList[i]].isProjectile || manager.getEntities()[entityList[j]].isProjectile)
                        {
                            int projectileID;
                            Entity objectEntity;
                            int objectIndex = 0;

                            if (manager.getEntities()[entityList[i]].isProjectile)
                            {
                                projectileID = manager.getEntities()[entityList[i]].entityID;
                                objectEntity = manager.getEntities()[entityList[j]];
                                objectIndex = entityList[j];
                            }
                            else
                            {
                                projectileID = manager.getEntities()[entityList[j]].entityID;
                                objectEntity = manager.getEntities()[entityList[i]];
                                objectIndex = entityList[i];
                            }
                            //Console.WriteLine("Other Object is Shackleable?: " + componentManager.getCollides()[objectEntity.entityID].isShackleable);////

                            //Arrow Collision
                            if (manager.getProjectiles()[projectileID].isArrow)
                            {
                                //Arrow - Damageable Enemy Collision
                                if (manager.getCollides()[objectEntity.entityID].isDamageable)
                                {
                                    //Remove enemy
                                    toBeRemoved.Add(objectEntity.entityID);

                                    //Remove shackles linked to enemy
                                    foreach (int shackleID in manager.getShackles().Keys){
                                        int shackledID1 = manager.getShackles()[shackleID].firstPointID;
                                        int shackledID2 = manager.getShackles()[shackleID].secondPointID;

                                        if (objectEntity.entityID == shackledID1 || objectEntity.entityID == shackledID2)
                                        {
                                            toBeRemoved.Add(shackleID);
                                        }
                                    }

                                }

                                //Arrow - Shackle Platform Collision
                                //COLLISION DETECTION TRACKS HITBOX, NOT ACTUAL SHACKLE
                                else if (objectEntity.isShackle)
                                {
                                    //Detect Collision


                                    //Remove shackle
                                    toBeRemoved.Add(objectEntity.entityID);

                                    //Make player fall if player on shackle
                                    if (manager.getCollides()[objectEntity.entityID].hitbox.Intersects(manager.getCollides()[manager.playerID].hitbox))
                                    {
                                        manager.getPlayers()[manager.playerID].grounded = false;
                                    }

                                    //Unshackle objects
                                    int firstUnshackled = manager.getShackles()[objectEntity.entityID].firstPointID;
                                    int secondUnshackled = manager.getShackles()[objectEntity.entityID].secondPointID;
                                    manager.getCollides()[firstUnshackled].numShackled--;
                                    manager.getCollides()[secondUnshackled].numShackled--;
                                    manager.getPlayers()[manager.playerID].shackles += 1;
                                }
                                toBeRemoved.Add(projectileID);
                                manager.getPlayers()[manager.playerID].arrows += 1;
                            }

                            //Shackle Projectile
                            else 
                            {
                                //Shackle Projectile - Shackleable Collision
                                if (manager.getCollides()[objectEntity.entityID].isShackleable)
                                {
                                    //Check for shackle
                                    int otherID = checkShackle(projectileID, objectEntity, manager);
                                    if (otherID != -1)
                                    {
                                        //Check to see if shackle already exists
                                        bool shackleExists = false;
                                        foreach (Shackle shackle in manager.getShackles().Values)
                                        {
                                            if((shackle.firstPointID == objectEntity.entityID && shackle.secondPointID == otherID)
                                                || (shackle.firstPointID == otherID && shackle.secondPointID == objectEntity.entityID)){
                                                    shackleExists = true;
                                                    manager.getPlayers()[manager.playerID].shackles += 1;
                                                    Console.WriteLine("Shackle already exists!");
                                                    break;
                                                }
                                        }

                                        //Make Shackle if doesn't exist
                                        if (!shackleExists){
                                            //Set two entities as shackled
                                            manager.getCollides()[objectEntity.entityID].numShackled++;
                                            manager.getCollides()[otherID].numShackled++;

                                            //Add Shackle
                                            int newShackleID = manager.addEntity();
                                            Rectangle tempRect1 = manager.getCollides()[objectEntity.entityID].hitbox;
                                            Rectangle tempRect2 = manager.getCollides()[otherID].hitbox;
                                            int rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                                            int rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                                            int rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                                            int rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;

                                            manager.addShackle(newShackleID, objectEntity.entityID, otherID);
                                            manager.addCollide(newShackleID, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);
                                        }                                        
                                    }
                                }
                                    
                                //Shackle Projectile - random collideable
                                else
                                {
                                    manager.getPlayers()[manager.playerID].shackles += 1;
                                    Console.WriteLine("No Shackle Made.");
                                }
                                Console.WriteLine("Number of shackle platforms: " + manager.getShackles().Count);
                            }
                            toBeRemoved.Add(projectileID);
                        }
                    }    
                }
            }
            //Remove entities
            foreach (int id in toBeRemoved)
            {
                manager.getProjectiles().Remove(id);
                manager.getSprites().Remove(id);
                manager.getCollides().Remove(id);
                manager.getPatrols().Remove(id);
                manager.getShackles().Remove(id);
                manager.getEntities().Remove(id);
            }

            return false;
        }

        public int checkShackle(int projectileID, Entity objectEntity, ComponentManager componentManager)
        {
            double angle = componentManager.getProjectiles()[projectileID].angle;
            Collide objectCollide = componentManager.getCollides()[objectEntity.entityID];
            int objX = objectCollide.hitbox.X + (int)(objectCollide.hitbox.Width / 2.0);
            int objY = objectCollide.hitbox.Y + (int)(objectCollide.hitbox.Height / 2.0);
            double lowerBound = -0.2; // angle - 0.1;
            double upperBound = 0.2;  // angle + 0.1;
            Vector2 projectileVector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            projectileVector.Normalize();
            
            int otherID = -1;
            int[] entityList = componentManager.getEntities().Keys.ToArray();
            for (int i = 0; i < entityList.Count(); i++)
            {
                if (entityList[i] == objectEntity.entityID || !componentManager.getEntities()[entityList[i]].isCollide)
                {
                    continue;
                }
                Collide tempCollide = componentManager.getCollides()[componentManager.getEntities()[entityList[i]].entityID];
                if (!tempCollide.isShackleable)
                {
                    continue;
                }
                int tempX = tempCollide.hitbox.X + (int)(tempCollide.hitbox.Width / 2.0);
                int tempY = tempCollide.hitbox.Y + (int)(tempCollide.hitbox.Height / 2.0);
                Vector2 tempVector = new Vector2(tempX - objX, tempY - objY);
                tempVector.Normalize();
                double tempAngle = Math.Acos(Vector2.Dot(projectileVector, tempVector));
                if (lowerBound < tempAngle && tempAngle < upperBound)
                {
                    otherID = tempCollide.entityID;
                    break;
                }
            }
            return otherID;
        }
    }
}

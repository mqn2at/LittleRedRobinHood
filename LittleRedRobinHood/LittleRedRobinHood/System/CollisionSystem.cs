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
        public bool Update(ComponentManager componentManager, GraphicsDevice gd)
        {
            
            int[] entityList = componentManager.getEntities().Keys.ToArray();
            List<int> toBeRemoved = new List<int>();
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

                            //Player - Shackle Collision WILL NEED TO BE IMPROVED
                            //Currently not accounting for shackles as walls
                            else if (componentManager.getEntities()[objectIndex].isShackle)
                            {
                                int firstPointID = componentManager.getShackles()[objectID].firstPointID;
                                int secondPointID = componentManager.getShackles()[objectID].secondPointID;

                                Vector2 topPoint;
                                Vector2 bottomPoint;

                                if(componentManager.getCollides()[firstPointID].hitbox.Y > componentManager.getCollides()[secondPointID].hitbox.Y){
                                    Rectangle topRect = componentManager.getCollides()[secondPointID].hitbox;
                                    Rectangle bottomRect = componentManager.getCollides()[firstPointID].hitbox;
                                    topPoint = new Vector2(topRect.X + (int)(topRect.Width / 2.0), topRect.Y + (int)(topRect.Height / 2.0));
                                    bottomPoint = new Vector2(bottomRect.X + (int)(bottomRect.Width / 2.0), bottomRect.Y + (int)(bottomRect.Height / 2.0));
                                }
                                else
                                {
                                    Rectangle topRect = componentManager.getCollides()[firstPointID].hitbox;
                                    Rectangle bottomRect = componentManager.getCollides()[secondPointID].hitbox;
                                    topPoint = new Vector2(topRect.X + (int)(topRect.Width / 2.0), topRect.Y + (int)(topRect.Height / 2.0));
                                    bottomPoint = new Vector2(bottomRect.X + (int)(bottomRect.Width / 2.0), bottomRect.Y + (int)(bottomRect.Height / 2.0));
                                }
                                //To translate from image coordinates to normal coordinates
                                topPoint.Y = gd.Viewport.Height - topPoint.Y;
                                bottomPoint.Y = gd.Viewport.Height - bottomPoint.Y;

                                double slope = (double)(topPoint.Y - bottomPoint.Y) / (double)(topPoint.X - bottomPoint.X);
                                Rectangle playerCollide = componentManager.getCollides()[playerID].hitbox;
                                int playerMidX = playerCollide.X + (int)(playerCollide.Width / 2.0);
                                Vector2 playerBottom = new Vector2(playerMidX, gd.Viewport.Height - playerCollide.Y);
                                Vector2 playerTop = new Vector2(playerMidX, gd.Viewport.Height - playerCollide.Y + playerCollide.Height);

                                //Vertical shackle platform


                                //Horizontal shackle platform
                                if (Math.Min(topPoint.X, bottomPoint.X) < playerMidX && playerMidX < Math.Max(topPoint.X, bottomPoint.X)
                                    && componentManager.getPlayers()[componentManager.playerID].dy >= 0)
                                {
                                    //Find point on shackle platform that is at the middle of the player's width
                                    double shackleY = slope * (double)(playerMidX - bottomPoint.X) + bottomPoint.Y;
                                    double playerMidY = (double)(playerTop.Y + playerBottom.Y) / 2.0;

                                    Console.WriteLine(shackleY + " | " + playerMidY + " | " + playerBottom.Y);
                                    //Player on top of shackle
                                    if (shackleY < playerMidY)
                                    {
                                        componentManager.getCollides()[playerID].hitbox.Y = (gd.Viewport.Height - (int)shackleY) - playerCollide.Height;
                                        componentManager.getPlayers()[playerID].grounded = true;
                                    }
                                    //Player on bottom of shackle
                                    else if (shackleY > playerMidY)
                                    {
                                        componentManager.getCollides()[playerID].hitbox.Y = gd.Viewport.Height - (int)shackleY;
                                    }
                                }
                            }

                            //Player - Object Collision
                            else if (!componentManager.getEntities()[objectIndex].isShackle && !componentManager.getEntities()[objectIndex].isProjectile)
                            {
                                Dictionary<int, Collide> collideables = componentManager.getCollides();
                                Rectangle playerHitbox = collideables[playerID].hitbox;
                                Rectangle objectHitbox = collideables[objectID].hitbox;
                                //Y-collision
                                //bottom side of object
                                if (playerHitbox.Y < objectHitbox.Y + objectHitbox.Height
                                    && playerHitbox.Y > objectHitbox.Y
                                    && playerHitbox.Y + (int)(0.3 * playerHitbox.Height) > objectHitbox.Y + objectHitbox.Height)
                                {
                                    componentManager.getCollides()[playerID].hitbox.Y = objectHitbox.Y + objectHitbox.Height;
                                }
                                //top side of object
                                else if (playerHitbox.Y + playerHitbox.Height > objectHitbox.Y
                                    && playerHitbox.Y + playerHitbox.Height < objectHitbox.Y + objectHitbox.Height
                                    && playerHitbox.Y + (int)(0.7 * playerHitbox.Height) < objectHitbox.Y)
                                {
                                    componentManager.getPlayers()[playerID].grounded = true;
                                    componentManager.getCollides()[playerID].hitbox.Y = objectHitbox.Y - playerHitbox.Height;
                                }

                                //X-collision
                                //left side of object
                                else if (playerHitbox.X + playerHitbox.Width > objectHitbox.X
                                    && playerHitbox.X + playerHitbox.Width < objectHitbox.X + collideables[objectID].hitbox.Width)
                                {
                                    Console.WriteLine("collide LEFT");
                                    componentManager.getCollides()[playerID].hitbox.X = objectHitbox.X - playerHitbox.Width;
                                }
                                //right side of object
                                else if (playerHitbox.X < objectHitbox.X + objectHitbox.Width
                                    && playerHitbox.X > objectHitbox.X)
                                {
                                    Console.WriteLine("collide RIGHT");
                                    componentManager.getCollides()[playerID].hitbox.X = objectHitbox.X + objectHitbox.Width;
                                }
                            }
                        }

                        //Projectile Collision
                        else if (componentManager.getEntities()[entityList[i]].isProjectile || componentManager.getEntities()[entityList[j]].isProjectile)
                        {
                            int projectileID;
                            Entity objectEntity;
                            int objectIndex = 0;

                            if (componentManager.getEntities()[entityList[i]].isProjectile)
                            {
                                projectileID = componentManager.getEntities()[entityList[i]].entityID;
                                objectEntity = componentManager.getEntities()[entityList[j]];
                                objectIndex = entityList[j];
                            }
                            else
                            {
                                projectileID = componentManager.getEntities()[entityList[j]].entityID;
                                objectEntity = componentManager.getEntities()[entityList[i]];
                                objectIndex = entityList[i];
                            }
                            //Console.WriteLine("Other Object is Shackleable?: " + componentManager.getCollides()[objectEntity.entityID].isShackleable);////

                            //Arrow Collision
                            if (componentManager.getProjectiles()[projectileID].isArrow)
                            {
                                //Arrow - Damageable Enemy Collision NEED TO ADD DAMAGE
                                if (componentManager.getCollides()[objectEntity.entityID].isDamageable)
                                {
                                    toBeRemoved.Add(objectEntity.entityID);
                                }

                                //Arrow - Shackle Platform Collision
                                //COLLISION DETECTION TRACKS HITBOX, NOT ACTUAL SHACKLE
                                else if (objectEntity.isShackle)
                                {
                                    //Detect Collision


                                    //Remove shackle
                                    toBeRemoved.Add(objectEntity.entityID);

                                    //Unshackle objects
                                    int firstUnshackled = componentManager.getShackles()[objectEntity.entityID].firstPointID;
                                    int secondUnshackled = componentManager.getShackles()[objectEntity.entityID].secondPointID;
                                    componentManager.getCollides()[firstUnshackled].isShackled = false;
                                    componentManager.getCollides()[secondUnshackled].isShackled = false;
                                }
                                toBeRemoved.Add(projectileID);
                                componentManager.getPlayers()[componentManager.playerID].arrows += 1;
                            }

                            //Shackle Projectile - Shackleable Collision
                            else if (componentManager.getCollides()[objectEntity.entityID].isShackleable)
                            {
                                Console.WriteLine("HIT SHACKLEABLE OBJECT!");
                                //Check for shackle
                                int otherID = checkShackle(projectileID, objectEntity, componentManager);
                                if (otherID != -1)
                                {
                                    //Set two entities as shackled
                                    componentManager.getCollides()[objectEntity.entityID].isShackled = true;
                                    //componentManager.getCollides()[objectEntity.entityID].isShackleable = false;
                                    componentManager.getCollides()[otherID].isShackled = true;
                                    //componentManager.getCollides()[otherID].isShackleable = false;

                                    //Add Shackle
                                    int newShackleID = componentManager.addEntity();
                                    Rectangle tempRect1 = componentManager.getCollides()[objectEntity.entityID].hitbox;
                                    Rectangle tempRect2 = componentManager.getCollides()[otherID].hitbox;
                                    int rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                                    int rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                                    int rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                                    int rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                                    
                                    componentManager.addShackle(newShackleID, objectEntity.entityID, otherID);
                                    componentManager.addCollide(newShackleID, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);

                                    Console.WriteLine("MADE SHACKLE!!! Number of Shackles: " + componentManager.getShackles().Count);
                                }
                            }
                            toBeRemoved.Add(projectileID);
                            componentManager.getPlayers()[componentManager.playerID].shackles += 1;
                        }
                    }    
                }
            }
            //Remove entities
            foreach (int id in toBeRemoved)
            {
                componentManager.getProjectiles().Remove(id);
                componentManager.getSprites().Remove(id);
                componentManager.getCollides().Remove(id);
                componentManager.getPatrols().Remove(id);
                componentManager.getShackles().Remove(id);
                componentManager.getEntities().Remove(id);
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

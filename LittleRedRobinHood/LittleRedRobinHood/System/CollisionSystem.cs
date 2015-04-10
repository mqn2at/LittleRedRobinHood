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
        private int MAX_SHACKLE_LENGTH = 250;

        //returns an int representing the status -1 : nothing special, 0 : reload the stage,  1 : level beat
        public int Update(ComponentManager manager, GraphicsDevice gd)
        {
            
            int[] entityList = manager.getEntities().Keys.ToArray();
            List<int> toBeRemoved = new List<int>();
            List<int> shackleCollided = new List<int>();
            List<int> arrowCollided = new List<int>();
            bool playerCollided = false;
            bool playerOnPlatform = false;

            for (int i = 0; i < entityList.Count(); i++)
            {
                for (int j = i + 1; j < entityList.Count(); j++)
                {
                    //Check to see if two entities are collidable
                    if (!manager.getEntities()[entityList[i]].isCollide || !manager.getEntities()[entityList[j]].isCollide)
                    {
                        continue;
                    }

                    //Check to see if both entities exist
                    if (toBeRemoved.Contains(manager.getEntities()[entityList[i]].entityID)
                        || toBeRemoved.Contains(manager.getEntities()[entityList[i]].entityID))
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
                            playerCollided = true;

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
                                /////Console.WriteLine("FINISHED STAGE!!!");
                                return 1;
                            }

                            //Player - Textbox
                            else if (manager.getEntities()[objectID].isText)
                            {
                                manager.getTexts()[objectID].visible = true;
                            }

                            //Player - Enemy Collision
                            else if (manager.getCollides()[objectID].isEnemy && manager.getCollides()[objectID].numShackled == 0)
                            {
                                manager.getPlayers()[playerID].lives--;
                                return 0;
                            }

                            //Player - Shackle Collision WILL NEED TO BE IMPROVED
                            //Currently not accounting for shackles as walls
                            //BUG, bottom get pushed to top for some reason
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
                                    double playerMidY = (double)(playerBottom.Y - playerCollide.Height/2.0);

                                    /////Console.WriteLine(shackleY + " | " + playerMidY + " | " + playerBottom.Y);
                                    //Player on top of shackle
                                    if (shackleY < playerMidY && shackleY+playerCollide.Height/2.0 > playerMidY)
                                    {
                                        /////Console.WriteLine("PLAYER ON TOP OF SHACKLE!");
                                        manager.getCollides()[playerID].hitbox.Y = (gd.Viewport.Height - (int)shackleY) - playerCollide.Height;
                                        manager.getPlayers()[playerID].grounded = true;
                                        manager.getPlayers()[playerID].jumping = false;
                                    }
                                    /*
                                    //Player on bottom of shackle
                                    else if (shackleY > playerMidY)
                                    {
                                        Console.WriteLine("PLAYER BELOW SHACKLE!");
                                        manager.getCollides()[playerID].hitbox.Y = Math.Max(manager.getCollides()[playerID].hitbox.Y, 
                                            gd.Viewport.Height - (int)shackleY);
                                    }
                                     * */
                                }
                            }

                            //Player - Object Collision
                            //FIX SOME BUG THAT CAUSES PLAYER TO PUSH OFF OF LEFT + 
                            //
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
                                    //Ground if not jumping
                                    if (manager.getPlayers()[manager.playerID].dy >= 0)
                                    {
                                        manager.getPlayers()[playerID].grounded = true;
                                        manager.getPlayers()[playerID].jumping = false;
                                    }
                                    manager.getCollides()[playerID].hitbox.Y = objectHitbox.Y - playerHitbox.Height;

                                    //If platform is moving, have player move with
                                    if (manager.getEntities()[objectID].isPatrol && manager.getPatrols()[objectID].dx != null 
                                        && manager.getPatrols()[objectID].dy != null)
                                    {
                                        int dx = (int)manager.getPatrols()[objectID].curr_dx;
                                        int dy = (int)manager.getPatrols()[objectID].curr_dy;
                                        manager.getCollides()[playerID].hitbox.X += (int)(dx * 2);

                                        //manager.getCollides()[playerID].hitbox.Y += dy*2;
                                    }
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
                            /////Console.WriteLine("Other Object is Shackleable?: " + componentManager.getCollides()[objectEntity.entityID].isShackleable);////

                            //Arrow Collision
                            if (manager.getProjectiles()[projectileID].isArrow)
                            {
                                if (manager.getCollides()[objectEntity.entityID].numShackled > 0)
                                {
                                    //Remove shackles linked to enemy
                                    foreach (int shackleID in manager.getShackles().Keys)
                                    {
                                        
                                        int shackledID1 = manager.getShackles()[shackleID].firstPointID;
                                        int shackledID2 = manager.getShackles()[shackleID].secondPointID;

                                        if (objectEntity.entityID == shackledID1 || objectEntity.entityID == shackledID2)
                                        {
                                            //Remove shackle
                                            toBeRemoved.Add(shackleID);
                                            
                                            //Make player fall if player on shackle
                                            if (manager.getCollides()[shackleID].hitbox.Intersects(manager.getCollides()[manager.playerID].hitbox))
                                            {
                                                manager.getPlayers()[manager.playerID].grounded = false;
                                            }

                                            //Unshackle objects
                                            int firstUnshackled = manager.getShackles()[shackleID].firstPointID;
                                            int secondUnshackled = manager.getShackles()[shackleID].secondPointID;
                                            manager.getCollides()[firstUnshackled].numShackled--;
                                            manager.getCollides()[secondUnshackled].numShackled--;
                                            if (manager.getShackles()[shackleID].playerMade)
                                            {
                                                manager.getPlayers()[manager.playerID].shackles += 1;
                                            }
                                        }
                                    }

                                    //Remove arrow
                                    toBeRemoved.Add(projectileID);
                                    if (!arrowCollided.Contains(projectileID))
                                    {
                                        manager.getPlayers()[manager.playerID].arrows += 1;
                                        arrowCollided.Add(projectileID);
                                    }
                                }
                                
                                //Arrow - Damageable Enemy Collision
                                else if (manager.getCollides()[objectEntity.entityID].isDamageable)
                                {
                                    /////Console.WriteLine("ARROW HIT ENEMY!"); ////
                                    //Remove enemy
                                    toBeRemoved.Add(objectEntity.entityID);

                                    //Remove arrow
                                    toBeRemoved.Add(projectileID);
                                    if (!arrowCollided.Contains(projectileID))
                                    {
                                        manager.getPlayers()[manager.playerID].arrows += 1;
                                        arrowCollided.Add(projectileID);
                                    }
                                }

                                //Arrow - Shackle Platform Collision
                                else if (objectEntity.isShackle)
                                {
                                    /////Console.WriteLine("ARROW HIT SHACKLE HITBOX!"); ////

                                    //Detect Collision
                                    int spd = manager.getProjectiles()[projectileID].speed;
                                    double ang = manager.getProjectiles()[projectileID].angle;
                                    Vector2 currArrLoc = new Vector2(manager.getCollides()[projectileID].hitbox.X,
                                        manager.getCollides()[projectileID].hitbox.Y);
                                    Vector2 prevArrLoc = new Vector2(currArrLoc.X - (int)(2 * spd * Math.Cos(ang)),
                                        currArrLoc.Y - (int)(2 * spd * Math.Sin(ang)));
                                    int firstPtID = manager.getShackles()[objectEntity.entityID].firstPointID;
                                    int secondPtID = manager.getShackles()[objectEntity.entityID].secondPointID;
                                    Rectangle firstPtBox = manager.getCollides()[firstPtID].hitbox;
                                    Rectangle secondPtBox = manager.getCollides()[secondPtID].hitbox;
                                    Vector2 firstPt = new Vector2(firstPtBox.X + firstPtBox.Width / 2, firstPtBox.Y + firstPtBox.Height / 2);
                                    Vector2 secondPt = new Vector2(secondPtBox.X + secondPtBox.Width / 2, secondPtBox.Y + secondPtBox.Height / 2);

                                    //If the sweep collision happens
                                    if (sweepCollision(prevArrLoc, currArrLoc, firstPt, secondPt))
                                    {
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
                                        if (!shackleCollided.Contains(projectileID) && manager.getShackles()[objectEntity.entityID].playerMade)
                                        {
                                            manager.getPlayers()[manager.playerID].shackles += 1;
                                            shackleCollided.Add(projectileID);
                                        }

                                        //Remove arrow
                                        toBeRemoved.Add(projectileID);
                                        if (!arrowCollided.Contains(projectileID))
                                        {
                                            manager.getPlayers()[manager.playerID].arrows += 1;
                                            arrowCollided.Add(projectileID);
                                        }
                                        /////Console.WriteLine("ARROW HIT SHACKLE!"); ////
                                    }
                                }
                                
                                //Arrow - other collideable Collision
                                else if (!manager.getEntities()[objectEntity.entityID].isText)
                                {
                                    /////Console.WriteLine("ARROW HIT SOMETHING ELSE!"); ////

                                    //Remove arrow
                                    toBeRemoved.Add(projectileID);
                                    if (!arrowCollided.Contains(projectileID))
                                    {
                                        manager.getPlayers()[manager.playerID].arrows += 1;
                                        arrowCollided.Add(projectileID);
                                    }
                                }
                            }

                            //Shackle Projectile
                            else {
                                //Shackle Projectile - Shackleable Collision
                                if (manager.getCollides()[objectEntity.entityID].isShackleable)
                                {
                                    //Check for shackle
                                    int otherID = checkShackle(projectileID, objectEntity, manager);
                                    //Check shackle length
                                    if(otherID != -1){
                                        Rectangle objCollide = manager.getCollides()[objectEntity.entityID].hitbox;
                                        Rectangle otherCollide = manager.getCollides()[otherID].hitbox;
                                        Vector2 objCoord = new Vector2(objCollide.X, objCollide.Y);
                                        Vector2 otherCoord = new Vector2(otherCollide.X, otherCollide.Y);

                                        float dist = Vector2.Distance(objCoord, otherCoord);
                                        if (dist > this.MAX_SHACKLE_LENGTH)
                                        {
                                            otherID = -1;
                                        }
                                    }
                                    if (otherID != -1)
                                    {
                                        //Check to see if shackle already exists
                                        bool shackleExists = false;
                                        foreach (Shackle shackle in manager.getShackles().Values)
                                        {
                                            if((shackle.firstPointID == objectEntity.entityID && shackle.secondPointID == otherID)
                                                || (shackle.firstPointID == otherID && shackle.secondPointID == objectEntity.entityID)){
                                                    shackleExists = true;
                                                    if (!shackleCollided.Contains(projectileID))
                                                    {
                                                        manager.getPlayers()[manager.playerID].shackles += 1;
                                                        shackleCollided.Add(projectileID);
                                                    }
                                                    /////Console.WriteLine("Shackle already exists!");
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

                                            //Check number of shackles
                                            if (shackleCollided.Contains(projectileID))
                                            {
                                                manager.getPlayers()[manager.playerID].shackles -= 1;
                                            }
                                        }                                        
                                    }
                                    //Replenish shackle if enemy not shackled
                                    else if (!shackleCollided.Contains(projectileID))
                                    {
                                        manager.getPlayers()[manager.playerID].shackles += 1;
                                        shackleCollided.Add(projectileID);
                                    }
                                    //remove Shackle
                                    toBeRemoved.Add(projectileID);                                    
                                }
                                //Shackle Projectile - shackle platform
                                else if (objectEntity.isShackle)
                                {
                                    /////Console.WriteLine("SHACKLE HIT SHACKLE HITBOX!"); ////

                                    //Detect Collision
                                    int spd = manager.getProjectiles()[projectileID].speed;
                                    double ang = manager.getProjectiles()[projectileID].angle;
                                    Vector2 currShackleLoc = new Vector2(manager.getCollides()[projectileID].hitbox.X,
                                        manager.getCollides()[projectileID].hitbox.Y);
                                    Vector2 prevShackleLoc = new Vector2(currShackleLoc.X - (int)(spd * Math.Cos(ang)),
                                        currShackleLoc.Y - (int)(spd * Math.Sin(ang)));
                                    int firstPtID = manager.getShackles()[objectEntity.entityID].firstPointID;
                                    int secondPtID = manager.getShackles()[objectEntity.entityID].secondPointID;
                                    Rectangle firstPtBox = manager.getCollides()[firstPtID].hitbox;
                                    Rectangle secondPtBox = manager.getCollides()[secondPtID].hitbox;
                                    Vector2 firstPt = new Vector2(firstPtBox.X + firstPtBox.Width / 2, firstPtBox.Y + firstPtBox.Height / 2);
                                    Vector2 secondPt = new Vector2(secondPtBox.X + secondPtBox.Width / 2, secondPtBox.Y + secondPtBox.Height / 2);

                                    //If the sweep collision happens
                                    if (sweepCollision(prevShackleLoc, currShackleLoc, firstPt, secondPt))
                                    {
                                        //Remove shackle
                                        toBeRemoved.Add(projectileID);
                                        if (!shackleCollided.Contains(projectileID))
                                        {
                                            manager.getPlayers()[manager.playerID].shackles += 1;
                                            shackleCollided.Add(projectileID);
                                        }
                                    }
                                } 
                                    
                                //Shackle Projectile - random collideable
                                else if (!shackleCollided.Contains(projectileID) && !manager.getEntities()[objectEntity.entityID].isText)
                                {
                                    manager.getPlayers()[manager.playerID].shackles += 1;
                                    shackleCollided.Add(projectileID);
                                    //remove Shackle
                                    toBeRemoved.Add(projectileID);
                                    /////Console.WriteLine("No Shackle Made.");
                                }
                                /////Console.WriteLine("Number of shackle platforms: " + manager.getShackles().Count);
                            }
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

            //Check to see if player is out of bounds
            //X-axis - Prevent player from leaving screen X-wise
            Rectangle playerHB = manager.getCollides()[manager.playerID].hitbox;
            if (playerHB.X < 0)
            {
                manager.getCollides()[manager.playerID].hitbox.X = 0;
            }
            if (playerHB.X + playerHB.Width > gd.Viewport.Width)
            {
                manager.getCollides()[manager.playerID].hitbox.X = gd.Viewport.Width - playerHB.Width;
            }

            //Y-axis
            if (playerHB.Y > gd.Viewport.Height)
            {
                manager.getPlayers()[manager.playerID].lives--;
                return 0;
            }

            if (!playerCollided && manager.getPlayers()[manager.playerID].dy >= 0)
            {
                manager.getPlayers()[manager.playerID].grounded = false;
            }

            return -1;
        }


        //Method to check if objects are in a line to be shackled
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


        /*
         * METHODS FOR CHECKING SWEEPING COLLISION
         * Source: http://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
         */

        //Checks if point b is on line segment ac
        private bool onSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            return (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y));
        }

        //Find orientation of 3 points: a,b,c
        //0 = colinear, 1 = clockwise, 2 = counter clockwise
        private int orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0)
            {
                return 0;
            }
            return (val > 0) ? 1 : 2;
        }

        //Method to check if line segments intersect for sweep collision
        private bool sweepCollision(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
        {
            // Find the four orientations needed for general and
            // special cases
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and p2 are colinear and q2 lies on segment p1q1
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases
        }
    }
}

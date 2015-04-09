using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.System
{
    class PathingSystem
    {
        public void Update(ComponentManager componentManager)
        {
            Dictionary<int, LittleRedRobinHood.Entities.Entity> entities = componentManager.getEntities();
            int[] entityList = componentManager.getEntities().Keys.ToArray();
            for (int i = 0; i < entityList.Count(); i++)
            {
                if (entities[entityList[i]].isPatrol)
                {
                    //Find current point and destination point
                    int entityID = entities[entityList[i]].entityID;
                    if (componentManager.getCollides()[entityID].numShackled == 0)
                    {
                        List<Vector2> path = componentManager.getPatrols()[entityID].waypoint;
                        Rectangle rectangle = componentManager.getCollides()[entityID].hitbox;
                        Vector2 currentPos = new Vector2((float)(rectangle.X + (0.5) * rectangle.Width),
                            (float)(rectangle.Y + (0.5) * rectangle.Height));
                        if (path.Count != 0)
                        {
                            Vector2 destPos = path[componentManager.getPatrols()[entityID].currentDest];
                                                
                            //Calculate angle
                            double height = (double)(currentPos.Y - destPos.Y);
                            double width = (double)(currentPos.X - destPos.X);
                            double hypotenuse = Math.Sqrt(Math.Pow(height, 2) + Math.Pow(width, 2));
                            double angle = Math.Atan2(height, width);
                            //Move patrolling unit
                            int speed = componentManager.getPatrols()[entityID].speed;

                            int dX = (int)(speed * Math.Cos(angle)) * -1;
                            int dY = (int)(speed * Math.Sin(angle)) * -1;

                            //For Sprite Animations
                            if (dX <= 0)
                            {
                                componentManager.getPatrols()[entityID].is_right = false;
                            }
                            else
                            {
                                componentManager.getPatrols()[entityID].is_right = true;
                            }

                            int x = componentManager.getCollides()[entityID].hitbox.X + dX;
                            int y = componentManager.getCollides()[entityID].hitbox.Y + dY;


                            int dest = componentManager.getPatrols()[entityID].currentDest;
                            //Console.WriteLine(dest);
                            //check if overshot assume 0 is the spawn point
                            if ((dX > 0 && destPos.X < x) || (dX < 0 && destPos.X > x) || (dY > 0 && destPos.Y < y) || (dY < 0 && destPos.Y > y) || (x==destPos.X && y==destPos.Y))
                            {

                                /*componentManager.getCollides()[entityID].hitbox.X = (int)destPos.X;
                                componentManager.getCollides()[entityID].hitbox.Y = (int)destPos.Y;*/

                                if (path.Count - 1 == componentManager.getPatrols()[entityID].currentDest)
                                {
                                    if (componentManager.getPatrols()[entityID].isCyclical)
                                    {

                                        componentManager.getPatrols()[entityID].setCurrentDest(0);
                                        ///componentManager.getPatrols()[entityID].prevLoc = new Vector2(componentManager.getCollides()[entityID].hitbox.X, componentManager.getCollides()[entityID].hitbox.Y);
                                        componentManager.getCollides()[entityID].hitbox.X = x;
                                        componentManager.getCollides()[entityID].hitbox.Y = y;
                                        
                                    }
                                    else
                                    {
                                        //Console.WriteLine("asdfsadf");
                                        Vector2 destCyclical = path[componentManager.getPatrols()[entityID].currentDest-1];
                                        ///componentManager.getPatrols()[entityID].prevLoc = new Vector2(componentManager.getCollides()[entityID].hitbox.X, componentManager.getCollides()[entityID].hitbox.Y);
                                        componentManager.getCollides()[entityID].hitbox.X = (int)destCyclical.X;
                                        componentManager.getCollides()[entityID].hitbox.Y = (int)destCyclical.Y;
                                    }
                                }
                                else
                                {
                                    dest = dest + 1;
                                    componentManager.getPatrols()[entityID].setCurrentDest(dest);
                                    ///componentManager.getPatrols()[entityID].prevLoc = new Vector2(componentManager.getCollides()[entityID].hitbox.X, componentManager.getCollides()[entityID].hitbox.Y);
                                    componentManager.getCollides()[entityID].hitbox.X = x;
                                    componentManager.getCollides()[entityID].hitbox.Y = y;
                                    //destPos = path[componentManager.getPatrols()[entityID].currentDest];
                                }
                            }
                            else
                            {
                                ///componentManager.getPatrols()[entityID].prevLoc = new Vector2(componentManager.getCollides()[entityID].hitbox.X, componentManager.getCollides()[entityID].hitbox.Y);
                                componentManager.getCollides()[entityID].hitbox.X = x;
                                componentManager.getCollides()[entityID].hitbox.Y = y;
                                //Console.WriteLine("currentdest"+componentManager.getPatrols()[entityID].currentDest);
                                //Console.WriteLine("dx:" + (destPos.X-x)+","+dX + "dy:" + (destPos.Y-dY)+","+dY );
                            }
                            /*else if ((dY > 0 && destPos.Y < y) || (dY < 0 && destPos.Y > y))
                            {

                                componentManager.getCollides()[entityID].hitbox.X = (int)destPos.X;
                                componentManager.getCollides()[entityID].hitbox.Y = (int)destPos.Y;
                                if (path.Count - 1 == componentManager.getPatrols()[entityID].currentDest)
                                {
                                    componentManager.getPatrols()[entityID].setCurrentDest(0);
                                }
                                else
                                {

                                    dest = dest + 1;

                                    componentManager.getPatrols()[entityID].setCurrentDest(dest);

                                }
                            }*/

                        }
                    }
                }
                else if (entities[entityList[i]].isHoming)
                {

                }
            }
        }
    }
}

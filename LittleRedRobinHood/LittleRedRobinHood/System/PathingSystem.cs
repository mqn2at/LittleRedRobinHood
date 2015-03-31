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

                            int x = componentManager.getCollides()[entityID].hitbox.X + dX;
                            int y = componentManager.getCollides()[entityID].hitbox.Y + dY;


                            int dest = componentManager.getPatrols()[entityID].currentDest;
                            //Console.WriteLine(dest);
                            //check if overshot
                            if ((dX > 0 && destPos.X < x) || (dX < 0 && destPos.X > x) && (dY > 0 && destPos.Y < y) || (dY < 0 && destPos.Y > y))
                            {

                                /*componentManager.getCollides()[entityID].hitbox.X = (int)destPos.X;
                                componentManager.getCollides()[entityID].hitbox.Y = (int)destPos.Y;*/

                                if (path.Count - 1 == componentManager.getPatrols()[entityID].currentDest)
                                {

                                    componentManager.getPatrols()[entityID].setCurrentDest(0);
                                }
                                else
                                {
                                    dest = dest + 1;

                                    componentManager.getPatrols()[entityID].setCurrentDest(dest);

                                    //destPos = path[componentManager.getPatrols()[entityID].currentDest];

                                }
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
                            

                            componentManager.getCollides()[entityID].hitbox.X = x;
                            componentManager.getCollides()[entityID].hitbox.Y = y;
                            

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

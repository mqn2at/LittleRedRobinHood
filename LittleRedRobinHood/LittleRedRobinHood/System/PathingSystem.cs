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
            for (int i = 0; i < componentManager.getEntities().Count; i++)
            {
                if (componentManager.getEntities()[i].isPatrol)
                {
                    //Find current point and destination point
                    int entityID = componentManager.getEntities()[i].entityID;
                    if (!componentManager.getCollides()[entityID].isShackled)
                    {
                        List<Vector2> path = componentManager.getPatrols()[entityID].waypoint;
                        Rectangle rectangle = componentManager.getCollides()[entityID].hitbox;
                        Vector2 currentPos = new Vector2(rectangle.X + (1 / 2) * rectangle.Width,
                            rectangle.Y + (1 / 2) * rectangle.Height);
                        Vector2 destPos = path[componentManager.getPatrols()[entityID].currentDest];

                        //Calculate angle
                        double height = (double)(currentPos.Y - destPos.Y);
                        double width = (double)(currentPos.X - destPos.X);
                        double hypotenuse = Math.Sqrt(Math.Pow(height, 2) + Math.Pow(width, 2));
                        double angle = Math.Asin(width / hypotenuse);
                        //Move patrolling unit
                        int speed = componentManager.getPatrols()[entityID].speed;

                        int dX = (int)(speed * Math.Cos(angle));
                        int dY = (int)(speed * Math.Sin(angle));
                        if (destPos.X < currentPos.X)
                        {
                            dX *= -1;
                        }
                        if (destPos.Y < currentPos.Y)
                        {
                            dY *= -1;
                        }
                        int x = componentManager.getCollides()[entityID].hitbox.X + dX;
                        int y = componentManager.getCollides()[entityID].hitbox.Y + dY;
                        //check if overshot
                        if ((dX > 0 && destPos.X < x) || (dX < 0 && destPos.X > x))
                        {
                            componentManager.getCollides()[entityID].hitbox.X = x;
                            componentManager.getCollides()[entityID].hitbox.Y = y;
                            componentManager.getPatrols()[entityID].currentDest++;
                        }
                        else if ((dY > 0 && destPos.Y < y) || (dY < 0 && destPos.Y > y))
                        {
                            componentManager.getCollides()[entityID].hitbox.X = x;
                            componentManager.getCollides()[entityID].hitbox.Y = y;
                            componentManager.getPatrols()[entityID].currentDest++;
                        }
                    }
                }
                else if (componentManager.getEntities()[i].isHoming){

                }
            }
        }
    }
}

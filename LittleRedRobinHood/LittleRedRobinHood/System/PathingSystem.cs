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
                        Vector2 currentPos = new Vector2(componentManager.getCollides()[entityID].hitbox.X,
                            componentManager.getCollides()[entityID].hitbox.Y);
                        Vector2 destPos = path[componentManager.getPatrols()[entityID].currentDest];

                        //Calculate angle

                        //Move patrolling unit
                    }
                }
                else if (componentManager.getEntities()[i].isHoming){

                }
            }
        }
    }
}

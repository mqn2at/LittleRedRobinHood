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
        public void collide(List<Entity> entities, Dictionary<int,Collide> collideables, Dictionary<int,Player> player, Dictionary<int,Shackle> shackles)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    //Check to see if two entities are collidable
                    if (!entities[i].isCollide || !entities[j].isCollide)
                    {
                        break;
                    }

                    Collide collide1 = collideables[entities[i].entityID];
                    Collide collide2 = collideables[entities[j].entityID];
                    if (collide1.hitbox.Intersects(collide2.hitbox))
                    {
                        //Check if one entity is player and other is enemy
                        if((entities[i].isPlayer && collide2.isEnemy) || (entities[j].isPlayer && collide1.isEnemy)){
                            //Player loses health
                            if(entities[i].isPlayer){
                                player[entities[i].entityID].health--;
                            }
                            if (entities[j].isPlayer)
                            {
                                player[entities[j].entityID].health--;
                            }
                        }

                        //Check if one entity is player and other player is generic collideable
                        else if ((entities[i].isPlayer && !entities[j].isShackle) || (entities[j].isPlayer && !entities[i].isShackle))
                        {
                            int playerID = 0;
                            int objectID = 0;

                            if (entities[i].isPlayer)
                            {
                                playerID = entities[i].entityID;
                                objectID = entities[j].entityID;
                            }
                            else
                            {
                                playerID = entities[j].entityID;
                                objectID = entities[i].entityID;
                            }

                            //X-collision
                            //left side
                            if (collideables[playerID].hitbox.X < collideables[objectID].hitbox.X)
                            {
                                collideables[playerID].hitbox.X = collideables[objectID].hitbox.X - collideables[playerID].hitbox.Width;
                            }

                            //Y-collision

                        }
                    }
                }
            }                
        }
    }
}

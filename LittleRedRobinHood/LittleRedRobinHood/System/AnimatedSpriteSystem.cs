using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LittleRedRobinHood.Component;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;

namespace LittleRedRobinHood.System
{
    class AnimatedSpriteSystem
    {
        //Sprite Animation Stuff
        float timePlayer;
        float timePatrol;
        const float frameTime = 0.1f;

        public bool running = false;
        public bool idle = true;
        public bool shooting = false;

        private int playerCurrentFrame = 0;
        private int playerTotalFrame = 10;
        private int patrolCurrentFrame = 0;
        private int patrolTotalFrame = 3;
        private int spriteSpeed = 10;
        public List<Vector2> flyingCoords;
        //End Sprite Animation

        public AnimatedSpriteSystem()
        {

            //Patrol enemy spritesheed coordinates
            flyingCoords = new List<Vector2>();
            flyingCoords.Add(new Vector2(0, 0));
            flyingCoords.Add(new Vector2(1, 0));
            flyingCoords.Add(new Vector2(2, 0));
        }

        public void Draw(SpriteBatch sb, ComponentManager cm, GameTime gameTime)
        {
            //draw UI
            this.drawUI(sb, cm);
            //draw everything else
            Dictionary<int, LittleRedRobinHood.Component.Collide> collides = cm.getCollides();
            foreach (KeyValuePair<int, LittleRedRobinHood.Component.Sprite> sp in cm.getSprites())
            {
                int spriteX = cm.getCollides()[sp.Value.entityID].hitbox.X;
                int spriteY = cm.getCollides()[sp.Value.entityID].hitbox.Y;

                Texture2D image = cm.getSprites()[sp.Value.entityID].sprite;
                int spriteWidth = cm.getSprites()[sp.Value.entityID].width;
                int spriteHeight = cm.getSprites()[sp.Value.entityID].height;
                SpriteEffects effect = SpriteEffects.None;
                //check if sprite is an animated sprite
                if (sp.Value.animated)
                {
                    int column = 0;
                    float row = 0;
                    
                    //Player animation
                    if (cm.getEntities()[sp.Value.entityID].isPlayer)
                    {
                       
                        //Check which way player is facing
                        if (cm.getPlayers()[sp.Value.entityID].is_right)
                        {
                            effect = SpriteEffects.None;
                        }
                        else
                        {
                            effect = SpriteEffects.FlipHorizontally;
                        }

                        //shooting??
                        if (cm.getPlayers()[sp.Value.entityID].shooting)
                        {
                            spriteWidth = 73;
                            spriteHeight = 58;
                            column = playerCurrentFrame;
                            playerTotalFrame = 5;
                            row = 3;
                            spriteSpeed = 5;
                        }
                        //jumping
                        else if (cm.getPlayers()[sp.Value.entityID].jumping)
                        {
                            spriteWidth = 36;
                            spriteHeight = 62; //wonky
                            if (cm.getPlayers()[sp.Value.entityID].dy < 0)
                            {
                                playerTotalFrame = 2;
                                column = playerCurrentFrame;
                                row = 3.75f;
                            }
                            //falling
                            else if (cm.getPlayers()[sp.Value.entityID].dy >= 0)
                            {
                                playerTotalFrame = 1;
                                column = 2;
                                row = 3.75f;
                            }
                        }
                        //falling
                        else if (cm.getPlayers()[sp.Value.entityID].dy > 6)
                        {
                            spriteWidth = 36;
                            spriteHeight = 62;
                            playerTotalFrame = 1;
                            column = 2;
                            row = 3.75f; //because height is not 58 like everything else
                        }
                        
                        //running
                        else if (cm.getPlayers()[sp.Value.entityID].running)
                        {
                            spriteWidth = 46;
                            spriteHeight = 58;
                            column = playerCurrentFrame;
                            playerTotalFrame = 10;
                            row = 1;
                        }
                        //idle
                        else
                        {
                            spriteWidth = 36;
                            spriteHeight = 58;
                            column = playerCurrentFrame;
                            playerTotalFrame = 10;
                            row = 0;
                        }
                        //grab the current animation frame
                        Rectangle sourceRectangle = new Rectangle(spriteWidth * column, (int)(spriteHeight * row), spriteWidth, spriteHeight);
                        Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
                        //Reset the collide hitbox size in case it changes (which it does with the Player)
                        spriteWidth = 36; //NOT SURE ABOUT THIS OK but it works fine so that's good
                        spriteHeight = 58;
                        /*cm.getCollides()[sp.Value.entityID].hitbox = destinationRectangle;
                        //Make it so that shooting sprite isn't really off
                        if (cm.getPlayers()[sp.Value.entityID].shooting && !cm.getPlayers()[sp.Value.entityID].is_right)
                        {
                            destinationRectangle.X = destinationRectangle.X - (73-36);
                        }*/
                        //draw the current animation frame
                        sb.Draw(image, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), effect, 1);

                        //update the current frames
                        timePlayer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        while (timePlayer > frameTime)
                        {
                            // Play the next frame in the SpriteSheet
                            playerCurrentFrame++;
                            // reset elapsed time
                            timePlayer = 0f;
                        }
                        if (playerCurrentFrame >= playerTotalFrame)
                        {
                            playerCurrentFrame = 0;
                            if (cm.getPlayers()[sp.Value.entityID].shooting)
                                playerCurrentFrame = 3;
                        }
                    }
                    //Patrol animation
                    else if (cm.getEntities()[sp.Value.entityID].isPatrol)
                    {
                        row = 0;
                        column = 0;
                        //Check which way patrol is facing
                        if (cm.getPatrols()[sp.Value.entityID].is_right)
                        {
                            effect = SpriteEffects.None;
                        }
                        else
                        {
                            effect = SpriteEffects.FlipHorizontally;
                        }
                        //flying patrols
                        //check whether or not they are shackled/frozen
                        if (cm.getCollides()[sp.Value.entityID].numShackled == 0)
                        {
                            column = patrolCurrentFrame;
                            row = 0;
                        }
                        //grab the current animation frame
                        Rectangle sourceRectangle = new Rectangle(spriteWidth * column, (int)(spriteHeight * row), spriteWidth, spriteHeight);
                        Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
                        //draw the current animation frame
                        sb.Draw(image, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), effect, 1);

                        //update the current frames
                        timePatrol += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        while (timePatrol > frameTime)
                        {
                            // Play the next frame in the SpriteSheet
                            patrolCurrentFrame++;
                            // reset elapsed time
                            timePatrol = 0f;
                        }
                        //check out of bounds
                        if (patrolCurrentFrame >= patrolTotalFrame)
                        {
                            patrolCurrentFrame = 0;
                        }
                    }
                }
                else //no animation, so just draw
                {
                    if (cm.getEntities()[sp.Value.entityID].isProjectile)
                    {
                        sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), null, Color.White, (float)cm.getProjectiles()[sp.Value.entityID].angle+(float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 1);
                    }
                    else
                    {
                        sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
                    }
                }
            }
        }

        public void drawUI(SpriteBatch sb, ComponentManager cm)
        {
            Player pl = cm.getPlayers()[cm.playerID];
            Rectangle hB = cm.getHealthBox();
            Rectangle aB = cm.getArrowsBox();
            Rectangle shB = cm.getShackleBox();
            Texture2D hS = cm.getHealthSprite();
            Texture2D aS = cm.getArrowSprite();
            Texture2D shS = cm.getShackleSprite();
            for (int x = 0; x < pl.lives; x++)
            {
                sb.Draw(hS, new Rectangle(hB.X + (x * 30), hB.Y, hB.Width, hB.Height), Color.White);
            }
            for (int x = 0; x < pl.arrows; x++)
            {
                sb.Draw(aS, new Rectangle(aB.X + (x * 30), aB.Y, aB.Width, aB.Height), Color.White);
            }
            for (int x = 0; x < pl.shackles; x++)
            {
                sb.Draw(shS, new Rectangle(shB.X + (x * 30), shB.Y, shB.Width, shB.Height), Color.White);
            }
        }
    }
}
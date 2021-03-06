﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        float timeSparkle;
        const float sparkleFrameTime = 0.2f;
        const float frameTime = 0.1f;

        public bool running = false;
        public bool idle = true;
        public bool shooting = false;

        private int playerCurrentFrame = 0;
        private int playerTotalFrame = 10;
        private int patrolCurrentFrame = 0;
        private int patrolTotalFrame = 3;
        private int sparkleCurrentFrame = 0;
        private int sparkleTotalFrame = 12;

        private Texture2D sparkleImage;
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

        public void LoadContent(ContentManager cm)
        {
            //load shacklable sparkles as they are not part of any other sprite
            sparkleImage = cm.Load<Texture2D>("sparkle.png");
        }

        public void Draw(SpriteBatch sb, ComponentManager cm, GameTime gameTime)
        {
            //draw UI
            this.drawUI(sb, cm);
            //draw everything else
            Dictionary<int, LittleRedRobinHood.Component.Collide> collides = cm.getCollides();
            SpriteEffects effect = SpriteEffects.None;

            
            foreach (KeyValuePair<int, LittleRedRobinHood.Component.Sprite> sp in cm.getSprites())
            {
                int spriteX = cm.getCollides()[sp.Value.entityID].hitbox.X;
                int spriteY = cm.getCollides()[sp.Value.entityID].hitbox.Y;

                Texture2D image = cm.getSprites()[sp.Value.entityID].sprite;
                int spriteWidth = cm.getSprites()[sp.Value.entityID].width;
                int spriteHeight = cm.getSprites()[sp.Value.entityID].height;
                //check if sprite is an animated sprite
                if (sp.Value.animated)
                {
                    int column = 0;
                    float row = 0;
                    //Patrol animation
                    if (cm.getEntities()[sp.Value.entityID].isPatrol)
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
                    //Rotate if projectile BUT NOT PINECONE
                    if (cm.getEntities()[sp.Value.entityID].isProjectile && cm.getProjectiles()[sp.Value.entityID].isArrow && !cm.getEntities()[sp.Value.entityID].isPatrol)
                    {
                        sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), null, Color.White, (float)cm.getProjectiles()[sp.Value.entityID].angle+(float)Math.PI, new Vector2(spriteWidth / 2, spriteHeight / 2), SpriteEffects.None, 1);
                    }
                    else
                    {
                        sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
                    }
                }
            }
            
        }

        public void DrawSparkles(SpriteBatch sb, ComponentManager cm, GameTime gameTime)
        {
            Dictionary<int, LittleRedRobinHood.Component.Collide> collides = cm.getCollides();
            SpriteEffects effect = SpriteEffects.None;
            //draw sparkles
            foreach (KeyValuePair<int, Collide> col in collides)
            {
                if (col.Value.isShackleable)
                {
                    //Console.WriteLine("Col: " + (sparkleCurrentFrame % 8) + "\tRow: " + (sparkleCurrentFrame / 8));

                    //grab the current animation frame
                    //Rectangle sourceRectangle = new Rectangle(32 * ((sparkleCurrentFrame % 8)), (int)(32 * (sparkleCurrentFrame / 8)), 32, 32);
                    Rectangle sourceRectangle = new Rectangle(76 * sparkleCurrentFrame, 0, 76, 76);
                    Rectangle destRectangle = new Rectangle(col.Value.hitbox.X-5, col.Value.hitbox.Y-5, col.Value.hitbox.Width+10, col.Value.hitbox.Height+10);
                    //draw the current animation frame
                    sb.Draw(sparkleImage, destRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), effect, 1);

                    //update the current frames
                    timeSparkle += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    while (timeSparkle > sparkleFrameTime)
                    {
                        // Play the next frame in the SpriteSheet
                        sparkleCurrentFrame++;
                        // reset elapsed time
                        timeSparkle = 0f;
                    }
                    if (sparkleCurrentFrame >= sparkleTotalFrame)
                    {
                        sparkleCurrentFrame = 0;
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

        public void DrawPlayer(SpriteBatch sb, ComponentManager cm, GameTime gameTime)
        {
            SpriteEffects effect = SpriteEffects.None;
            int column = 0;
            float row = 0;
            int spriteX = cm.getCollides()[cm.playerID].hitbox.X;
            int spriteY = cm.getCollides()[cm.playerID].hitbox.Y;
            Texture2D image = cm.getSprites()[cm.playerID].sprite;
            int spriteWidth = cm.getSprites()[cm.playerID].width;
            int spriteHeight = cm.getSprites()[cm.playerID].height;
            if (cm.getPlayers()[cm.playerID].is_right)
            {
                effect = SpriteEffects.None;
            }
            else
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            //shooting??
            if (cm.getPlayers()[cm.playerID].shooting)
            {
                spriteWidth = 73;
                spriteHeight = 58;
                column = playerCurrentFrame;
                playerTotalFrame = 5;
                row = 3;
                spriteSpeed = 5;
            }
            //jumping
            else if (cm.getPlayers()[cm.playerID].jumping)
            {
                spriteWidth = 36;
                spriteHeight = 62; //wonky
                if (cm.getPlayers()[cm.playerID].dy < 0)
                {
                    playerTotalFrame = 2;
                    column = playerCurrentFrame;
                    row = 3.75f;
                }
                //falling
                else if (cm.getPlayers()[cm.playerID].dy >= 0)
                {
                    playerTotalFrame = 1;
                    column = 2;
                    row = 3.75f;
                }
            }
            //falling
            else if (cm.getPlayers()[cm.playerID].dy > 6)
            {
                spriteWidth = 36;
                spriteHeight = 62;
                playerTotalFrame = 1;
                column = 2;
                row = 3.75f; //because height is not 58 like everything else
            }

            //running
            else if (cm.getPlayers()[cm.playerID].running)
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
            //cm.getCollides()[sp.Value.entityID].hitbox = destinationRectangle;
            //Make it so that shooting sprite isn't really off
            if (cm.getPlayers()[cm.playerID].shooting && !cm.getPlayers()[cm.playerID].is_right)
            {
                destinationRectangle.X = destinationRectangle.X - (73 - 36);
            }
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
                if (cm.getPlayers()[cm.playerID].shooting)
                    playerCurrentFrame = 3;
            }
        }
    }
}
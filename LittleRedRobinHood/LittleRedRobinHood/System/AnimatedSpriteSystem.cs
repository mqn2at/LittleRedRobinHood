﻿using System;
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
        float time;
        const float frameTime = 0.1f;

        public bool running = false;
        public bool idle = true;
        public bool shooting = false;

        private int playerCurrentFrame = 0;
        private int playerTotalFrame = 10;
        private int patrolCurrentFrame = 0;
        private int patrolTotalFrame = 3;
        private int spriteSpeed = 10;
        //private int spriteWidth;
        //private int spriteHeight;
        public List<Vector2> idleCoords;
        public List<Vector2> runningCoords;
        public List<Vector2> shootingCoords;
        public List<Vector2> flyingCoords;
        public List<Vector2> jumpCoords;
        //End Sprite Animation

        public AnimatedSpriteSystem()
        {
            //Initialize player spritesheet coordinates
            idleCoords = new List<Vector2>();
            idleCoords.Add(new Vector2(0, 0));
            idleCoords.Add(new Vector2(1, 0));
            idleCoords.Add(new Vector2(2, 0));
            idleCoords.Add(new Vector2(3, 0));
            idleCoords.Add(new Vector2(4, 0));
            idleCoords.Add(new Vector2(5, 0));
            idleCoords.Add(new Vector2(6, 0));
            idleCoords.Add(new Vector2(7, 0));
            idleCoords.Add(new Vector2(8, 0));
            idleCoords.Add(new Vector2(9, 0));

            runningCoords = new List<Vector2>();
            runningCoords.Add(new Vector2(0, 1));
            runningCoords.Add(new Vector2(1, 1));
            runningCoords.Add(new Vector2(2, 1));
            runningCoords.Add(new Vector2(3, 1));
            runningCoords.Add(new Vector2(4, 1));
            runningCoords.Add(new Vector2(5, 1));
            runningCoords.Add(new Vector2(6, 1));
            runningCoords.Add(new Vector2(7, 1));
            runningCoords.Add(new Vector2(8, 1));
            runningCoords.Add(new Vector2(9, 1));

            shootingCoords = new List<Vector2>();
           /* shootingCoords.Add(new Vector2(4, 3));
            shootingCoords.Add(new Vector2(5, 3));
            shootingCoords.Add(new Vector2(6, 3));
            shootingCoords.Add(new Vector2(7, 3));*/
            //Initialize player spritesheet coordinates
           /* idleCoords = new List<Vector2>();
            idleCoords.Add(new Vector2(0, 0));
            idleCoords.Add(new Vector2(1, 0));
            idleCoords.Add(new Vector2(2, 0));
            idleCoords.Add(new Vector2(3, 0));

            runningCoords = new List<Vector2>();
            runningCoords.Add(new Vector2(4, 0));
            runningCoords.Add(new Vector2(5, 0));
            runningCoords.Add(new Vector2(6, 0));
            runningCoords.Add(new Vector2(7, 0));
            runningCoords.Add(new Vector2(0, 1));
            runningCoords.Add(new Vector2(1, 1));
            runningCoords.Add(new Vector2(2, 1));
            runningCoords.Add(new Vector2(3, 1));

            shootingCoords = new List<Vector2>();
            shootingCoords.Add(new Vector2(4, 3));
            shootingCoords.Add(new Vector2(5, 3));
            shootingCoords.Add(new Vector2(6, 3));
            shootingCoords.Add(new Vector2(7, 3));*/

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
                    int row = 0;
                    
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
                        //shooting
                       /* if (cm.getPlayers()[sp.Value.entityID].shooting)
                        {
                            column = (int)(this.shootingCoords[playerCurrentFrame / spriteSpeed].X);
                            row = (int)(this.shootingCoords[playerCurrentFrame / spriteSpeed].Y);
                            //update current player frame
                            if (playerCurrentFrame == playerTotalFrame - 1)
                            {
                                cm.getPlayers()[sp.Value.entityID].shooting = false;
                                playerCurrentFrame = 0;
                                playerTotalFrame = spriteSpeed * idleCoords.Count;
                            }
                        }*/
                        //running
                        /*else*/ if (cm.getPlayers()[sp.Value.entityID].running)
                        {
                            //column = (int)(this.runningCoords[playerCurrentFrame / spriteSpeed].X);
                            column = playerCurrentFrame;
                            //row = (int)(this.runningCoords[playerCurrentFrame / spriteSpeed].Y);
                            row = 1;
                        }
                        //idle
                        else
                        {
                            //column = (int)(this.idleCoords[playerCurrentFrame / spriteSpeed].X);
                            column = playerCurrentFrame;
                            //Console.WriteLine("playerCurrentFrame: " + playerCurrentFrame);
                            //row = (int)(this.idleCoords[playerCurrentFrame / spriteSpeed].Y);
                            row = 0;
                        }
                        //grab the current animation frame
                        Rectangle sourceRectangle = new Rectangle(spriteWidth * column, spriteHeight * row, spriteWidth, spriteHeight);
                        Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
                        //draw the current animation frame
                        sb.Draw(image, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), effect, 1);

                        //update the current frames
                        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        while (time > frameTime)
                        {
                            // Play the next frame in the SpriteSheet
                            playerCurrentFrame++;
                            // reset elapsed time
                            time = 0f;
                        }
                        if (playerCurrentFrame >= playerTotalFrame)
                            playerCurrentFrame = 0;
                        /* playerCurrentFrame++;
                        if (playerCurrentFrame == playerTotalFrame)
                        {
                            playerCurrentFrame = 0;
                        }*/
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
                            column = (int)(this.flyingCoords[patrolCurrentFrame / spriteSpeed].X);
                            row = (int)(this.flyingCoords[patrolCurrentFrame / spriteSpeed].Y);
                            //update current patrol frame
                            if (patrolCurrentFrame == patrolTotalFrame - 1)
                            {
                                patrolCurrentFrame = 0;
                                patrolTotalFrame = spriteSpeed * flyingCoords.Count;
                            }
                        }
                        //grab the current animation frame
                        Rectangle sourceRectangle = new Rectangle(spriteWidth * column, spriteHeight * row, spriteWidth, spriteHeight);
                        Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
                        //draw the current animation frame
                        sb.Draw(image, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), effect, 1);

                        //update the current frames
                        patrolCurrentFrame++;
                        if (patrolCurrentFrame == patrolTotalFrame)
                        {
                            patrolCurrentFrame = 0;
                        }
                    }
                }
                else //no animation, so just draw
                {
                   sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
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
            for (int x = 0; x < pl.health; x++)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;

namespace LittleRedRobinHood.System
{
    class AnimatedSpriteSystem
    {
        //Sprite Animation Stuff
        public bool running = false;
        public bool idle = true;
        public bool shooting = false;

        private int currentFrame;
        private int totalFrame;
        private int spriteSpeed = 10;
        //private int spriteWidth;
        //private int spriteHeight;
        public List<Vector2> idleCoords;
        public List<Vector2> runningCoords;
        public List<Vector2> shootingCoords;
        ///public List<Vector2> jump;
        //End Sprite Animation

        public AnimatedSpriteSystem()
        {
            //Initialize player spritesheet coordinates
            currentFrame = 0;
            totalFrame = 40;
            idleCoords = new List<Vector2>();
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
            shootingCoords.Add(new Vector2(7, 3));
        }

        public void Draw(SpriteBatch sb, ComponentManager cm)
        {
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
                    //Player animation
                    if (cm.getPlayers()[sp.Value.entityID] != null)
                    {
                        int column = 0;
                        int row = 0;
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
                        if (cm.getPlayers()[sp.Value.entityID].shooting)
                        {
                            column = (int)(this.shootingCoords[currentFrame / spriteSpeed].X);
                            row = (int)(this.shootingCoords[currentFrame / spriteSpeed].Y);
                            Console.WriteLine(currentFrame + "/" + totalFrame);
                            if (currentFrame == totalFrame - 1)
                            {
                                cm.getPlayers()[sp.Value.entityID].shooting = false;
                                currentFrame = 0;
                                totalFrame = spriteSpeed * idleCoords.Count;
                            }
                        }
                        //running
                        else if (cm.getPlayers()[sp.Value.entityID].running)
                        {
                            column = (int)(this.runningCoords[currentFrame / spriteSpeed].X);
                            row = (int)(this.runningCoords[currentFrame / spriteSpeed].Y);
                        }
                        //idle
                        else
                        {
                            column = (int)(this.idleCoords[currentFrame / spriteSpeed].X);
                            row = (int)(this.idleCoords[currentFrame / spriteSpeed].Y);
                        }
                        //grab the current animation frame
                        Rectangle sourceRectangle = new Rectangle(spriteWidth * column, spriteHeight * row, spriteWidth, spriteHeight);
                        Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
                        //draw the current animation frame
                        sb.Draw(image, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), effect, 1);

                        //update the current frames
                        currentFrame++;
                        if (currentFrame == totalFrame)
                        {
                            currentFrame = 0;
                        }
                    }
                    //Patrol animation
                    else if (cm.getPatrols()[sp.Value.entityID] != null) {

                    }
                }
                else //no animation, so just draw
                {
                   sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
                }
            }
        }
    }
}
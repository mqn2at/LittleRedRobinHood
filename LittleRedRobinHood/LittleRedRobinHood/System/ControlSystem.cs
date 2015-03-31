using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using LittleRedRobinHood.Component;
using LittleRedRobinHood.Entities;

namespace LittleRedRobinHood.System
{
    class ControlSystem
    {
        public KeyboardState kb;
        public KeyboardState kbo;
        public MouseState ms;
        public MouseState mso;
        private bool clicked = false;
        private int TIMER_MAX = 10;
        private int SHACKLE_SPEED = 9;
        private int ARROW_SPEED = 12;
        private int X_SPEED = 4;
        private int JUMP = 15;
        private int MAX_FALL = 20;
        private int timer;

        public ControlSystem()
        {
            this.kb = Keyboard.GetState();
            this.kbo = Keyboard.GetState();
            this.ms = Mouse.GetState();
            this.mso = Mouse.GetState();
            this.timer = TIMER_MAX;
        }

        public bool Update(ComponentManager cm)
        {
            kbo = kb;
            kb = Keyboard.GetState();
            mso = ms;
            ms = Mouse.GetState();
            Player player = cm.getPlayers()[cm.playerID];
            Collide pMove = cm.getCollides()[cm.playerID];
            
            //Check reset first; we don't have to do anything if we are reseting
            if (isPressed(Keys.R))
            {
                Console.WriteLine("RESET!");
                return true;
            }

            if (player.grounded)
            {
                player.dy = 0;
                //Jumping
                if (isPressed(Keys.Space) || isPressed(Keys.W) || isPressed(Keys.Up))
                {
                    player.dy -= JUMP;
                    player.jumping = true;
                    player.grounded = false;
                }
            }
            //Player is falling; apply gravity then update position. Player can also accelerate even faster down.
            else
            {
                if (player.dy < MAX_FALL)
                {
                    player.dy += 1;
                }
                if (isPressed(Keys.S) || isPressed(Keys.Down))
                {
                    if (player.dy < MAX_FALL)
                    {
                        player.dy += 1;
                    }
                }
                pMove.hitbox.Y += player.dy;
            }

            if (isPressed(Keys.D) || isPressed(Keys.Right))
            {
                pMove.hitbox.X += X_SPEED;
                player.grounded = false;
                player.running = true;
                player.is_right = true;
                //cm.getSprites()[player.entityID].effect = SpriteEffects.None;
            }
            else if (isPressed(Keys.A) || isPressed(Keys.Left))
            {
                pMove.hitbox.X -= X_SPEED;
                player.grounded = false;
                player.running = true;
                //cm.getSprites()[player.entityID].effect = SpriteEffects.FlipHorizontally;
                player.is_right = false;
            }
            else
            {
                player.running = false;
            }

            //Check if we have clicked recently, with a timer before the next click is accepted
            if (clicked)
            {
                if (timer > 0)
                {
                    timer -= 1;
                    player.shooting = true;
                }
                else
                {
                    Console.WriteLine("timer done!");
                    clicked = false;
                    player.shooting = false;
                }
            }
            /*
            else
            {
                mso = ms;
                ms = Mouse.GetState();
                /*
                Console.WriteLine("check mouse");
                if (ms.LeftButton != mso.LeftButton || ms.RightButton != mso.RightButton)
                {
                    Console.WriteLine("new mouse state");
                }
                 /
            }
            */
            
            //Shackle
            if (!clicked && player.shackles > 0 && ms.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("-1 shackle");
                double angle = Math.Atan2((ms.Y - 8) - (pMove.hitbox.Y + pMove.hitbox.Height / 2), (ms.X - 8) - (pMove.hitbox.X + pMove.hitbox.Width / 2));
                int temp = cm.addEntity();
                cm.addProjectile(temp, false, angle, SHACKLE_SPEED);
                cm.addCollide(temp, new Rectangle(pMove.hitbox.X + pMove.hitbox.Width/2, pMove.hitbox.Y + pMove.hitbox.Height/2, 16, 16), false, false);
                cm.addSprite(temp, 16, 16, cm.conman.Load<Texture2D>("rope.png"));
                player.shackles -= 1;
                //player.shooting = true;
                //Force timer before next click
                clicked = true;
                timer = TIMER_MAX;
            }
            //Arrow
            else if (!clicked && player.arrows > 0 && ms.RightButton == ButtonState.Pressed)
            {
                Console.WriteLine("-1 arrow");
                double angle = Math.Atan2((ms.Y - 8) - (pMove.hitbox.Y + pMove.hitbox.Height / 2), (ms.X - 8) - (pMove.hitbox.X + pMove.hitbox.Width / 2));
                int temp = cm.addEntity();
                cm.addProjectile(temp, true, angle, ARROW_SPEED);
                cm.addCollide(temp, new Rectangle(pMove.hitbox.X + pMove.hitbox.Width / 2, pMove.hitbox.Y + pMove.hitbox.Height / 2, 16, 16), false, false);
                cm.addSprite(temp, 16, 16, cm.conman.Load<Texture2D>("arrow.gif"));
                player.arrows -= 1;
                //player.shooting = true;
                //Force timer before next click
                clicked = true;
                timer = TIMER_MAX;
            }
            else
            {
                //player.shooting = false;
            }
            //DON'T RESET
            return false;

        }

        public bool isPressed(Keys key)
        {
            //Console.WriteLine (button);
            return kb.IsKeyDown(key);
        }

        public bool onPress(Keys key)
        {
            return kb.IsKeyDown(key) && kbo.IsKeyUp(key);
        }

        public bool onRelease(Keys key)
        {
            //Console.WriteLine (button);
            return kb.IsKeyUp(key) && kbo.IsKeyDown(key);
        }

        public bool isHeld(Keys key)
        {
            //Console.WriteLine (button);
            return kb.IsKeyDown(key) && kbo.IsKeyDown(key);
        }

    }
}

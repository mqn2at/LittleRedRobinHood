﻿using System;
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
        private int timer = 15;

        public ControlSystem()
        {
            this.kb = Keyboard.GetState();
            this.kbo = Keyboard.GetState();
            this.ms = Mouse.GetState();
            this.mso = Mouse.GetState();
        }

        public void Update(ComponentManager cm)
        {
            kbo = kb;
            kb = Keyboard.GetState();
            ms = Mouse.GetState();
            mso = ms;
            Player player = cm.getPlayers()[cm.playerID];
            Collide pMove = cm.getCollides()[cm.playerID];
            //Running Right
            if (isPressed(Keys.D))
            {
                pMove.hitbox.X += 5;
            }
            //Running left
            else if (isPressed(Keys.A))
            {
                pMove.hitbox.X -= 5;
            }
            //Are they on ground? If so, verify they have 0 y momentum. Allow jumps
            if (player.grounded)
            {
                player.dy = 0;
                //Jumping
                if (isPressed(Keys.Space) || isPressed(Keys.W))
                {
                    player.dy = -20;
                    player.grounded = false;
                }
            }
            //Player is falling; apply gravity then update position
            else
            {
                if (player.dy < player.maxFall)
                {
                    player.dy += 1;
                }
                pMove.hitbox.Y += player.dy;
            }

            //Check if we have clicked recently, with a timer before the next click is accepted
            if (clicked)
            {
                if (timer > 0)
                {
                    timer -= 1;
                }
                else
                {
                    clicked = false;
                }
            }
            else
            {
                mso = ms;
                ms = Mouse.GetState();
            }
            
            //Shackle
            if (!clicked && ms.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("shackle fired");
                double angle = Math.Atan2(ms.Y - pMove.hitbox.Y, ms.X - pMove.hitbox.X);
                int temp = cm.addEntity();
                cm.addProjectile(temp, false, angle, 15);
                cm.addCollide(temp, new Rectangle(pMove.hitbox.X, pMove.hitbox.Y - 100, 15, 15), false, false);
                cm.addSprite(temp, 15, 15, cm.conman.Load<Texture2D>("Sprite-Soda.png"));
                //Force timer before next click
                clicked = true;
                timer = 15;
            }
            //Arrow
            else if (!clicked && ms.RightButton == ButtonState.Pressed)
            {
                Console.WriteLine("arrow fired");
                double angle = Math.Atan2(ms.Y - pMove.hitbox.Y, ms.X - pMove.hitbox.X);
                int temp = cm.addEntity();
                cm.addProjectile(temp, false, angle, 15);
                cm.addCollide(temp, new Rectangle(pMove.hitbox.X, pMove.hitbox.Y - 100, 15, 15), false, false);
                cm.addSprite(temp, 15, 15, cm.conman.Load<Texture2D>("Sprite-Soda.png"));
                //Force timer before next click
                clicked = true;
                timer = 15;
            }

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

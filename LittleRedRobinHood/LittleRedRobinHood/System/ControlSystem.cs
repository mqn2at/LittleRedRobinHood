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

        public ControlSystem()
        {
            this.kb = Keyboard.GetState();
            this.kbo = Keyboard.GetState();
        }

        public void Update(ComponentManager cm)
        {
            Player player = cm.getPlayers()[cm.playerID];
            Collide pMove = cm.getCollides()[cm.playerID];
            kbo = kb;
            kb = Keyboard.GetState();
            if (isPressed(Keys.D))
            {
                pMove.hitbox.X += 5;
            }
            else if (isPressed(Keys.A))
            {
                pMove.hitbox.X -= 5;
            }
            if (player.grounded)
            {
                player.dy = 0;
                if (isPressed(Keys.Space) || isPressed(Keys.W))
                {
                    player.dy = -20;
                    player.grounded = false;
                }
            }
            else
            {
                if (player.dy < player.maxFall)
                {
                    player.dy += 1;
                }
                pMove.hitbox.Y += player.dy;
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

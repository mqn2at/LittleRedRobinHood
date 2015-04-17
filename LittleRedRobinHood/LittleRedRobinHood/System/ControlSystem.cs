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
        public int menuIndex = 0;
        public bool subMenu = false;
        private int TIMER_MAX = 8;
        private int SHACKLE_SPEED = 9;
        private int ARROW_SPEED = 12;
        private int X_SPEED = 4;
        private int JUMP = 15;
        private int MAX_FALL = 17;
        private int TIMER_JUMP_MAX = 20;
        private int jump_timer;
        private int timer;
        private int menuTimer;

        public ControlSystem()
        {
            this.kb = Keyboard.GetState();
            this.kbo = Keyboard.GetState();
            this.ms = Mouse.GetState();
            this.mso = Mouse.GetState();
            this.timer = TIMER_MAX;
        }

        public void UpdateStates()
        {
            kbo = kb;
            kb = Keyboard.GetState();
            mso = ms;
            ms = Mouse.GetState();
        }
        public bool Update(ComponentManager cm)
        {
            Player player = cm.getPlayers()[cm.playerID];
            Collide pMove = cm.getCollides()[cm.playerID];
            if (jump_timer > 0)
            {
                jump_timer--;
            }

            if (player.grounded)
            {
                player.dy = 0;
                //Jumping
                if ((isPressed(Keys.Space) || isPressed(Keys.W) || isPressed(Keys.Up)) && jump_timer == 0)
                {
                    player.dy -= JUMP;
                    player.jumping = true;
                    player.grounded = false;
                    jump_timer = TIMER_JUMP_MAX;
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
                    cm.soundsys.playBow();
                }
                else
                {
                    //Console.WriteLine("timer done!");
                    clicked = false;
                    player.shooting = false;
                    cm.soundsys.stopBow();
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
                if (pMove.hitbox.X - ms.X > 0)
                {
                    player.is_right = false;
                }
                else
                {
                    player.is_right = true;
                }
                //Console.WriteLine("-1 shackle");
                double angle = Math.Atan2((ms.Y - 8) - (pMove.hitbox.Y + (pMove.hitbox.Height / 2) ), (ms.X - 8) - (pMove.hitbox.X + (pMove.hitbox.Width / 2) ));
                int temp = cm.addEntity();
                cm.addProjectile(temp, false, angle, SHACKLE_SPEED);
                cm.addCollide(temp, new Rectangle(pMove.hitbox.X + (pMove.hitbox.Width / 2) - (4), pMove.hitbox.Y + (pMove.hitbox.Height / 2) - (4), 16, 16), false, false);
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
                if (pMove.hitbox.X - ms.X > 0)
                {
                    player.is_right = false;
                }
                else
                {
                    player.is_right = true;
                }
                //Console.WriteLine("-1 arrow");
                double angle = Math.Atan2((ms.Y - 8) - (pMove.hitbox.Y + (pMove.hitbox.Height / 2) ), (ms.X - 8) - (pMove.hitbox.X + (pMove.hitbox.Width / 2) ));
                int temp = cm.addEntity();
                cm.addProjectile(temp, true, angle, ARROW_SPEED);
                cm.addCollide(temp, new Rectangle(pMove.hitbox.X + (pMove.hitbox.Width / 2) - (4), pMove.hitbox.Y + (pMove.hitbox.Height / 2) - (4), 16, 16), false, false);
                cm.addSprite(temp, 16, 16, cm.conman.Load<Texture2D>("arrow.png"));
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

        public bool checkReset()
        {
            return onPress(Keys.R);
        }
        
        //1 is P pressed (pause), 0 is M pressed (quit/main menu), -1 is no change
        public int checkPause()
        {
            if (onPress(Keys.P))
            {
                return 1;
            }
            else if (onPress(Keys.M))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        //handle menu controls
        public int UpdateMenu(ComponentManager cm)
        {
            if (menuTimer > 0)
            {
                menuTimer -= 1;
            }
            else
            {
                if (isPressed(Keys.Enter) || isPressed(Keys.E))
                {
                    menuTimer = TIMER_MAX;
                    if (subMenu)
                    {
                        if (menuIndex == cm.numStages)
                        {
                            subMenu = false;
                            menuIndex = 1;
                            return -1;
                        }
                        return menuIndex;
                    }
                    else
                    {
                        switch (menuIndex)
                        {
                            case 0:
                                return 0;
                            case 1:
                                subMenu = true;
                                menuIndex = 0;
                                break;
                            default:
                                Console.WriteLine("there is a problem");
                                break;
                        }

                    }
                }
                else if (isPressed(Keys.Up) || isPressed(Keys.W))
                {
                    menuTimer = TIMER_MAX;
                    if (menuIndex == 0)
                    {
                        if (subMenu)
                        {
                            menuIndex = cm.numStages;
                        }
                        else
                        {
                            menuIndex = 1;
                        }
                    }
                    else
                    {
                        menuIndex -= 1;
                    }
                }
                else if (isPressed(Keys.Down) || isPressed(Keys.S))
                {
                    menuTimer = TIMER_MAX;
                    if ((subMenu && menuIndex == cm.numStages) || (!subMenu && menuIndex >= 1))
                    {
                        menuIndex = 0;
                    }
                    else
                    {
                        menuIndex += 1;
                    }
                }
                else if (subMenu)
                {

                    if (isPressed(Keys.Right) || isPressed(Keys.D))
                    {
                        menuTimer = TIMER_MAX;
                        menuIndex += 6;
                        if (menuIndex > cm.numStages)
                        {
                            menuIndex = menuIndex % 6;
                        }
                    }
                    else if (isPressed(Keys.Left) || isPressed(Keys.A))
                    {
                        menuTimer = TIMER_MAX;
                        menuIndex -= 6;
                        if (menuIndex < 0)
                        {
                            menuIndex += (6*(1+(cm.numStages/6)));
                            if (menuIndex > cm.numStages)
                            {
                                menuIndex = cm.numStages;
                            }
                        }
                    }

                }
            }
            return -1;
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

        public int mouseX()
        {
            return Mouse.GetState().X;
        }

        public int mouseY()
        {
            return Mouse.GetState().Y;
        }


        public int UpdateEndScreen(ComponentManager manager)
        {
            if (isPressed(Keys.M) || isPressed(Keys.Enter) || isPressed(Keys.E))
            {
                return 1;
            }
            return -1;
        }
    }
}

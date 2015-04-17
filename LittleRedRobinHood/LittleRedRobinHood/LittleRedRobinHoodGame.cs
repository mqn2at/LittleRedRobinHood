#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using LittleRedRobinHood.Entities;
using LittleRedRobinHood.Component;
using LittleRedRobinHood.System;
//using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace LittleRedRobinHood
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LittleRedRobinHoodGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        List<Stage> stages;
        ComponentManager manager;
        CollisionSystem colsys;
        ControlSystem consys;
        ProjectileSystem projsys;
        PathingSystem pathsys;
        AnimatedSpriteSystem anisys;
        private bool paused, mainMenu, dead, realDead, loading, victory;
        private int loadTimer = 0;
        private int LOAD_TIMER_MAX = 15;
        private int MENU_COUNT = 6;
        private int TITLESTART = 25;
        private int MENUSTART_X = 50;
        private int MENUSTART_Y = 70;
        private int MENUOFFSET_Y = 30;
        private int SUBMENUOFFSET_X = 50;
        private int SUBMENUOFFSET_Y = 30;
        private int SUBMENUSTART_Y = 70;
        private int SELECTOFFSET_X = 10;
        private Rectangle screenBox;
        private Single TITLESIZE = 1.5F;
        private Texture2D titleScreen, pauseScreen, deathScreen, victoryScreen;
        

        int currentStage = 0;
        public LittleRedRobinHoodGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../../../Content"; //this gets out of the Debug Content
            //Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            this.manager = new ComponentManager(this.Content);
            colsys = new CollisionSystem();
            consys = new ControlSystem();
            projsys = new ProjectileSystem();
            pathsys = new PathingSystem();
            anisys = new AnimatedSpriteSystem();
            paused = false;
            mainMenu = true; //start with main menu open
            dead = false;
            realDead = false;
            victory = false;
            //Create stages
            this.stages = new List<Stage>();
            
            Stage stage1 = new Stage("stage1.tmx", this.manager);
            this.stages.Add(stage1);
            Stage stage2 = new Stage("stage2.tmx", this.manager);
            this.stages.Add(stage2);
            Stage stage3 = new Stage("stage3.tmx", this.manager);
            this.stages.Add(stage3);
            Stage stage4 = new Stage("stage4.tmx", this.manager);
            this.stages.Add(stage4);
            Stage stage5 = new Stage("stage5.tmx", this.manager);
            this.stages.Add(stage5);
            Stage stage6 = new Stage("stage6.tmx", this.manager);
            this.stages.Add(stage6);
            Stage stage7 = new Stage("stage7.tmx", this.manager);
            this.stages.Add(stage7);
            Stage stage8 = new Stage("stage8.tmx", this.manager);
            this.stages.Add(stage8);
            Stage stage9 = new Stage("stage9.tmx", this.manager);
            this.stages.Add(stage9);

            this.manager.numStages = this.stages.ToArray().Length;
            this.screenBox = new Rectangle(0, 0, 800, 480);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //text
            font = this.Content.Load<SpriteFont>("Arial");
            titleScreen = this.Content.Load<Texture2D>("titlescreen.jpg");
            deathScreen = this.Content.Load<Texture2D>("deathscreen.jpg");
            pauseScreen = this.Content.Load<Texture2D>("pausescreen.jpg");
            victoryScreen = this.Content.Load<Texture2D>("victoryscreen.jpg");
            this.LoadMainMenu();
            //stages[currentStage].LoadContent(this.Content); now called during updates of main menu
            // TODO: use this.Content to load your game content here
          
        }
        protected void LoadStage(int stageNum)
        {
            loading = true;
            loadTimer = LOAD_TIMER_MAX;
            manager.clearDictionaries();
            currentStage = stageNum;
            stages[currentStage].LoadContent(this.Content, stageNum);
            //LoadPauseMenu();

        }

        protected void LoadMainMenu()
        {
            //Reset menu details
            consys.subMenu = false;
            consys.menuIndex = 0;
            //Add selection indicator
            int temp = manager.addEntity();
            manager.setSelect(temp);
            manager.addCollide(temp, new Rectangle(this.MENUSTART_X - 20 - this.SELECTOFFSET_X, this.MENUSTART_Y, 20, 20), false, false);
            manager.addSprite(temp, 20, 20, this.Content.Load<Texture2D>("rope.png"));
            //Add texts to be drawn
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(this.TITLESTART, this.TITLESTART), "Little Red Robin Hood", true, this.TITLESIZE);
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(this.MENUSTART_X, this.MENUSTART_Y), "New Game", true, 1);
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(this.MENUSTART_X, this.MENUSTART_Y + this.MENUOFFSET_Y), "Level Select", true, 1);

            //Song
            manager.soundsys.stopSong();
            manager.soundsys.playMenuSong();
        }

        protected void LoadPauseMenu()
        {
            //Reset menu details
            consys.subMenu = false;
            consys.menuIndex = 0;
            /*
            //Add texts to be drawn
            int temp = manager.addEntity();
            temp = manager.addEntity();
            //manager.addText(temp, font, new Vector2(0, 0), new Vector2(this.TITLESTART, this.TITLESTART), "Little Red Robin Hood", true, this.TITLESIZE);
            //temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(350, 150), "PAUSED", true, 1);
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(350, 175), "Press P to unpause\nPress R to reset\nPress M to quit", true, 1);
            */
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //should really do something with this
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            consys.UpdateStates();
            manager.soundsys.Update(mainMenu, currentStage);
            if (realDead)
            {
                int temp = consys.UpdateEndScreen(manager);
                if (temp > -1)
                {
                    mainMenu = true;
                    realDead = false;
                }

            }
            else if (loading)
            {
                if (loadTimer <= 0)
                {
                    loading = false;
                }
                loadTimer -= 1;
            }
            else if (mainMenu)
            {
                int temp = consys.UpdateMenu(manager);
                if (temp > -1)
                {
                    mainMenu = false;
                    LoadStage(temp);
                    manager.soundsys.playGameSong(currentStage);
                }
            }
            else if (victory)
            {
                int temp = consys.UpdateEndScreen(manager);
                if (temp > -1)
                {
                    mainMenu = true;
                    victory = false;
                }
            }
            else
            {
                if (consys.checkReset())
                {
                    dead = false;
                    paused = false;
                    int temp = manager.currentLives();
                    LoadStage(currentStage);
                    manager.persistLives(temp);
                }
                if (!paused && !dead)
                {
                    consys.Update(manager);
                    projsys.Update(manager, GraphicsDevice);
                    pathsys.Update(manager);
                    int lives;
                    switch (colsys.Update(manager, GraphicsDevice))
                    {
                        case 1:
                            if (currentStage < manager.numStages - 1) // load next stage
                            {
                                lives = manager.currentLives();
                                currentStage += 1;
                                LoadStage(currentStage);
                                manager.persistLives(lives);
                                break;
                            }
                            else // load victory screen
                            {
                                manager.clearDictionaries();
                                LoadMainMenu();
                                victory = true;
                                break;
                            }
                        case -1:
                            break;
                        default:
                            lives = manager.currentLives();
                            if (lives == 0) //add death screen here later
                            {
                                realDead = true;
                                manager.clearDictionaries();
                                LoadMainMenu();
                                break;
                            }
                            else
                            {
                                dead = true;
                                LoadStage(currentStage);
                                manager.persistLives(lives);
                                break;
                            }
                    }
                }
                switch (consys.checkPause())
                {
                    case 1:
                        paused = !paused;
                        break;
                    case 0:
                        if (paused)
                        {
                            manager.clearDictionaries();
                            LoadMainMenu();
                            mainMenu = true;
                            paused = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (mainMenu)
            {
                spriteBatch.Draw(titleScreen, screenBox, Color.White);
                foreach (KeyValuePair<int, Text> pair in manager.getTexts())
                {
                    if (pair.Value.visible)
                    {
                        spriteBatch.DrawString(pair.Value.font, pair.Value.text, pair.Value.textPosition, Color.Black, 0, new Vector2(0, 0), pair.Value.scale, SpriteEffects.None, 1);
                    }
                }
                Collide selC = manager.getCollides()[manager.selectID];
                Sprite selS = manager.getSprites()[manager.selectID];
                int selX = selC.hitbox.X;
                int selY = selC.hitbox.Y;
                if (consys.subMenu)
                {
                    selX += this.SUBMENUOFFSET_X; //adjust for indented submenu
                    selY = this.MENUSTART_Y + this.MENUOFFSET_Y + this.SUBMENUOFFSET_Y + 10; // start of submenu
                    for (int x = 0; x <= manager.numStages; x++)
                    {
                        if (x != manager.numStages)
                        {
                            spriteBatch.DrawString(font, "Stage: " + (x + 1), new Vector2(this.MENUSTART_X + this.SUBMENUOFFSET_X + this.SUBMENUOFFSET_X * 3 * (x / MENU_COUNT) + selC.hitbox.Width, this.MENUSTART_Y + this.SUBMENUSTART_Y + this.SUBMENUOFFSET_Y * (x % MENU_COUNT)), Color.Black);
                        }
                        else
                        {
                            spriteBatch.DrawString(font, "Back", new Vector2(this.MENUSTART_X + this.SUBMENUOFFSET_X + this.SUBMENUOFFSET_X * 3 * (x / MENU_COUNT) + selC.hitbox.Width, this.MENUSTART_Y + this.SUBMENUSTART_Y + this.SUBMENUOFFSET_Y * (x % MENU_COUNT)), Color.Black);
                        }
                    }
                    selX += selC.hitbox.Width;
                }
                selY += (consys.menuIndex % MENU_COUNT) * this.MENUOFFSET_Y;
                selX += (consys.menuIndex / MENU_COUNT) * 3 * this.SUBMENUOFFSET_X;
                //Console.WriteLine("MenuIndex: " + consys.menuIndex);
                spriteBatch.Draw(selS.sprite, new Rectangle(selX, selY, selC.hitbox.Width, selC.hitbox.Height), Color.White);
            }
            else if (!paused && !realDead && !dead && !victory)
            {
                stages[currentStage].Draw(spriteBatch, GraphicsDevice);
                /*Dictionary<int, Collide> collides = manager.getCollides();
                foreach(KeyValuePair<int, Sprite> sp in manager.getSprites()) {
                    spriteBatch.Draw(sp.Value.sprite, collides[sp.Value.entityID].hitbox, Color.White);
                }*/
                //Moved above foreach to AnimatedSpriteSystem
                Texture2D crosshair = manager.conman.Load<Texture2D>("crosshair");
                spriteBatch.Draw(crosshair, new Vector2(consys.mouseX(), consys.mouseY()), Color.White);
                DrawTrajectoryLine(spriteBatch);
                anisys.Draw(spriteBatch, manager, gameTime);
                foreach (KeyValuePair<int, Text> pair in manager.getTexts())
                {
                    if (pair.Value.visible)
                    {
                        spriteBatch.DrawString(pair.Value.font, pair.Value.text, pair.Value.textPosition, Color.White);
                    }
                }
                //DrawLine(spriteBatch, new Vector2(200, 200), new Vector2(100, 100));
                DrawShackle();
                //spriteBatch.Draw(manager.getSprites()[manager.playerID].sprite, manager.getCollides()[manager.playerID].hitbox, Color.White);
                // TODO: Add your drawing code here
                //spriteBatch.DrawString(font, "HELLO", new Vector2(120, 10), Color.White);
            }
            else if(paused)
            {
                spriteBatch.Draw(pauseScreen, screenBox, Color.White);
                //Display paused information
                spriteBatch.DrawString(font, "PAUSED", new Vector2(350, 150), Color.Black);
                spriteBatch.DrawString(font, "Press P to unpause\nPress R to reset\nPress M to quit", new Vector2(350, 175), Color.Black);
                    /*
                foreach (KeyValuePair<int, Text> pair in manager.getTexts())
                {
                    if (pair.Value.visible)
                    {
                        spriteBatch.DrawString(pair.Value.font, pair.Value.text, pair.Value.textPosition, Color.Black, 0, new Vector2(0, 0), pair.Value.scale, SpriteEffects.None, 1);
                    }
                }*/
            }
            else if(realDead)
            {
                spriteBatch.Draw(deathScreen, screenBox, Color.White);
                spriteBatch.DrawString(font, "               Game Over\nPress M to return to main menu", new Vector2(260, 180), Color.White);
            }
            else if (dead)
            {
                spriteBatch.Draw(deathScreen, screenBox, Color.White);
                spriteBatch.DrawString(font, "     You died\nPress R to reset", new Vector2(320, 180), Color.White);
            }
            else if (victory)
            {
                spriteBatch.Draw(victoryScreen, screenBox, Color.White);
                spriteBatch.DrawString(font, "    Congratulations you won!\nPress M to return to main menu", new Vector2(315, 200), Color.Black);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //FIX THE DRAWING THE SHACKLES
        protected void DrawShackle()
        {
            foreach (KeyValuePair<int, Shackle> shackle in manager.getShackles())
            {
                Rectangle rectangle1 = manager.getCollides()[shackle.Value.firstPointID].hitbox;
                Rectangle rectangle2 = manager.getCollides()[shackle.Value.secondPointID].hitbox;
                Vector2 start = new Vector2(rectangle1.X + (int)(rectangle1.Width / 2.0), rectangle1.Y + (int)(rectangle1.Height / 2.0));
                Vector2 end = new Vector2(rectangle2.X + (int)(rectangle1.Width / 2.0), rectangle2.Y + (int)(rectangle2.Height / 2.0));
                DrawLine(spriteBatch, start, end);
            }
        }
        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
            Texture2D t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.SaddleBrown});

            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    5), //width of line, change this to make thicker line
                null,
                Color.SaddleBrown, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
        void DrawTrajectoryLine(SpriteBatch sb)
        {
            Rectangle player = manager.getCollides()[manager.playerID].hitbox;
            Vector2 start = new Vector2(player.X + player.Width/2 - 4, player.Y + player.Height/2 - 4);
            Vector2 end = new Vector2(consys.mouseX(), consys.mouseY());
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
            Texture2D t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.Red });

            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}

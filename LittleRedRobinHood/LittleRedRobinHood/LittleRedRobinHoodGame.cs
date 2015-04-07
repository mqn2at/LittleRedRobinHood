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
        private bool paused;
        private bool mainMenu;
        private int MENU_COUNT = 8;
        private int TITLESTART = 25;
        private int MENUSTART_X = 50;
        private int MENUSTART_Y = 125;
        private int MENUOFFSET_Y = 35;
        private int SUBMENUOFFSET_X = 50;
        private int SUBMENUOFFSET_Y = 35;
        private int SUBMENUSTART_Y = 70;
        private int SELECTOFFSET_X = 10;
        private Single TITLESIZE = 1.5F;
        

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
            //Create stages
            this.stages = new List<Stage>();
            Stage stage0 = new Stage("stage0.tmx", this.manager);
            this.stages.Add(stage0);
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
            this.manager.numStages = this.stages.ToArray().Length;

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
            this.LoadMainMenu();
            //stages[currentStage].LoadContent(this.Content); now called during updates of main menu
            // TODO: use this.Content to load your game content here
          
        }
        protected void LoadStage(int stageNum)
        {
            manager.clearDictionaries();
            currentStage = stageNum;
            stages[currentStage].LoadContent(this.Content);
            LoadPauseMenu();
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
            manager.addSprite(temp, 20, 20, this.Content.Load<Texture2D>("Sprite-soda.png"));
            //Add texts to be drawn
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(this.TITLESTART, this.TITLESTART), "Little Red Robin Hood", true, this.TITLESIZE);
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(this.MENUSTART_X, this.MENUSTART_Y), "New Game", true, 1);
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(this.MENUSTART_X, this.MENUSTART_Y + this.MENUOFFSET_Y), "Level Select", true, 1);

            //Song
            manager.soundsys.stopSong();
            manager.soundsys.playMenuSong();
        }

        protected void LoadPauseMenu()
        {
            //Reset menu details
            consys.subMenu = false;
            consys.menuIndex = 0;
            //Add texts to be drawn
            int temp = manager.addEntity();
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(this.TITLESTART, this.TITLESTART), "Little Red Robin Hood", true, this.TITLESIZE);
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(350, 200), "PAUSED", true, 1);
            temp = manager.addEntity();
            manager.addText(temp, font, new Vector2(0, 0), new Vector2(300, 250), "Press P to unpause\nPress M to quit", true, 1);

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
            if (mainMenu)
            {
                int temp  = consys.UpdateMenu(manager);
                if (temp > -1)
                {
                    mainMenu = false;
                    LoadStage(temp);
                    manager.soundsys.playGameSong(currentStage);
                }
            }
            else
            {
                if (consys.checkReset())
                {
                    int temp = manager.currentLives();
                    LoadStage(currentStage);
                    manager.persistLives(temp);
                }
                if (!paused)
                {
                    consys.Update(manager);
                    projsys.Update(manager, GraphicsDevice);
                    pathsys.Update(manager);
                    switch (colsys.Update(manager, GraphicsDevice))
                    {
                        case 1:
                            if (currentStage < manager.numStages - 1)
                            {
                                currentStage += 1;
                                goto default;
                            }
                            else
                            {
                                manager.clearDictionaries();
                                LoadMainMenu();
                                mainMenu = true;
                                break;
                            }
                        case -1: 
                            break;
                        default:
                            int temp = manager.currentLives();
                            if (temp == 0) //add death screen here later
                            {
                                manager.clearDictionaries();
                                LoadMainMenu();
                                mainMenu = true;
                                break;
                            }
                            else
                            {
                                LoadStage(currentStage);
                                manager.persistLives(temp);
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
                        manager.clearDictionaries();
                        LoadMainMenu();
                        mainMenu = true;
                        paused = false;
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
                    selY = this.MENUSTART_Y + this.MENUOFFSET_Y + this.SUBMENUOFFSET_Y; // start of submenu
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
            else if (!paused)
            {
                stages[currentStage].Draw(spriteBatch, GraphicsDevice);
                /*Dictionary<int, Collide> collides = manager.getCollides();
                foreach(KeyValuePair<int, Sprite> sp in manager.getSprites()) {
                    spriteBatch.Draw(sp.Value.sprite, collides[sp.Value.entityID].hitbox, Color.White);
                }*/
                //Moved above foreach to AnimatedSpriteSystem
                anisys.Draw(spriteBatch, manager, gameTime);
                //DrawLine(spriteBatch, new Vector2(200, 200), new Vector2(100, 100));
                DrawShackle();
                //spriteBatch.Draw(manager.getSprites()[manager.playerID].sprite, manager.getCollides()[manager.playerID].hitbox, Color.White);
                // TODO: Add your drawing code here
                //spriteBatch.DrawString(font, "HELLO", new Vector2(120, 10), Color.White);
            }
            else
            {
                //Display paused information
                foreach (KeyValuePair<int, Text> pair in manager.getTexts())
                {
                    if (pair.Value.visible)
                    {
                        spriteBatch.DrawString(pair.Value.font, pair.Value.text, pair.Value.textPosition, Color.Black, 0, new Vector2(0, 0), pair.Value.scale, SpriteEffects.None, 1);
                    }
                }
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
                new Color[] { Color.Black });

            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    5), //width of line, change this to make thicker line
                null,
                Color.Black, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}

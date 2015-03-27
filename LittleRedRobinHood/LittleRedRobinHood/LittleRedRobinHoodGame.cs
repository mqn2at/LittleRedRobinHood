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
        List<Stage> stages;
        ComponentManager manager;
        CollisionSystem colsys;
        ControlSystem consys;
        ProjectileSystem projsys;
        PathingSystem pathsys;
        int currentStage = 2;
        public LittleRedRobinHoodGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../../../Content"; //this gets out of the Debug Content
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

            //Create stages
            this.stages = new List<Stage>();
            Stage stage0 = new Stage("stage0.tmx", this.manager);
            this.stages.Add(stage0);
            Stage stage1 = new Stage("stage1.tmx", this.manager);
            this.stages.Add(stage1);
            Stage stage2 = new Stage("stage2.tmx", this.manager);
            this.stages.Add(stage2);

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
            stages[currentStage].LoadContent(this.Content);
            // TODO: use this.Content to load your game content here
        }
        protected void LoadStage(int stageNum)
        {
            manager.getEntities().Clear();
            manager.getSprites().Clear();
            manager.getCollides().Clear();
            manager.getPlayers().Clear();
            manager.getProjectiles().Clear();
            manager.getShackles().Clear();
            manager.getPatrols().Clear();
            currentStage = stageNum;
            stages[currentStage].LoadContent(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            consys.Update(manager);
            projsys.Update(manager, GraphicsDevice);
            pathsys.Update(manager);

            if (colsys.Update(manager))
            {
                currentStage = (currentStage + 1) % 3;
                LoadStage(currentStage);
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
            stages[currentStage].Draw(spriteBatch, GraphicsDevice);
            Dictionary<int, Collide> collides = manager.getCollides();
            foreach(KeyValuePair<int, Sprite> sp in manager.getSprites()) {
                spriteBatch.Draw(sp.Value.sprite, collides[sp.Value.entityID].hitbox, Color.White);
            }
            //DrawLine(spriteBatch, new Vector2(200, 200), new Vector2(100, 100));
            DrawShackle();
            //spriteBatch.Draw(manager.getSprites()[manager.playerID].sprite, manager.getCollides()[manager.playerID].hitbox, Color.White);
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void DrawShackle()
        {
            foreach (KeyValuePair<int, Shackle> shackle in manager.getShackles())
            {
                Rectangle rectangle1 = manager.getCollides()[shackle.Value.firstPointID].hitbox;
                Rectangle rectangle2 = manager.getCollides()[shackle.Value.firstPointID].hitbox;
                Vector2 start = new Vector2(rectangle1.X + (1/2) * rectangle1.Width, rectangle1.Y + (1/2) * rectangle1.Height);
                Vector2 end = new Vector2(rectangle2.X + (1/2) * rectangle1.Width, rectangle2.Y + (1/2) * rectangle2.Height);
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
                    1), //width of line, change this to make thicker line
                null,
                Color.Black, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}

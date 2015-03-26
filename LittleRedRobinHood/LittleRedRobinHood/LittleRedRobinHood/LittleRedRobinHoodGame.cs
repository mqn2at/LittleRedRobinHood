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
        int currentStage = 1;
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

            this.manager = new ComponentManager();

            //Create stages
            this.stages = new List<Stage>();
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
            //Load the current stage
            stages[currentStage].LoadContent(this.Content);
            // TODO: use this.Content to load your game content here
        }
        protected void LoadStage(int stageNum)
        {
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

            // TODO: Add your update logic here

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
            stages[currentStage].Draw(spriteBatch, this.GraphicsDevice);
            //spriteBatch.Draw(manager.getSprites()[manager.playerID].sprite, manager.getCollides()[manager.playerID].hitbox, Color.White);
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

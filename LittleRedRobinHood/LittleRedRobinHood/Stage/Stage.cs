using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace LittleRedRobinHood
{
    class Stage
    {
        //private List<Entity> entityList;
        private int width;
        private int height;
        private Texture2D background;
        private string backgroundImage;
        private SpriteBatch sb;
        public Stage(String image) {
            backgroundImage = image;
        }
        public int getWidth()
        {
            return this.width;
        }
        public int getHeight()
        {
            return this.height;
        }
        protected override void LoadContent(ContentManager content, GraphicsDevice gd)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sb = new SpriteBatch(gd);

            // This is the code we added earlier.
            background = content.Load<Texture2D>(backgroundImage);
        }
        
    }
}

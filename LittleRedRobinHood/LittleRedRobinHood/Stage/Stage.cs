using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Squared.Tiled;
using System.IO;
using Microsoft.Xna.Framework;

namespace LittleRedRobinHood
{
    class Stage
    {
        //private List<Entity> entityList;
        private int width;
        private int height;
        private Texture2D background;
        private String tiled;
        int num_Obj;
        //private SpriteBatch sb;
        Map map;
        public Stage(String til) {
            tiled = til;
            
        }
        public int getWidth()
        {
            return this.width;
        }
        public int getHeight()
        {
            return this.height;
        }
        public void LoadContent(ContentManager content)
        {

           // map = Map.Load(Path.Combine(content.RootDirectory, "mytest.tmx"), content);
            map = Map.Load(Path.Combine(content.RootDirectory, tiled), content);
            width = map.Width;
            height = map.Height;
        }
        public void Draw(SpriteBatch sb, GraphicsDevice gd)
        {
            map.Draw(sb, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), new Vector2());
        }
        
    }
}

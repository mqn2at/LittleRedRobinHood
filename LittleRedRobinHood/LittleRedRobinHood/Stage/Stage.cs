using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Squared.Tiled;
using System.IO;

namespace LittleRedRobinHood
{
    class Stage
    {
        //private List<Entity> entityList;
        private int width;
        private int height;
        private Texture2D background;
        private String tiled;
        private SpriteBatch sb;
        Map map;
        public Stage(String til, int wid, int hei) {
            tiled = til;
            width = wid;
            height = hei;
        }
        public int getWidth()
        {
            return this.width;
        }
        public int getHeight()
        {
            return this.height;
        }
        protected override void LoadContent()
        {

            map = Map.Load(Path.Combine(Content.RootDirectory, "mytest.tmx"), Content);
           // map = Map.Load(Path.Combine(Content.RootDirectory, tiled), Content);

        }
        
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Squared.Tiled;
using System.IO;
using Microsoft.Xna.Framework;
using LittleRedRobinHood.Component;

namespace LittleRedRobinHood
{
    class Stage
    {
        //private List<Entity> entityList;
        private ComponentManager cm;
        private SortedList<string, Squared.Tiled.Object> obstacles;
        private Squared.Tiled.Object start;
        private Squared.Tiled.Object finish;
        private int width;
        private int height;
        private Texture2D background;
        private String tmxFile;
        int num_Obj;
        Map map;

        public Stage(String tmxFile, ComponentManager cm) {
            this.tmxFile = tmxFile;
            this.cm = cm;
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
            map = Map.Load(Path.Combine(content.RootDirectory, tmxFile), content);
            width = map.Width;
            height = map.Height;
            int tempID;

            //Add all collidable obstacles
            obstacles = map.ObjectGroups["obstacles"].Objects;
            foreach (Squared.Tiled.Object o in obstacles.Values)
            {
                tempID = cm.addEntity();
                cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), false, false);
            }

            //Add start and finish points
            start = map.ObjectGroups["startFinish"].Objects["start"];
            //Change this to add the player and player sprite at this object's coordinates!
            tempID = cm.addEntity();
            cm.addPlayer(tempID);
            start.Texture = content.Load<Texture2D>("Sprite-Soda.png");
        }

        public void Draw(SpriteBatch sb, GraphicsDevice gd)
        {
            //Draw the map / background
            map.Draw(sb, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), new Vector2());
        }
        
    }
}

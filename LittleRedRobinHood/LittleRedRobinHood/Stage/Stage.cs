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
        private SortedList<string, Squared.Tiled.Object> shackleables;
        private SortedList<string, Squared.Tiled.Object> enemies;
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

            //Add shackleable objects
            shackleables = map.ObjectGroups["shackleables"].Objects;
            foreach (Squared.Tiled.Object o in shackleables.Values)
            {
                tempID = cm.addEntity();
                cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), false, true);
            }

            //Add all enemies
            enemies = map.ObjectGroups["enemies"].Objects;
            int enemycount = 0;
            foreach (Squared.Tiled.Object o in enemies.Values)
            {
                tempID = cm.addEntity();
                //HARD CODED ENEMY MOVEMENTS IN, need to change later based on stage
                List<Vector2> waypoints = new List<Vector2>();
                if (enemycount == 0)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(o.X, o.Y - 100));
                    waypoints.Add(new Vector2(o.X, o.Y));
                    waypoints.Add(new Vector2(o.X +100 , o.Y-100));

                }
                if (enemycount == 1)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(o.X, o.Y + 250));
                    waypoints.Add(new Vector2(o.X, o.Y));
                }
                if (enemycount == 2)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true, true);
                    waypoints.Add(new Vector2(o.X+200, o.Y));
                    waypoints.Add(new Vector2(o.X, o.Y));
                }
                /*List<Vector2> waypoints = new List<Vector2>();
                waypoints.Add(new Vector2(o.X+200, o.Y));
                waypoints.Add(new Vector2(o.X, o.Y+200));
                waypoints.Add(new Vector2(o.X, o.Y));*/

                cm.addPatrol(tempID, waypoints, 3);
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("birdsheet.png"), true);
                enemycount++;
            }

            //Add player at start
            start = map.ObjectGroups["startFinish"].Objects["start"];
            tempID = cm.addEntity();
            cm.addPlayer(tempID);
            cm.addCollide(tempID, new Rectangle(start.X, start.Y, start.Width, start.Height), false, false);
            cm.addSprite(tempID, 64, 64, content.Load<Texture2D>("Player_Sprites.png"), true);
            
            //Add finish collidable
            finish = map.ObjectGroups["startFinish"].Objects["finish"];
            tempID = cm.addEntity();
            cm.addCollide(tempID, new Rectangle(finish.X, finish.Y, finish.Width, finish.Height), false, false, false, true);
        }

        public void Draw(SpriteBatch sb, GraphicsDevice gd)
        {
            //Draw the map / background
            map.Draw(sb, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), new Vector2());
        }
        
    }
}

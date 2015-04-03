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
            background = content.Load<Texture2D>("Gothic-Forest_Clip_Art.png");
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

            int shackleBoxID = 0;
            //Add shackleable objects
            shackleables = map.ObjectGroups["shackleables"].Objects;
            foreach (Squared.Tiled.Object o in shackleables.Values)
            {
                tempID = cm.addEntity();
                cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), false, true);
                shackleBoxID = tempID;
            }

            //Add all enemies
            enemies = map.ObjectGroups["enemies"].Objects;
            int enemycount = 0;
            foreach (Squared.Tiled.Object o in enemies.Values)
            {
                tempID = cm.addEntity();
                //HARD CODED ENEMY MOVEMENTS IN, need to change later based on stage
                List<Vector2> waypoints = new List<Vector2>();
                if (tmxFile.Equals("stage2.tmx")) {
                if (enemycount == 0)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(o.X, o.Y - 100));
                    waypoints.Add(new Vector2(o.X, o.Y));
                    waypoints.Add(new Vector2(o.X +100 , o.Y-100));

                }
                else if (enemycount == 1)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(o.X, o.Y + 250));
                    waypoints.Add(new Vector2(o.X, o.Y));
                }
                else if (enemycount == 2)
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
                }
                else if (tmxFile.Equals("stage3.tmx")) {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(o.X, o.Y - 100));
                    waypoints.Add(new Vector2(o.X, o.Y));

                    cm.addPatrol(tempID, waypoints, 2);
                }
                else if (tmxFile.Equals("stage4.tmx"))
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(o.X, o.Y - 100));
                    waypoints.Add(new Vector2(o.X, o.Y));

                    cm.addPatrol(tempID, waypoints, 2);

                    int shackleTemp = cm.addEntity();
                    cm.addShackle(shackleTemp, tempID, shackleBoxID);
                }
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("birdsheet.png"), true);
                enemycount++;
            }

            //Add player at start
            start = map.ObjectGroups["startFinish"].Objects["start"];
            tempID = cm.addEntity();
            cm.addPlayer(tempID);
            cm.addCollide(tempID, new Rectangle(start.X, start.Y, start.Width, start.Height), false, false);
            cm.addSprite(tempID, start.Width, start.Height, content.Load<Texture2D>("BBHood.png"), true);
            
            //Add finish collidable
            finish = map.ObjectGroups["startFinish"].Objects["finish"];
            tempID = cm.addEntity();
            cm.addCollide(tempID, new Rectangle(finish.X, finish.Y, finish.Width, finish.Height), false, false, false, true);
        }

        public void Draw(SpriteBatch sb, GraphicsDevice gd)
        {
            //HARDCODED DRAWING BACKGROUND IMAGE
            sb.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            //Draw the map
            map.Draw(sb, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), new Vector2());
        }
        
    }
}

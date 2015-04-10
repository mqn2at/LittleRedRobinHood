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
        private SortedList<string, Squared.Tiled.Object> platforms;
        private SortedList<string, Squared.Tiled.Object> shackleables;
        private SortedList<string, Squared.Tiled.Object> enemies;
        private SortedList<string, Squared.Tiled.Object> pinecones;
        private Squared.Tiled.Object start;
        private Squared.Tiled.Object finish;
        private int width;
        private int height;
        private List<int> preExistingShackleIDs;
        private Texture2D background;
        private String tmxFile;
        int num_Obj;
        Map map;

        public Stage(String tmxFile, ComponentManager cm) {
            this.tmxFile = tmxFile;
            this.cm = cm;
            this.preExistingShackleIDs = new List<int>();
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

            //Add all collidable platforms
            platforms = map.ObjectGroups["platforms"].Objects;
            int platformNum = 0;
            foreach (Squared.Tiled.Object o in platforms.Values)
            {
                int x = o.X + o.Width / 2;
                int y = o.Y + o.Height / 2;
                tempID = cm.addEntity();
                cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height),true);
                List<Vector2> waypoints = new List<Vector2>();
                if (tmxFile.Equals("stage4.tmx"))
                {
                    if (platformNum % 2 == 0)
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y - 180));
                    }
                    else
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y + 180));
                    }
                }
                else if (tmxFile.Equals("stage9.tmx"))
                {
                    if (platformNum % 2 == 0)
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x + 200, y));
                    }
                    else
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x - 200, y));
                    }
                }
                else if (tmxFile.Equals("stage7.tmx"))
                {
                    waypoints.Add(new Vector2(x, y));
                    waypoints.Add(new Vector2(x, y - 300));
                }
                else
                {
                    waypoints.Add(new Vector2(x, y));
                    waypoints.Add(new Vector2(x, y + 200));
                }
                cm.addPatrol(tempID, waypoints, 3);
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("crate.png"), false);
                platformNum++;
            }

            //int shackleBoxID = 0;
            //Add shackleable objects
            shackleables = map.ObjectGroups["shackleables"].Objects;
            foreach (Squared.Tiled.Object o in shackleables.Values)
            {
                tempID = cm.addEntity();
                cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), false, true);
                preExistingShackleIDs.Add(tempID);
            }
          

            //Add all enemies
            enemies = map.ObjectGroups["enemies"].Objects;
            int enemycount = 0;
            foreach (Squared.Tiled.Object o in enemies.Values)
            {
                int x = o.X + o.Width / 2;
                int y = o.Y + o.Height / 2;
                tempID = cm.addEntity();
                //HARD CODED ENEMY MOVEMENTS IN, need to change later based on stage
                List<Vector2> waypoints = new List<Vector2>();
                if (tmxFile.Equals("stage3.tmx")) {
                if (enemycount == 0)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(x, y - 100));
                    waypoints.Add(new Vector2(x, y));
                    waypoints.Add(new Vector2(x +100 , y-100));

                }
                else if (enemycount == 1)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(x, y + 250));
                    waypoints.Add(new Vector2(x, y));
                }
                else if (enemycount == 2)
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true, true);
                    waypoints.Add(new Vector2(x + 200, y));
                    waypoints.Add(new Vector2(x, y));
                }

                cm.addPatrol(tempID, waypoints, 3);
                }
                else if (tmxFile.Equals("stage5.tmx")) {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(x, y - 100));
                    waypoints.Add(new Vector2(x, y + 20));
                    waypoints.Add(new Vector2(x, y));

                    cm.addPatrol(tempID, waypoints, 2);
                }
                else if (tmxFile.Equals("stage6.tmx"))
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true, 1, false);
                    waypoints.Add(new Vector2(x, y + 200));
                    waypoints.Add(new Vector2(x, y));

                    cm.addPatrol(tempID, waypoints, 2);

                    //Connect pre-existing shackles
                    int shackleTemp = cm.addEntity();
                    cm.addShackle(shackleTemp, tempID, preExistingShackleIDs[enemycount],false);
                    //Calculate hitbox for shackle
                    Rectangle tempRect1 = cm.getCollides()[tempID].hitbox;
                    Rectangle tempRect2 = cm.getCollides()[preExistingShackleIDs[enemycount]].hitbox;
                    int rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                    int rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                    int rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                    int rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                    //Add collide for shackle
                    cm.addCollide(shackleTemp, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, true);
                }
                else if (tmxFile.Equals("stage7.tmx"))
                {
                    if (enemycount == 0)
                    {
                        cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                        waypoints.Add(new Vector2(o.X, o.Y - 100));
                        waypoints.Add(new Vector2(o.X, o.Y));

                    }
                    else if (enemycount == 1)
                    {
                        cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                        waypoints.Add(new Vector2(o.X, o.Y + 100));
                        waypoints.Add(new Vector2(o.X, o.Y));
                    }
                    else if (enemycount == 2)
                    {
                        cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true, true);
                        waypoints.Add(new Vector2(o.X - 200, o.Y));
                        waypoints.Add(new Vector2(o.X, o.Y));
                    }

                    cm.addPatrol(tempID, waypoints, 3);
                }
                else if (tmxFile.Equals("stage8.tmx"))
                {
                    cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, true);
                    waypoints.Add(new Vector2(o.X, o.Y + 200));
                    waypoints.Add(new Vector2(o.X, o.Y));

                    cm.addPatrol(tempID, waypoints, 2);
                }
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("birdsheet.png"), true);
                enemycount++;
            }

            //Add all PINECONES
            pinecones = map.ObjectGroups["pinecones"].Objects;
            foreach (Squared.Tiled.Object o in pinecones.Values)
            {
                tempID = cm.addEntity();
                cm.addCollide(tempID, new Rectangle(o.X, o.Y, o.Width, o.Height), true, false,false,false);
                List<Vector2> waypoints = new List<Vector2>();
                
                waypoints.Add(new Vector2(o.X, o.Y));
                waypoints.Add(new Vector2(o.X, o.Y + 400));
                cm.addPatrol(tempID, waypoints, 4, false);
                //cm.addProjectile(tempID, true, Math.PI / 2, 2);
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("pinecone.png"), false);
            }
            
            //Add player at start
            start = map.ObjectGroups["startFinish"].Objects["start"];
            tempID = cm.addEntity();
            cm.addPlayer(tempID);
            cm.addCollide(tempID, new Rectangle(start.X, start.Y, start.Width, start.Height), false, false);
            cm.addSprite(tempID, start.Width, start.Height, content.Load<Texture2D>("BBHood.png"), true);
            
            //add tutorial text
            if (tmxFile.Equals("stage1.tmx"))
            {
                    int temp = cm.addEntity();
                    cm.addText(temp, cm.font, new Vector2(100, 10), "Press A or D to move.", false);
                    cm.addCollide(temp, new Rectangle(start.X, start.Y, start.Width, start.Height), false);
                    temp = cm.addEntity();
                    cm.addText(temp, cm.font, new Vector2(100, 35), "Press W to jump.", false);
                    cm.addCollide(temp, new Rectangle(start.X + 100, start.Y, start.Width, start.Height), false);
                    temp = cm.addEntity();
                    cm.addText(temp, cm.font, new Vector2(100, 60), "Press P to pause a level.", false);
                    cm.addCollide(temp, new Rectangle(start.X + 500, start.Y - 100, start.Width, start.Height * 5), false);
                    temp = cm.addEntity();
                    cm.addText(temp, cm.font, new Vector2(100, 85), "Press R to reset a level.", false);
                    cm.addCollide(temp, new Rectangle(start.X + 500, start.Y - 100, start.Width, start.Height * 5), false);
            }
            if (tmxFile.Equals("stage2.tmx"))
            {
                int temp = cm.addEntity();
                cm.addText(temp, cm.font, new Vector2(100, 10), "Left click to a shoot a shackle. Shackles will create a platform between two objects.", false);
                cm.addCollide(temp, new Rectangle(start.X, start.Y, start.Width, start.Height), false);
                temp = cm.addEntity();
                cm.addText(temp, cm.font, new Vector2(100, 35), "Right click to shoot an arrow. Arrows destroy shackle platforms and some enemies.", false);
                cm.addCollide(temp, new Rectangle(start.X + 350, start.Y - 300, start.Width, start.Height * 10), false);
                temp = cm.addEntity();
                cm.addText(temp, cm.font, new Vector2(100, 60), "Note that you can only use 3 shackles and 3 arrows at a time.", false);
                cm.addCollide(temp, new Rectangle(start.X + 500, start.Y - 400, start.Width * 2, start.Height * 10), false);
                temp = cm.addEntity();
                cm.addText(temp, cm.font, new Vector2(100, 85), "You also have only 3 lives for the entire game.", false);
                cm.addCollide(temp, new Rectangle(start.X + 500, start.Y - 400, start.Width, start.Height * 10), false);
            }

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

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

        public void LoadContent(ContentManager content, int stageNum)
        {
            map = Map.Load(Path.Combine(content.RootDirectory, tmxFile), content);
            
            //background = content.Load<Texture2D>("background" + ((stageNum / 3) + 1) + ".jpg");
            background = content.Load<Texture2D>("background1.jpg");
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
                Rectangle crateHitBox = new Rectangle(o.X, o.Y, o.Width, o.Height);
                cm.addCollide(tempID, crateHitBox,true);
                List<Vector2> waypoints = new List<Vector2>();
                if (tmxFile.Equals("stage4.tmx"))
                {
                    waypoints.Add(new Vector2(x, y));
                    waypoints.Add(new Vector2(x, y - 300));
                    cm.addPatrol(tempID, waypoints, 3);
                }
                else if (tmxFile.Equals("stage6.tmx"))
                {
                    waypoints.Add(new Vector2(x, y));
                    waypoints.Add(new Vector2(x, y + 350));
                    cm.addPatrol(tempID, waypoints, 3);
                }
                else if (tmxFile.Equals("stage9.tmx"))
                {
                    if (platformNum % 2 == 0)
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x + 200, y));
                        cm.addPatrol(tempID, waypoints, 2);
                    }
                    else
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x - 200, y));
                        cm.addPatrol(tempID, waypoints, 2);
                    }
                }
                else if (tmxFile.Equals("stage10.tmx"))
                {
                    if (platformNum ==0)
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x + 168, y));
                        cm.addPatrol(tempID, waypoints, 2);
                    }
                    else if (platformNum == 1)
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x - 132, y));
                        cm.addPatrol(tempID, waypoints, 2);
                    }
                    else if (platformNum == 2)
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x + 168, y));
                        cm.addPatrol(tempID, waypoints, 2);
                    }
                    else
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x - 200, y));
                        cm.addPatrol(tempID, waypoints, 2);
                    }
                }
                else if (tmxFile.Equals("stage12.tmx")) {
                    waypoints.Add(new Vector2(x, y));
                    waypoints.Add(new Vector2(x + 100, y));
                    cm.addPatrol(tempID, waypoints, 3);
                }
                else
                {
                    waypoints.Add(new Vector2(x, y));
                    waypoints.Add(new Vector2(x, y + 300));
                    cm.addPatrol(tempID, waypoints, 3);
                }
                
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("crate.png"), crateHitBox, false);
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
          

            //ADD ENEMIES
            enemies = map.ObjectGroups["enemies"].Objects;
            int enemy0ID = 0;
            int enemy1ID = 0; 
            int enemy2ID = 0;
            int enemy3ID = 0;
            int enemycount = 0;
            foreach (Squared.Tiled.Object o in enemies.Values)
            {
                int x = o.X + o.Width / 2;
                int y = o.Y + o.Height / 2;
                tempID = cm.addEntity();
                //HARD CODED ENEMY MOVEMENTS IN, need to change later based on stage
                List<Vector2> waypoints = new List<Vector2>();
                Rectangle enemySpriteBox = new Rectangle(o.X, o.Y, o.Width, o.Height);
                Rectangle enemyHitBox = new Rectangle(o.X, o.Y, o.Width, o.Height);
                if (tmxFile.Equals("stage3.tmx")) {
                    if (enemycount == 0)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y - 100));
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x +100 , y-100));

                    }
                    else if (enemycount == 1)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y + 230));
                        waypoints.Add(new Vector2(x, y));
                    }
                    else if (enemycount == 2)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true, true);
                        waypoints.Add(new Vector2(x + 200, y));
                        waypoints.Add(new Vector2(x, y));
                    }

                    cm.addPatrol(tempID, waypoints, 3);
                }
                else if (tmxFile.Equals("stage4.tmx"))
                {
                    cm.addCollide(tempID, enemyHitBox, true, true);
                    waypoints.Add(new Vector2(x, y + 250));
                    waypoints.Add(new Vector2(x + 100, y + 250));
                    waypoints.Add(new Vector2(x + 100, y));
                    waypoints.Add(new Vector2(x, y));

                    cm.addPatrol(tempID, waypoints, 2);
                }
                else if (tmxFile.Equals("stage5.tmx"))
                {
                    cm.addCollide(tempID, enemyHitBox, true, true);
                    waypoints.Add(new Vector2(x, y - 100));
                    waypoints.Add(new Vector2(x, y + 20));
                    waypoints.Add(new Vector2(x, y));

                    cm.addPatrol(tempID, waypoints, 2);
                }
                else if (tmxFile.Equals("stage6.tmx"))
                {
                    if (enemycount == 0)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true, 1, false);
                        waypoints.Add(new Vector2(x, y + 300));
                        waypoints.Add(new Vector2(x, y));

                        //Connect pre-existing shackle
                        int shackleTemp = cm.addEntity();
                        cm.addShackle(shackleTemp, tempID, preExistingShackleIDs[enemycount], false);
                        //Calculate hitbox for shackle
                        Rectangle tempRect1 = cm.getCollides()[tempID].hitbox;
                        Rectangle tempRect2 = cm.getCollides()[preExistingShackleIDs[enemycount]].hitbox;
                        int rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                        int rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                        int rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                        int rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                        //Add collide for shackle
                        cm.addCollide(shackleTemp, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);
                    }
                    else if (enemycount == 1)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y - 300));
                        waypoints.Add(new Vector2(x, y));
                    }

                    cm.addPatrol(tempID, waypoints, 2);
                }
                else if (tmxFile.Equals("stage7.tmx"))
                {
                    
                    if (enemycount == 0)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true,2,false);
                        waypoints.Add(new Vector2(x, y - 100));
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y + 80));

                        //Connect pre-existing shackle
                        int shackleTemp = cm.addEntity();
                        cm.addShackle(shackleTemp, tempID, preExistingShackleIDs[0], false);
                        //Calculate hitbox for shackle
                        Rectangle tempRect1 = cm.getCollides()[tempID].hitbox;
                        Rectangle tempRect2 = cm.getCollides()[preExistingShackleIDs[0]].hitbox;
                        int rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                        int rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                        int rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                        int rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                        //Add collide for shackle
                        cm.addCollide(shackleTemp, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);

                        enemy0ID = tempID;
                    }
                    else if (enemycount == 1)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true, 2, false);
                        waypoints.Add(new Vector2(x + 100, y - 200));
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x - 50, y + 100));
                        enemy1ID = tempID;
                    }
                    else if (enemycount == 2)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true, 2, false);
                        waypoints.Add(new Vector2(x -30, y - 30));
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x+30, y + 30));
                        enemy2ID = tempID;
                    }
                    else if (enemycount == 3)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true, 2, false);
                        waypoints.Add(new Vector2(x, y + 150));
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y - 80));

                        //Connect pre-existing shackle
                        int shackleTemp = cm.addEntity();
                        cm.addShackle(shackleTemp, tempID, preExistingShackleIDs[1], false);
                        //Calculate hitbox for shackle
                        Rectangle tempRect1 = cm.getCollides()[tempID].hitbox;
                        Rectangle tempRect2 = cm.getCollides()[preExistingShackleIDs[1]].hitbox;
                        int rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                        int rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                        int rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                        int rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                        //Add collide for shackle
                        cm.addCollide(shackleTemp, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);
                        enemy3ID = tempID;
                    }
                    cm.addPatrol(tempID, waypoints, 2);


                }
                else if (tmxFile.Equals("stage8.tmx"))
                {
                    if (enemycount == 0)
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y - 100));
                        waypoints.Add(new Vector2(x, y));
                    }
                    else
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y + 100));
                        waypoints.Add(new Vector2(x, y));
                    }

                    cm.addPatrol(tempID, waypoints, 2);
                }
                else if (tmxFile.Equals("stage12.tmx"))
                {
                    if (o.Name.Equals("enemy1"))
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y + 100));
                        waypoints.Add(new Vector2(x, y));
                    }
                    else if (o.Name.Equals("enemy3"))
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y + 150));
                        waypoints.Add(new Vector2(x, y));
                    }
                    else if (o.Name.Equals("enemy2"))
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x, y - 100));
                        waypoints.Add(new Vector2(x, y));
                    }
                    else if (o.Name.Equals("enemy14"))
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                        waypoints.Add(new Vector2(x+500, y));
                        waypoints.Add(new Vector2(x, y));
                    }
                    else
                    {
                        cm.addCollide(tempID, enemyHitBox, true, true);
                    }

                    cm.addPatrol(tempID, waypoints, 2);
                }
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("birdsheet.png"), enemySpriteBox, true);
                enemycount++;
            }
            //Add shackles between birbs
            if (tmxFile.Equals("stage7.tmx"))
            {
                //Connect pre-existing shackle 1
                int shackleTemp = cm.addEntity();
                cm.addShackle(shackleTemp, enemy0ID, enemy1ID, false);
                //Calculate hitbox for shackle
                Rectangle tempRect1 = cm.getCollides()[enemy0ID].hitbox;
                Rectangle tempRect2 = cm.getCollides()[enemy1ID].hitbox;
                int rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                int rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                int rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                int rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                //Add collide for shackle
                cm.addCollide(shackleTemp, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);

                //Connect pre-existing shackle 2
                shackleTemp = cm.addEntity();
                cm.addShackle(shackleTemp, enemy1ID, enemy2ID, false);
                //Calculate hitbox for shackle
                tempRect1 = cm.getCollides()[enemy1ID].hitbox;
                tempRect2 = cm.getCollides()[enemy2ID].hitbox;
                rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                //Add collide for shackle
                cm.addCollide(shackleTemp, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);

                //Connect pre-existing shackle 2
                shackleTemp = cm.addEntity();
                cm.addShackle(shackleTemp, enemy2ID, enemy3ID, false);
                //Calculate hitbox for shackle
                tempRect1 = cm.getCollides()[enemy2ID].hitbox;
                tempRect2 = cm.getCollides()[enemy3ID].hitbox;
                rectX = Math.Min(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0));
                rectY = Math.Min(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0));
                rectWidth = Math.Max(tempRect1.X + (int)(tempRect1.Width / 2.0), tempRect2.X + (int)(tempRect2.Width / 2.0)) - rectX;
                rectHeight = Math.Max(tempRect1.Y + (int)(tempRect1.Height / 2.0), tempRect2.Y + (int)(tempRect2.Height / 2.0)) - rectY;
                //Add collide for shackle
                cm.addCollide(shackleTemp, new Rectangle(rectX, rectY, rectWidth, rectHeight), false, false);
            }

            //Add all PINECONES
            pinecones = map.ObjectGroups["pinecones"].Objects;
            int pineconeCount = 0;
            foreach (Squared.Tiled.Object o in pinecones.Values)
            {
                int x = o.X + o.Width / 2;
                int y = o.Y + o.Height / 2;
                tempID = cm.addEntity();
                Rectangle pineconeHitBox = new Rectangle(o.X, o.Y, o.Width, o.Height);
                Rectangle pineconeSpriteBox = new Rectangle(o.X, o.Y, o.Width, o.Height);
                cm.addCollide(tempID, pineconeHitBox, true, false,false,false);
                List<Vector2> waypoints = new List<Vector2>();
                if (tmxFile.Equals("stage12.tmx"))
                {
                    if (pineconeCount == 0) {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y + 300));
                        cm.addPatrol(tempID, waypoints, 2, false);
                    }
                    else {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y + 240));
                        cm.addPatrol(tempID, waypoints, 3, false);
                    }
                }
                else
                {
                    if (pineconeCount % 2 == 0)
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y + 500));
                        cm.addPatrol(tempID, waypoints, 4, false);
                    }
                    else
                    {
                        waypoints.Add(new Vector2(x, y));
                        waypoints.Add(new Vector2(x, y + 600));
                        cm.addPatrol(tempID, waypoints, 4, false);
                    }
                }
                cm.addProjectile(tempID, true, Math.PI / 2, 1, o.X, o.Y);
                cm.addSprite(tempID, o.Width, o.Height, content.Load<Texture2D>("pinecone.png"), pineconeSpriteBox, false);
                pineconeCount++;
            }
            
            //Add player at start
            start = map.ObjectGroups["startFinish"].Objects["start"];
            tempID = cm.addEntity();
            cm.addPlayer(tempID);
            Rectangle playerHitBox = new Rectangle(start.X+10, start.Y, 40, 62); //Change dis
            Rectangle playerSpriteBox = new Rectangle(start.X, start.Y, 60, 62);
            cm.addCollide(tempID, playerHitBox, false, false);
            cm.addSprite(tempID, 60, 62, content.Load<Texture2D>("BBHood.png"), playerSpriteBox, true);
            
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
            
            //Draw the map
            map.Draw(sb, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), new Vector2());
        }

        public void DrawBackground(SpriteBatch sb, GraphicsDevice gd)
        {
            //HARDCODED DRAWING BACKGROUND IMAGE
            sb.Draw(background, new Rectangle(0, 0, 800, 540), Color.White);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleRedRobinHood.Component;
using LittleRedRobinHood.Entities;
using LittleRedRobinHood.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LittleRedRobinHood
{
    class ComponentManager
    {
        private Dictionary<int, Entity> entities;
        private Dictionary<int, Sprite> sprites;
        private Dictionary<int, Collide> collides;
        private Dictionary<int, Player> players;
        private Dictionary<int, Projectile> projectiles;
        private Dictionary<int, Shackle> shacklePlatforms;
        private Dictionary<int, Patrol> patrols;
        private Dictionary<int, Text> texts;
        public SoundSystem soundsys;
        private Rectangle healthBox, arrowBox, shackleBox; //for UI drawing
        private Texture2D healthSprite, arrowSprite, shackleSprite;
        private int maxID;
        public int playerID; // GET RID OF LATER
        public int selectID; // ALSO GET RID OF LATER
        public int numStages; //YET ANOTHER FOR THE CHOPPING BOARD
        public ContentManager conman;
        public ComponentManager(ContentManager cm)
        {
            this.entities = new Dictionary<int, Entity>();
            this.sprites = new Dictionary<int, Sprite>();
            this.collides = new Dictionary<int, Collide>();
            this.players = new Dictionary<int, Player>();
            this.projectiles = new Dictionary<int, Projectile>();
            this.shacklePlatforms = new Dictionary<int, Shackle>();
            this.patrols = new Dictionary<int, Patrol>();
            this.texts = new Dictionary<int, Text>();
            soundsys = new SoundSystem();
            soundsys.LoadContent(cm);
            this.maxID = 0;
            this.conman = cm;
            //adjust UI position here
            this.healthBox = new Rectangle(10, 10, 25, 25);
            this.shackleBox = new Rectangle(10, 40, 25, 25);
            this.arrowBox = new Rectangle(10, 70, 25, 25);
            //adjust UI images here
            this.healthSprite = conman.Load<Texture2D>("heart.png");
            this.arrowSprite = conman.Load<Texture2D>("arrow.png");
            this.shackleSprite = conman.Load<Texture2D>("rope.png");
        }

        public void clearDictionaries()
        {
            this.entities.Clear();
            this.sprites.Clear();
            this.collides.Clear();
            this.players.Clear();
            this.projectiles.Clear();
            this.shacklePlatforms.Clear();
            this.patrols.Clear();
            this.texts.Clear();
            this.maxID = 0;
        }

        public Dictionary<int, Entity> getEntities()
        {
            return this.entities;
        }

        public Dictionary<int, Sprite> getSprites()
        {
            return this.sprites;
        }

        public Dictionary<int, Collide> getCollides()
        {
            return this.collides;
        }

        public Dictionary<int, Player> getPlayers()
        {
            return this.players;
        }
        public Dictionary<int, Projectile> getProjectiles()
        {
            return this.projectiles;
        }
        public Dictionary<int, Shackle> getShackles()
        {
            return this.shacklePlatforms;
        }
        public Dictionary<int, Patrol> getPatrols()
        {
            return this.patrols;
        }
        public Dictionary<int, Text> getTexts()
        {
            return this.texts;
        }

        public int addEntity()
        {
            Entity temp = new Entity(maxID);
            entities.Add(temp.entityID, temp);
            maxID++;
            return temp.entityID;
        }

        public void addPlayer(int id)
        {
            entities[id].isPlayer = true;
            Player temp = new Player(id);
            players.Add(id, temp);
            this.playerID = id;
        }

        public void setSelect(int id)
        {
            this.selectID = id;
        }

        public void persistLives(int life)
        {
            Player temp = players[this.playerID];
            temp.lives = life;
        }

        public int currentLives()
        {
            return this.players[this.playerID].lives;
        }

        public void addCollide(int id, Rectangle hb, bool enemy, bool shackle)
        {
            Collide temp = new Collide(id, hb, enemy, shackle);
            collides.Add(id, temp);
            entities[id].isCollide = true;
        }
        public void addCollide(int id, Rectangle hb, bool enemy, bool shackle, bool damageable)
        {
            Collide temp = new Collide(id, hb, enemy, shackle, 0,damageable);
            collides.Add(id, temp);
            entities[id].isCollide = true;
        }
        public void addCollide(int id, Rectangle hb, bool enemy, bool shackle, bool damageable, bool endpoint)
        {
            Collide temp = new Collide(id, hb, enemy, shackle, 0,damageable,endpoint);
            collides.Add(id, temp);
            entities[id].isCollide = true;
        }

        public void addCollide(int id, Rectangle hb, bool enemy, bool shackle, int shackled, bool damageable)
        {
            Collide temp = new Collide(id, hb, enemy, shackle, shackled, damageable);
            collides.Add(id, temp);
            entities[id].isCollide = true;
        }

        public void addCollide(int id, Rectangle hb, bool platform)
        {
            Collide temp = new Collide(id, hb, platform);
            collides.Add(id, temp);
            entities[id].isCollide = true;
        }

        public void addSprite(int id, int width, int height, Texture2D sprite)
        {
            Sprite temp = new Sprite(id, width, height, sprite);
            sprites.Add(id, temp);
        }
        public void addSprite(int id, int width, int height, Texture2D sprite, bool animated)
        {
            Sprite temp = new Sprite(id, width, height, sprite, animated);
            sprites.Add(id, temp);
        }

        public void addProjectile(int id, bool isArrow, double angle, int speed)
        {
            Projectile temp = new Projectile(isArrow, angle, speed);
            projectiles.Add(id, temp);
            entities[id].isProjectile = true;
        }

        public void addShackle(int id, int fpID, int spID){
            Shackle temp = new Shackle(id, fpID, spID);
            shacklePlatforms.Add(id, temp);
            entities[id].isShackle = true;
        }
        public void addShackle(int id, int fpID, int spID, bool playerMade)
        {
            Shackle temp = new Shackle(id, fpID, spID, playerMade);
            shacklePlatforms.Add(id, temp);
            entities[id].isShackle = true;
        }


        public void addPatrol(int id, List<Vector2> path, int spd)
        {
            Patrol temp = new Patrol(id, path, spd);
            patrols.Add(id, temp);
            entities[id].isPatrol = true;
        }

        public void addPatrol(int id, List<Vector2> path, int spd, bool cycle)
        {
            Patrol temp = new Patrol(id, path, spd, cycle);
            patrols.Add(id, temp);
            entities[id].isPatrol = true;
        }

        public void addText(int id, SpriteFont font, Vector2 trigger, Vector2 position, string text)
        {
            Text temp = new Text(font, trigger, position, text);
            texts.Add(id, temp);
            entities[id].isText = true;
        }

        public void addText(int id, SpriteFont font, Vector2 trigger, Vector2 position, string text, bool vis)
        {
            Text temp = new Text(font, trigger, position, text, vis);
            texts.Add(id, temp);
            entities[id].isText = true;
        }

        public void addText(int id, SpriteFont font, Vector2 trigger, Vector2 position, string text, bool vis, Single scale)
        {
            Text temp = new Text(font, trigger, position, text, vis, scale);
            texts.Add(id, temp);
            entities[id].isText = true;
        }

        public Rectangle getHealthBox()
        {
            return this.healthBox;
        }

        public Rectangle getArrowsBox()
        {
            return this.arrowBox;
        }

        public Rectangle getShackleBox()
        {
            return this.shackleBox;
        }

        public Texture2D getHealthSprite()
        {
            return this.healthSprite;
        }

        public Texture2D getArrowSprite()
        {
            return this.arrowSprite;
        }

        public Texture2D getShackleSprite()
        {
            return this.shackleSprite;
        }
    }
}

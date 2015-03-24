using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleRedRobinHood.Component;
using LittleRedRobinHood.Entities;
using LittleRedRobinHood.System;
using Microsoft.Xna.Framework.Graphics;

namespace LittleRedRobinHood
{
    class ComponentManager
    {
        private Dictionary<int, Entity> entities;
        private Dictionary<int, Sprite> sprites;
        private Dictionary<int, Collide> collides;
        private Dictionary<int, Player> players;
        private int maxID;
        int playerID { get; set; }
        public ComponentManager()
        {
            this.entities = new Dictionary<int, Entity>();
            this.sprites = new Dictionary<int, Sprite>();
            this.collides = new Dictionary<int, Collide>();
            this.players = new Dictionary<int, Player>();
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
        }

        public void addCollide(int id)
        {
            entities[id].isCollide = true;
        }

        public void addSprite(int id, int width, int height, Texture2D sprite)
        {
            Sprite temp = new Sprite(id, width, height, sprite);
            sprites.Add(id, temp);
        }

    }
}

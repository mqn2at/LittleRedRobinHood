using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace LittleRedRobinHood.Component
{
    class Collide
    {
        public readonly int entityID;
        public readonly bool isEnemy;
        public readonly bool isShackleable;
        public readonly bool isEndPoint;
        public bool isShackled;
        public Rectangle hitbox;

        public Collide(int id, Vector2 pos, Rectangle hb, bool enemy, bool shackleable, bool shackled)
        {
            this.entityID = id;
            this.hitbox = hb;
            this.isEnemy = enemy;
            this.isShackleable = shackleable;
            this.isShackled = shackled;
            this.isEndPoint = false;
        }

        public Collide(int id, Vector2 pos, Rectangle hb, bool enemy, bool shackleable, bool shackled, bool endPoint)
        {
            this.entityID = id;
            this.hitbox = hb;
            this.isEnemy = enemy;
            this.isShackleable = shackleable;
            this.isShackled = shackled;
            this.isEndPoint = endPoint;
        }
    }
}

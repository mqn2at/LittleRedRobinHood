using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LittleRedRobinHood.Component
{
    class Sprite
    {
        public readonly int width;
        public readonly int height;
        public readonly Texture2D sprite;
        public readonly int entityID;
        public readonly bool animated;
        public SpriteEffects effect = SpriteEffects.None;
        public Rectangle spriteBox;

        public Sprite(int id, int width, int height, Texture2D sprite, Rectangle box)
        {
            this.entityID = id;
            this.width = width;
            this.height = height;
            this.sprite = sprite;
            this.spriteBox = box;
        }
        //Animated
        public Sprite(int id, int width, int height, Texture2D sprite, Rectangle box, bool animated)
        {
            this.entityID = id;
            this.width = width;
            this.height = height;
            this.sprite = sprite;
            this.spriteBox = box;
            this.animated = animated;
        }
    }
}

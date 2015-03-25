using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LittleRedRobinHood.Component
{
    class Sprite
    {
        public readonly int width;
        public readonly int height;
        public readonly Texture2D sprite;
        public readonly int entityID;
        public readonly bool animated;

        public Sprite(int id, int width, int height, Texture2D sprite)
        {
            this.entityID = id;
            this.width = width;
            this.height = height;
            this.sprite = sprite;
        }
    }
}

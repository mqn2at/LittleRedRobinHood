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

        public Sprite(int width, int height, Texture2D sprite)
        {
            this.width = width;
            this.height = height;
            this.sprite = sprite;
        }
    }
}

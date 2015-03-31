using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LittleRedRobinHood.Component
{
    class Text
    {
        public readonly SpriteFont font;
        public readonly Vector2 triggerPosition;
        public readonly Vector2 textPosition;
        public readonly string text;
        public bool visible;

        public Text(SpriteFont font, Vector2 posTrigger, Vector2 posText, string text)
        {
            this.font = font;
            this.triggerPosition = posTrigger;
            this.textPosition = posText;
            this.text = text;
            visible = false;
        }

        public Text(SpriteFont font, Vector2 posTrigger, Vector2 posText, string text, bool visible)
        {
            this.font = font;
            this.triggerPosition = posTrigger;
            this.textPosition = posText;
            this.text = text;
            this.visible = visible;
        }
    }
}

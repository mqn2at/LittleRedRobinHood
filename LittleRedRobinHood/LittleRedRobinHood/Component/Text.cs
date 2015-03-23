using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LittleRedRobinHood.Component
{
    class Text
    {
        public readonly Vector2 triggerPosition;
        public readonly Vector2 textPosition;
        public readonly string text;
        public bool visible;

        public Text(Vector2 posTrigger, Vector2 posText, string text)
        {
            this.triggerPosition = posTrigger;
            this.textPosition = posText;
            this.text = text;
            visible = false;
        }
    }
}

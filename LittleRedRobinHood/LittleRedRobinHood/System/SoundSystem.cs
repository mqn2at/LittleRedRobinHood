using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LittleRedRobinHood.Component;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace LittleRedRobinHood.System
{
    class SoundSystem
    {
        SoundEffect sound1;
        SoundEffectInstance sound1_inst;

        public void LoadContent(ContentManager content) {
            //initialize all sounds and add them to thing thing
            sound1 = content.Load<SoundEffect>("bow.wav");
            sound1_inst = sound1.CreateInstance();
        }

        public void playBow()
        {
            sound1_inst.Play();
        }

        public void stopBow()
        {
            sound1_inst.Stop();
        }
    }
}
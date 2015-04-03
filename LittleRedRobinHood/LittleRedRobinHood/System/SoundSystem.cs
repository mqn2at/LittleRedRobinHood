using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LittleRedRobinHood.Component;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;



namespace LittleRedRobinHood.System
{
    class SoundSystem
    {
        SoundEffect sound1;
        SoundEffectInstance sound1_inst;
        Song gameSong;
        Song menuSong;
        //SoundEffectInstance gameSong_inst;
        //sMediaPlayer gs;

        public void LoadContent(ContentManager content) {
            //initialize all sounds and add them to thing thing
            sound1 = content.Load<SoundEffect>("bow.wav");
            sound1_inst = sound1.CreateInstance();
            gameSong = content.Load<Song>("DST-3rdBallad.wav");
            menuSong = content.Load<Song>("DST-ALightIntro.wav");
            //gs = new MediaPlayer();
            //gameSong_inst = gameSong.CreateInstance();

        }

        public void playBow()
        {
            sound1_inst.Play();
        }

        public void stopBow()
        {
            sound1_inst.Stop();
        }

        public void playGameSong()
        {
            MediaPlayer.Play(gameSong);
            MediaPlayer.IsRepeating = true;
        }

        public void stopSong()
        {
            MediaPlayer.Stop();
        }

        public void playMenuSong()
        {
            MediaPlayer.Play(menuSong);
            MediaPlayer.IsRepeating = true;
        }
    }
}
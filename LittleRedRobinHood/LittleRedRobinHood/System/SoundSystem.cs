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
        SoundEffect bow_sound_1, menu_song;
        SoundEffectInstance bow_sound_1_inst, menu_song_inst, prev_inst;
        List<SoundEffect> game_songs;
        List<SoundEffectInstance> game_songs_inst;

        //SoundEffectInstance gameSong_inst;
        //sMediaPlayer gs;

        public void LoadContent(ContentManager content) {
            //initialize all sounds and add them to thing thing
            bow_sound_1 = content.Load<SoundEffect>("bow.wav");
            bow_sound_1_inst = bow_sound_1.CreateInstance();
            game_songs = new List<SoundEffect>();
            game_songs_inst = new List<SoundEffectInstance>();
            game_songs.Add(content.Load<SoundEffect>("DST-3rdBallad.wav"));
            for (int x = 0; x < game_songs.Count; x++)
            {
                game_songs_inst.Add(game_songs[x].CreateInstance());
            }
            menu_song = content.Load<SoundEffect>("DST-ALightIntro.wav");
            menu_song_inst = menu_song.CreateInstance();
            prev_inst = menu_song_inst;
            
            //gs = new MediaPlayer();
            //gameSong_inst = gameSong.CreateInstance();

        }

        public void Update(bool mainMenu, int currentStage)
        {
            //Console.WriteLine("checking soundsys: " + MediaPlayer.State);
            
                //Console.WriteLine("state change!");
            if (prev_inst.State == SoundState.Stopped)
            {
                if (mainMenu)
                {
                    prev_inst = menu_song_inst; 
                }
                else
                {
                    prev_inst = game_songs_inst[currentStage / 3];
                }
                prev_inst.Play();
            }
        }

        public void playBow()
        {
            bow_sound_1_inst.Play();
        }

        public void stopBow()
        {
            bow_sound_1_inst.Stop();
        }

        public void playGameSong(int currentStage)
        {
            prev_inst.Stop();
            prev_inst = game_songs_inst[currentStage / 3];
            prev_inst.Play();
        }

        public void stopSong()
        {
            prev_inst.Stop();
        }

        public void playMenuSong()
        {
            prev_inst.Stop();
            prev_inst = menu_song_inst;
            prev_inst.Play();
        }
    }
}
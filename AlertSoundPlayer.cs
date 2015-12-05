// DangerAlerts v1.0.1: A KSP mod. Public domain, do whatever you want, man.
// Author: SpaceIsBig42/Norpo (same person)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using KSP;

namespace DangerAlerts
{
    class AlertSoundPlayer
    {
        public GameObject dangeralertplayer = new GameObject("dangeralertplayer"); //Makes the GameObject
        public AudioSource source; //The source to be added to the object
        public AudioClip loadedClip;

        public void PlaySound()
        {
            source.audio.clip = loadedClip;
            source.audio.Play();
        }
        public void SetVolume(float vol)
        {
            source.audio.volume = vol;
        }
        public void StopSound()
        {
            source.audio.Stop();
        }
        public bool SoundPlaying() //Returns true if sound is playing, otherwise false
        {
            if (source != null)
            {
                return source.isPlaying;
            }
            else
            {
                return false;
            }
        }
        public void Initialize(string soundPath)
        {
            //Initializing stuff;
            dangeralertplayer = new GameObject("dangeralertplayer");
            source = dangeralertplayer.AddComponent<AudioSource>();
            loadedClip = GameDatabase.Instance.GetAudioClip(soundPath);
            Debug.Log("[DNGRALT] Did file stuff.");

            source.volume = 0.5f; //Volume can be changed, and probably should be. Add toolbar volume slider? TODO
            source.panLevel = 0; //Forces the sound to go 2D, to not decay over distance.
            Debug.Log("[DNGRALT] Initialized Danger Alert Player");
        }
        
    }
}

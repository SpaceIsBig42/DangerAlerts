//DangerAlerts v0.1pancake, a KSP mod, public domain

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

        public void PlaySound(Vessel vessel)
        {
            Debug.Log("[DNGRALT] Starting to play alarm");
            
            source.audio.clip = loadedClip;
            source.audio.Play();
            Debug.Log("[DNGRALT] ...Finished");
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
            Debug.Log("Initialized Danger Alert Player");
        }
        public void MovePlayer(Vessel vessel) //Moves the player, maaaaybe not needed?
        {
            dangeralertplayer.transform.parent = vessel.gameObject.transform;
            dangeralertplayer.transform.localPosition = new Vector3(0, 0, 0);
            Debug.Log("[DNGRALT] Moving player");
        }
    }
}

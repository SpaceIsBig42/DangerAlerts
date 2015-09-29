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
        AudioClip currentClip;
        public GameObject dangeralertplayer = new GameObject("dangeralertplayer");
        public AudioSource source;
        public void PlaySound(string soundPath, Vessel vessel)
        {
            Debug.Log("[DNGRALT] Starting to play alarm");
            currentClip = GameDatabase.Instance.GetAudioClip(soundPath);
            Debug.Log("[DNGRALT] Did file stuff.");
            source.audio.clip = currentClip;
            source.audio.Play();
            Debug.Log("[DNGRALT] ...Finished");
        }
        public void Initialize()
        {
            //Initializing stuff;
            dangeralertplayer = new GameObject("dangeralertplayer");
            source = dangeralertplayer.AddComponent<AudioSource>();
            
            source.volume = 1;
            Debug.Log("Initialized Danger Alert Player");
        }
        public void MovePlayer(Vessel vessel)
        {
            dangeralertplayer.transform.parent = vessel.gameObject.transform;
            dangeralertplayer.transform.localPosition = new Vector3(0, 0, 0);
            Debug.Log("[DNGRALT] Moving player");
        }
    }
}

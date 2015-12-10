// DangerAlerts v1.0.1: A KSP mod. Public domain, do whatever you want, man.
// Author: SpaceIsBig42/Norpo (same person)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using KSP;
using System.IO;

namespace DangerAlerts
{
    [KSPAddon(KSPAddon.Startup.Flight, false)] //Starts on flight
    public class DangerAlertCore : MonoBehaviour
    {
        private string normalAlert = "DangerAlerts/Sounds/normalAlert";
        AlertSoundPlayer soundplayer = new AlertSoundPlayer();
        DangerAlertGUI dangerAlertGui;

        private bool inDanger = false;

        public bool alarmActive = false;

        private bool soundActive = true;

        public bool Paused = false;

        private List<AlertBase> Alerts = new List<AlertBase>();

        void Start()
        {
            Debug.Log("[DNGRALT] Danger Alerts started."); //Lets the user know the add-on was started, DEBUG
            Debug.Log("[DNGRALT] Sound file exists: " + GameDatabase.Instance.ExistsAudioClip(normalAlert));
            soundplayer.Initialize(normalAlert); // Initializes the player, does some housekeeping

            DangerAlertSettings.Instance.UpdateFromCfg();

            dangerAlertGui = gameObject.AddComponent<DangerAlertGUI>();

            GameEvents.onGamePause.Add(OnPause);
            GameEvents.onGameUnpause.Add(OnUnpause);

            Alerts.Add(new CollisionAlert(7, 10, -2)); //TEMPORARY DEBUG ALL THAT GOOD STUFF REMOVE REPLACE ETC
        }

        void OnPause()
        {
            Paused = true;
            if (soundplayer.SoundPlaying())
            {
                soundplayer.StopSound();
            }
        }

        void OnUnpause()
        {
            Paused = false;
        }

        

        void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && !Paused)
            {
                Vessel currentVessel = FlightGlobals.ActiveVessel;
                soundActive = dangerAlertGui.soundToggle;
                DangerAlertSettings.Instance.UpdateFromGui(dangerAlertGui);

                soundplayer.SetVolume(DangerAlertSettings.Instance.MasterVolume);

                inDanger = false;
                foreach (AlertBase alert in Alerts)
                {
                    if (alert.Triggered(currentVessel))
                    {
                        if (!alarmActive) //alarmActive is to make it so the plugin doesn't keep spamming sound
                        {
                            alarmActive = true;
                            dangerAlertGui.InDanger(true);
                        }
                        if (!soundplayer.SoundPlaying()) //If the sound isn't playing, play the sound.
                        {
                            if (soundActive)
                            {
                                soundplayer.PlaySound(); //Plays sound
                            }
                        }

                        inDanger = true;
                    }
                }

                if (!inDanger)
                {
                    if (alarmActive)
                    {
                        alarmActive = false;
                        dangerAlertGui.InDanger(false);
                        soundplayer.StopSound();
                    }
                }

            }
        }

        void OnDestroy()
        {
            //When the core class is destroyed, save options to the cfg.
            DangerAlertSettings.Instance.SaveCfg();
        }
    }
}

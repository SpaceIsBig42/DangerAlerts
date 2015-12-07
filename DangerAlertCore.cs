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

        public bool alarmActive = false;

        private bool soundActive = true;

        public bool Paused = false;

        void Start()
        {
            Debug.Log("[DNGRALT] Danger Alerts started."); //Lets the user know the add-on was started, DEBUG
            Debug.Log("[DNGRALT] Sound file exists: " + GameDatabase.Instance.ExistsAudioClip(normalAlert));
            soundplayer.Initialize(normalAlert); // Initializes the player, does some housekeeping

            DangerAlertSettings.Instance.UpdateFromCfg();

            dangerAlertGui = gameObject.AddComponent<DangerAlertGUI>();

            GameEvents.onGamePause.Add(OnPause);
            GameEvents.onGameUnpause.Add(OnUnpause);

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
        bool InDangerOfCrashing(Vessel currentVessel) // Returns a value.
        {
            if (currentVessel.heightFromTerrain > 0)
            //I'd like to talk a bit about the if statement above, because it's totally rad and bonkers.                  //
            //For _some_ reason, KSP decides that once you're past that magical threshold,                                //
            //usually, but not always 50x timewarp minimum height,                                                        //
            //that calculating surface altitude is pointless, so it defaults to *something* low, maybe it's zero,         //
            //maybe it's -1, I don't know. All I know is, this makes the plugin work. In stock, at least. I'm questioning //
            //my own sanity writing this, but hey, it works. What else can I say? :)                                      //
            {
                
                if (!currentVessel.Landed &&
                    !currentVessel.situation.Equals(Vessel.Situations.PRELAUNCH)
                    && !currentVessel.situation.Equals(Vessel.Situations.ORBITING)
                    ) //The ship probably isn't in danger of crashing if it's landed
                {
                    if ((Math.Abs(currentVessel.verticalSpeed) * DangerAlertSettings.Instance.Tolerance) > currentVessel.heightFromTerrain &&
                        Math.Abs(currentVessel.srfSpeed) > DangerAlertSettings.Instance.MinimumSpeed &&
                        currentVessel.verticalSpeed < DangerAlertSettings.Instance.MinimumVerticalSpeed) // Does fancy math, only "if ship is crashing"
                    {
                        return true; //...I'm in danger!
                    }
                    return false;
                }
            }
            return false; //I'm safe.

        }

        bool LowResourceAlert(Vessel currentVessel, string resStr, byte percentage)
        {
            foreach (Vessel.ActiveResource res in currentVessel.GetActiveResources())
            {
                if (res.info.name.ToUpper() == resStr)
                {
                    if (res.amount < res.maxAmount * (percentage * 0.01))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && !Paused)
            {
                Vessel currentVessel = FlightGlobals.ActiveVessel;
                soundActive = dangerAlertGui.soundToggle;
                DangerAlertSettings.Instance.UpdateFromGui(dangerAlertGui);

                soundplayer.SetVolume(DangerAlertSettings.Instance.MasterVolume);
                if (InDangerOfCrashing(currentVessel))
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
                }
                else if (LowResourceAlert(currentVessel, "ELECTRICCHARGE", 20))
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
                }
                else
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

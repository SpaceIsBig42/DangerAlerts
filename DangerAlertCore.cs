// DangerAlerts v1.0.0: A KSP mod. Public domain, do whatever you want, man.
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
        bool InDangerOfCrashing() // Returns a value.
        {
            if (FlightGlobals.ship_altitude < FlightGlobals.getMainBody().timeWarpAltitudeLimits[2])
            //I'd like to talk a bit about the if statement above, because it's totally rad and bonkers.                  //
            //For _some_ reason, KSP decides that once you're past that magical threshold, (50x timewarp minimum height)  //
            //that calculating surface altitude is pointless, so it defaults to *something* low, maybe it's zero,         //
            //maybe it's -1, I don't know. All I know is, this makes the plugin work. In stock, at least. I'm questioning //
            //my own sanity writing this, but hey, it works. What else can I say? :)                                      //
            {
                Vessel currentVessel = FlightGlobals.ActiveVessel;
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

        void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && !Paused)
            {
                soundActive = dangerAlertGui.totalToggle;
                DangerAlertSettings.Instance.UpdateFromGui(dangerAlertGui);

                soundplayer.SetVolume(DangerAlertSettings.Instance.MasterVolume);
                if (InDangerOfCrashing())
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
            DangerAlertSettings.Instance.SaveCfg();
        }
    }
}

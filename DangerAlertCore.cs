﻿// DangerAlerts v1.0.0: A KSP mod. Public domain, do whatever you want, man.
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
        private int minimumSpeed = 10; //The alarm will only go off if the speed goes above this
                                        //so you don't get an alarm while on the launchpad

        public int MinimumVerticalSpeed = 3; //

        private int distanceTolerance = 7; //Multiplies the current speed to match with the height
        public bool alarmActive = false;

        private bool pluginActive = true;

        void Start()
        {
            Debug.Log("[DNGRALT] Danger Alerts started."); //Lets the user know the add-on was started, DEBUG
            Debug.Log("[DNGRALT] Sound file exists: " + GameDatabase.Instance.ExistsAudioClip(normalAlert));
            soundplayer.Initialize(normalAlert); // Initializes the player, does some housekeeping
            soundplayer.MovePlayer(FlightGlobals.ActiveVessel); //Moves the player to active vessel, hopefully shouldn't be needed

            dangerAlertGui = gameObject.AddComponent<DangerAlertGUI>();
        }

        bool InDangerOfCrashing() // Returns a value.
        {
            Vessel currentVessel = FlightGlobals.ActiveVessel;
            if (!currentVessel.Landed && 
                !currentVessel.situation.Equals(Vessel.Situations.PRELAUNCH)
                && !currentVessel.situation.Equals(Vessel.Situations.ORBITING)) //The ship probably isn't in danger of crashing if it's landed
            {
                if ((Math.Abs(currentVessel.verticalSpeed) * distanceTolerance) > currentVessel.heightFromTerrain &&
                    Math.Abs(currentVessel.srfSpeed) > minimumSpeed && 
                    currentVessel.verticalSpeed < MinimumVerticalSpeed) // Does fancy math, only "if ship is crashing"
                {
                    return true; //...I'm in danger!
                }
                return false;
            }
            return false; //I'm safe.

        }

        void Update()
        {
            pluginActive = dangerAlertGui.totalToggle;
            if (pluginActive)
            {
                if (InDangerOfCrashing())
                {
                    if (!alarmActive)
                    {
                        alarmActive = true;
                    }
                    if (!soundplayer.SoundPlaying())
                    {
                        soundplayer.PlaySound(FlightGlobals.ActiveVessel);
                    }
                }
                else
                {
                    if (alarmActive)
                    {
                        alarmActive = false;
                        soundplayer.StopSound();
                    }
                }
            }
        }

        void OnDestroy()
        {
            Destroy(dangerAlertGui); //Cleaning, cleaning, oh what fun is cleaning! (otherwise you get LOADS of buttons, apparently)
        }
    }
}

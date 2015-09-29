// DangerAlerts v0.1pancake: A KSP mod. Public domain, do whatever you want, man.

#define DEBUG

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
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DangerAlertCore : MonoBehaviour
    {
        private string normalAlert = "DangerAlerts/Sounds/normalAlert";
        AlertSoundPlayer soundplayer = new AlertSoundPlayer();
        private int minimumSpeed = -10; //The alarm will only go off if the speed goes below this, so you don't get an alarm while on the launchpad
        private int distanceTolerance = 7; //Multiplies the current speed to match with the height
        public bool alarmActive = false;
        void Start()
        {
            Debug.Log("Danger Alerts started."); //Lets the user know the add-on was started DEBUG
            Debug.Log("" + GameDatabase.Instance.ExistsAudioClip(normalAlert));
            soundplayer.Initialize();
            soundplayer.MovePlayer(FlightGlobals.ActiveVessel);

        }

        bool InDangerOfCrashing() // Returns a value.
        {
            if (!FlightGlobals.ActiveVessel.Landed) //The ship probably isn't in danger of crashing if it's landed
            {
                if ((Math.Abs(FlightGlobals.ActiveVessel.verticalSpeed)) * distanceTolerance > FlightGlobals.ActiveVessel.heightFromTerrain &&
                    FlightGlobals.ActiveVessel.verticalSpeed < minimumSpeed) // Does fancy math, only "crashing"
                {
                    return true; //...I'm in danger!
                }
                return false;
            }
            return false; //I'm safe.

        }

        void Update()
        {
            #if DEBUG
            if (Input.GetKeyDown(KeyCode.Backspace)) // DEBUG FUNCTION
            {
                Debug.Log("Yo, i'm still here!");
            }
            #endif
            if (HighLogic.LoadedSceneIsFlight)
            {
                //Remove the class, may not be needed
            }
            if (InDangerOfCrashing())
            {
                if (!alarmActive)
                {
                    Debug.Log("You're in danger, playing alert sound");
                    soundplayer.PlaySound(normalAlert, FlightGlobals.ActiveVessel);
                    alarmActive = true;
                }
            }
            else
            {
                alarmActive = false;
            }
        }
    }
}

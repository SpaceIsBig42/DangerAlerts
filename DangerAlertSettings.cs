using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace DangerAlerts
{
    class DangerAlertSettings
    {
        public static int Tolerance = 7;
        public static int MinimumVerticalSpeed = -3;
        public static int MinimumSpeed = 10;
        public static float MasterVolume = 0.5f;

        public static Rect GUIPosition = new Rect(); 

        public static void UpdateFromGui(DangerAlertGUI gui)
        {
            Tolerance = gui.ToleranceBox;
            MinimumVerticalSpeed = gui.MinimumVerticalSpeedBox;
            MinimumSpeed = gui.MinimumSpeedBox;
            MasterVolume = gui.VolumeSlider;

            GUIPosition = gui.GetPosition();
        }
    }
}

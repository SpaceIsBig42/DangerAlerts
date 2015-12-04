using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace DangerAlerts
{
    class DangerAlertSettings
    {
        private static DangerAlertSettings instance = null;
        public static DangerAlertSettings Instance //Singleton interface
        {
            get
            {
                if (instance == null)
                {
                    instance = new DangerAlertSettings();
                }
                return instance;
            }
        }

        protected string filePath = KSPUtil.ApplicationRootPath + "GameData/DangerAlerts/options.cfg";

        [Persistent] public bool SoundToggle;
        [Persistent] public int Tolerance = 7;
        [Persistent] public int MinimumVerticalSpeed = -3;
        [Persistent] public int MinimumSpeed = 10;
        [Persistent] public float MasterVolume = 0.5f;

        public Rect GUIPosition = new Rect(); 

        public void UpdateFromGui(DangerAlertGUI gui)
        {
            Tolerance = gui.ToleranceBox;
            MinimumVerticalSpeed = gui.MinimumVerticalSpeedBox;
            MinimumSpeed = gui.MinimumSpeedBox;
            MasterVolume = gui.VolumeSlider;

            GUIPosition = gui.GetPosition();
        }

        public void UpdateFromCfg()
        {
            if (System.IO.File.Exists(filePath))
            {
                ConfigNode cnToLoad = ConfigNode.Load(filePath);
                ConfigNode.LoadObjectFromConfig(this, cnToLoad);
            }
        }

        public void SaveCfg()
        {
            ConfigNode cnTemp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
            cnTemp.Save(filePath);
        }
    }
}

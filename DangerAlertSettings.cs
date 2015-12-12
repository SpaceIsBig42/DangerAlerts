// DangerAlerts v1.1: A KSP mod. Public domain, do whatever you want, man.
// Author: SpaceIsBig42/Norpo (same person)

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



        private string filePath = KSPUtil.ApplicationRootPath + "GameData/DangerAlerts/options.cfg"; //File path

        [Persistent] public bool SoundToggle = true;
        [Persistent] public float MasterVolume = 0.5f;
        [Persistent] public GUIWindow Window = GUIWindow.OPTIONS;

        public Rect GUIPosition = new Rect();
 


        public void UpdateFromGui(DangerAlertGUI gui)
            //Updates all the setting classes variables from a DangerAlertGUI's textbox values.
        {
            MasterVolume = gui.VolumeSlider;
            SoundToggle = gui.soundToggle;
            Window = gui.Window;

            GUIPosition = gui.GetPosition();
        }

        public void UpdateFromCfg()
            //Loads in values from filePath's file.
        {
            if (System.IO.File.Exists(filePath))
            {
                ConfigNode cnToLoad = ConfigNode.Load(filePath);
                ConfigNode.LoadObjectFromConfig(this, cnToLoad);
            }
        }

        

        public void SaveCfg()
            //Saves all [Persistent] variables to the cfg, in filePath
        {
            ConfigNode cnTemp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
            cnTemp.Save(filePath);
        }
    }
}

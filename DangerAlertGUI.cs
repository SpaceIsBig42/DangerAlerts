// DangerAlerts v1.0.0: A KSP mod. Public domain, do whatever you want, man.
// Author: SpaceIsBig42/Norpo (same person)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace DangerAlerts
{
    // [KSPAddon(KSPAddon.Startup.Flight, false)] Where we're going, we don't *need* Startup.Flight.
    class DangerAlertGUI : MonoBehaviour
    {
        public bool soundToggle = DangerAlertSettings.Instance.SoundToggle; //The toggle boolean for "disable all sound", 
                                                                            //currently the only toggle (v1.1), now only toggles sound

        private string toleranceBox = DangerAlertSettings.Instance.Tolerance.ToString();
        private string minimumVerticalSpeedBox = DangerAlertSettings.Instance.MinimumVerticalSpeed.ToString();
        private string minimumSpeedBox = DangerAlertSettings.Instance.MinimumSpeed.ToString();
        private float volumeSlider = DangerAlertSettings.Instance.MasterVolume;

        public int ToleranceBox { get { return Int32.Parse(toleranceBox); } }
        public int MinimumVerticalSpeedBox { get { return Int32.Parse(minimumVerticalSpeedBox); } }
        public int MinimumSpeedBox { get { return Int32.Parse(minimumSpeedBox); } }
        public float VolumeSlider { get { return volumeSlider; } }
        private ApplicationLauncherButton dangerAlertButton;
        private Rect windowPosition = DangerAlertSettings.Instance.GUIPosition;
        private bool visible = false; //Inbuilt "visible" boolean, in case I need it for something else.

        private Texture2D safeTexture;
        private Texture2D dangerTexture;

        void Start()
        {
            //Thank youuuuuu, github!
            safeTexture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            string safeTextureFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Icons/safeicon.png");
            safeTexture.LoadImage(File.ReadAllBytes(safeTextureFile));

            dangerTexture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            string dangerTextureFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Icons/dangericon.png");
            dangerTexture.LoadImage(File.ReadAllBytes(dangerTextureFile));

            dangerAlertButton = ApplicationLauncher.Instance.AddModApplication(GuiOn, GuiOff, null, null, null, null,
               (ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW), safeTexture);
        }

        public void InDanger(bool danger)
        {
            if (danger)
            {
                dangerAlertButton.SetTexture(dangerTexture);
            }
            else
            {
                dangerAlertButton.SetTexture(safeTexture);
            }
        }

        public void GuiOn()
        {
            visible = true;
            RenderingManager.AddToPostDrawQueue(42, Ondraw);
        }

        public void GuiOff()
        {
            visible = false;
            RenderingManager.RemoveFromPostDrawQueue(42, Ondraw);
        }
        void Ondraw()
        {
            if (visible)
            {
                windowPosition = GUILayout.Window(10, windowPosition, OnWindow, "Danger Alerts");
            }
        }

        void OnWindow(int windowId)
        {
            if (visible)
            {
                //I'm sad that this might be obsolete once 1.1 hits, but hey, I need it for now...
                GUILayout.BeginVertical(GUILayout.Width(250f));
                soundToggle = GUILayout.Toggle(soundToggle, "Sound Toggle");
                GUILayout.Label("Tolerance (7):");
                toleranceBox = GUI.TextField(new Rect(200, 60, 50, 20), toleranceBox, 2);
                GUILayout.Label("Minimum Vert Speed (-3):");
                minimumVerticalSpeedBox = GUI.TextField(new Rect(200, 90, 50, 20), minimumVerticalSpeedBox, 3);
                GUILayout.Label("Minimum Speed (10):");
                minimumSpeedBox = GUI.TextField(new Rect(200, 120, 50, 20), minimumSpeedBox, 2);
                GUILayout.Label("Master Volume:");
                volumeSlider = GUI.HorizontalSlider(new Rect(135, 150, 110, 10), volumeSlider, 0f, 1f);
                GUILayout.EndVertical();
                

                GUI.DragWindow();
                ValueCheck();
            }
        }
        void ValueCheck()
        {
            try
            {
                if (Int32.Parse(toleranceBox) < 1)
                {
                    toleranceBox = "1";
                }
            }
            catch (FormatException e)
            {
                toleranceBox = "1";
            }
            try
            {
                if (Int32.Parse(minimumVerticalSpeedBox) > 0)
                {
                    toleranceBox = "0";
                }
            }
            catch (FormatException e)
            {
                toleranceBox = "0";
            }
            try
            {
                if (Int32.Parse(minimumSpeedBox) < 0)
                {
                    toleranceBox = "0";
                }
            }
            catch (FormatException e)
            {
                toleranceBox = "0";
            }
        }

        public Rect GetPosition()
        {
            return windowPosition;
        }

        void OnDestroy()
        {
            //I don't even want to know why I wrote this, or when. Scared to remove it, though.
            ApplicationLauncher.Instance.RemoveModApplication(dangerAlertButton);
        }
    }
}

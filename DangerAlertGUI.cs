// DangerAlerts v1.1: A KSP mod. Public domain, do whatever you want, man.
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
    enum GUIWindow
    {
        OVERVIEW,
        OPTIONS,
        COLLISION,
        RESOURCE
    }

    class DangerAlertGUI : MonoBehaviour
    {
        public GUIWindow Window = GUIWindow.OPTIONS;

        public bool soundToggle = DangerAlertSettings.Instance.SoundToggle; //The toggle boolean for "disable all sound", 
                                                                            //currently the only toggle (v1.1), now only toggles sound

        private float volumeSlider = DangerAlertSettings.Instance.MasterVolume;

        public float VolumeSlider { get { return volumeSlider; } }

        string collisionTolerance = "7";

        string collisionMinimumSpeed = "10";

        string collisionMinimumVerticalSpeed = "-2";

        private ApplicationLauncherButton dangerAlertButton;
        private Rect windowPosition = DangerAlertSettings.Instance.GUIPosition;

        private bool visible = false; //Inbuilt "visible" boolean, in case I need it for something else.

        private Texture2D safeTexture;
        private Texture2D dangerTexture;

        void Start()
        {
            //Thank youuuuuu, github!
            safeTexture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            string safeTextureFile = KSPUtil.ApplicationRootPath + "GameData/DangerAlerts/Icons/safeicon.png";
            safeTexture.LoadImage(File.ReadAllBytes(safeTextureFile));

            dangerTexture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            string dangerTextureFile = KSPUtil.ApplicationRootPath + "GameData/DangerAlerts/Icons/dangericon.png";
            dangerTexture.LoadImage(File.ReadAllBytes(dangerTextureFile));

            dangerAlertButton = ApplicationLauncher.Instance.AddModApplication(GuiOn, GuiOff, null, null, null, null,
               (ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW), safeTexture);

            DangerAlertList.Instance.UpdateAlertsFromDat();

            DangerAlertSettings.Instance.UpdateFromGui(this);

            collisionTolerance = DangerAlertList.Instance.CollisionAlertList.First().Tolerance.ToString(); //that's a mouthful
            collisionMinimumSpeed = DangerAlertList.Instance.CollisionAlertList.First().MinimumSpeed.ToString();
            collisionMinimumVerticalSpeed = DangerAlertList.Instance.CollisionAlertList.First().MinimumVerticalSpeed.ToString();
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
                windowPosition = GUILayout.Window(10, windowPosition, OnWindow, "Danger Alerts v1.1");

                DangerAlertSettings.Instance.UpdateFromGui(this);
            }
        }

        void OnWindow(int windowId)
        {
            if (visible)
            {
                //I'm sad that this might be obsolete once 1.1 hits, but hey, I need it for now...
                GUILayout.BeginHorizontal(GUILayout.Width(550f));

                if(GUILayout.Button("Options"))
                {
                    Window = GUIWindow.OPTIONS;
                }

                if (GUILayout.Button("Collision"))
                {
                    Window = GUIWindow.COLLISION;
                }

                GUILayout.EndHorizontal();

                ShowCurrentGUI();
                    

                GUI.DragWindow();
            }
        }

        void ShowCurrentGUI()
        {
            switch (Window)
            {
                case GUIWindow.OPTIONS:
                    ShowOptionsGUI();
                    break;

                case GUIWindow.COLLISION:
                    ShowCollisionGUI();
                    break;

                default:
                    ShowOptionsGUI();
                    break;
            }
        }

        void ShowOptionsGUI()
        {

            GUILayout.BeginVertical(GUILayout.Width(550f));

            soundToggle = GUILayout.Toggle(soundToggle, "Sound Toggle");
            GUILayout.Label("Master Volume:");
            volumeSlider = GUILayout.HorizontalSlider(volumeSlider, 0f, 1f);

            GUILayout.EndVertical();
        }

        void ShowCollisionGUI()
        {

            GUILayout.Label("Tolerance:");

            collisionTolerance = GUILayout.TextField(collisionTolerance, 3);

            GUILayout.Label("Min Speed:");

            collisionMinimumSpeed = GUILayout.TextField(collisionMinimumSpeed, 3);

            GUILayout.Label("Min Vertical Speed:");

            collisionMinimumVerticalSpeed = GUILayout.TextField(collisionMinimumVerticalSpeed, 3);

            Debug.Log("1: collision, 2: total");
            Debug.Log(DangerAlertList.Instance.CollisionAlertList.Count);
            Debug.Log(DangerAlertList.Instance.AlertList.Count);

            CollisionAlert collisionAlert = DangerAlertList.Instance.CollisionAlertList.First();

            collisionAlert.Tolerance = Int32.Parse(collisionTolerance);

            collisionAlert.MinimumSpeed = Int32.Parse(collisionMinimumSpeed);

            collisionAlert.MinimumVerticalSpeed = Int32.Parse(collisionMinimumVerticalSpeed);

            CollisionValueCheck();

        }
        void CollisionValueCheck()
        {
            //Simple sanity check function, checks if the field is a possible value, if not, defaults to one.
            //This can be annoying when you're trying to type in a new value, and should be replaced by a different
            //system once KSP v1.1 hits, so I can know what I'm actually doing with the GUI then.
            if (Window == GUIWindow.COLLISION && visible)
            {
                try
                {
                    if (Int32.Parse(collisionTolerance) < 1)
                    {
                        collisionTolerance = "1";
                    }
                }
                catch (FormatException e)
                {
                    collisionTolerance = "1";
                }
                try
                {
                    if (Int32.Parse(collisionMinimumVerticalSpeed) > -1)
                    {
                        collisionMinimumVerticalSpeed = "-1";
                    }
                }
                catch (FormatException e)
                {
                    collisionMinimumVerticalSpeed = "-1";
                }
                try
                {
                    if (Int32.Parse(collisionMinimumSpeed) < 0)
                    {
                        collisionMinimumSpeed = "0";
                    }
                }
                catch (FormatException e)
                {
                    collisionMinimumSpeed = "0";
                }
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
            DangerAlertList.Instance.SaveAlertsDat();
        }
    }
}

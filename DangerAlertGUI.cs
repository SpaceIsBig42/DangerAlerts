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

        bool collisionEnabled = true;

        int resourceIndex = 0;

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
            collisionEnabled = DangerAlertList.Instance.CollisionAlertList.First().Enabled;
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

                else if (GUILayout.Button("Collision"))
                {
                    Window = GUIWindow.COLLISION;
                }

                else if (GUILayout.Button("Resources"))
                {
                    Window = GUIWindow.RESOURCE;
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
                
                case GUIWindow.RESOURCE:
                    ShowResourceGUI();
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

            GUILayout.BeginHorizontal(GUILayout.Width(550f));

            GUILayout.Label("Tolerance:");

            collisionTolerance = GUILayout.TextField(collisionTolerance, 3);

            GUILayout.Space(50f);

            GUILayout.Label("Min Speed:");

            collisionMinimumSpeed = GUILayout.TextField(collisionMinimumSpeed, 3);

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(GUILayout.Width(550f));

            GUILayout.Label("Min Vertical Speed:");

            collisionMinimumVerticalSpeed = GUILayout.TextField(collisionMinimumVerticalSpeed, 3);

            GUILayout.Space(100f);

            GUILayout.Label("Alarm Enabled");

            collisionEnabled = GUILayout.Toggle(collisionEnabled, "");

            GUILayout.EndHorizontal();


            CollisionAlert collisionAlert = DangerAlertList.Instance.CollisionAlertList.First();

            collisionAlert.Tolerance = Int32.Parse(collisionTolerance);

            collisionAlert.MinimumSpeed = Int32.Parse(collisionMinimumSpeed);

            collisionAlert.MinimumVerticalSpeed = Int32.Parse(collisionMinimumVerticalSpeed);

            collisionAlert.Enabled = collisionEnabled;

            CollisionValueCheck();

        }

        void ShowResourceGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(550f));

            GUILayout.Space(100f);

            if (resourceIndex != 0)
            {
                if (GUILayout.Button("<"))
                {
                    resourceIndex--;
                }
            }

            GUILayout.Label("Alert (" + (resourceIndex + 1).ToString() + ")"); // The reason for the + 1 is because most people don't use zero index :)

            if (resourceIndex < DangerAlertList.Instance.ResourceAlertList.Count)
            {
                if (GUILayout.Button(">"))
                {
                    DangerAlertUtils.Log("Boop!"); //TEMPORARY! TEMPORARY TEMPORARY TEMPORARY REMOVE TODO REMOVE
                    resourceIndex++;
                }
            }

            if (GUILayout.Button("Add"))
            {
                DangerAlertList.Instance.AddAlert(new ResourceAlert("ElectricCharge", 20));
                if (DangerAlertList.Instance.ResourceAlertList.Count > 1)
                {
                    resourceIndex++;
                }
            }

            if (DangerAlertList.Instance.ResourceAlertList.Count > 0)
            {
                if (GUILayout.Button("Remove"))
                {
                    DangerAlertList.Instance.RemoveAlert(DangerAlertList.Instance.ResourceAlertList[resourceIndex]);
                }
            }

            GUILayout.Space(100f);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(550f));

            if (DangerAlertList.Instance.ResourceAlertList.Count == 0)
            {
                GUILayout.Label("There are currently no Resource Alerts.");
                GUILayout.EndHorizontal();
            }
            else
            {
                // Add alert modifying code here
                GUILayout.Label(DangerAlertList.Instance.ResourceAlertList[resourceIndex].ResourceString);
                GUILayout.EndHorizontal();
            }

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
            DangerAlertUtils.Log("DangerAlertGUI is being destroyed");
            ApplicationLauncher.Instance.RemoveModApplication(dangerAlertButton);
            DangerAlertList.Instance.SaveAlertsDat();
        }
    }
}

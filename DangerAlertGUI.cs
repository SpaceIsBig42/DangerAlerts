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
        public bool totalToggle = true;
        private ApplicationLauncherButton dangerAlertButton;
        private Rect _windowPosition = new Rect();
        private bool visible = false;

        void Start()
        {
            //Thank you github!
            Texture2D texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            string textureFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Icons/dangeralerticondef.png");
            texture.LoadImage(File.ReadAllBytes(textureFile));
            dangerAlertButton = ApplicationLauncher.Instance.AddModApplication(GuiOn, GuiOff, null, null, null, null,
                ApplicationLauncher.AppScenes.FLIGHT, texture);
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
                _windowPosition = GUILayout.Window(10, _windowPosition, OnWindow, "Danger Alerts");
            }
        }
        public void Ping()
        {
            Debug.Log("DangerAlertGUI responded!");
        }

        void OnWindow(int windowId)
        {
            if (visible)
            {
                GUILayout.BeginHorizontal(GUILayout.Width(250f));
                totalToggle = GUILayout.Toggle(totalToggle, "Enable/Disable everything!");
                GUILayout.EndHorizontal();

                GUI.DragWindow();
            }
        }
        void OnDestroy()
        {
            ApplicationLauncher.Instance.RemoveModApplication(dangerAlertButton);
        }
    }
}

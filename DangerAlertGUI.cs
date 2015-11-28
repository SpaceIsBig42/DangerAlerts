using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;

namespace DangerAlerts
{
    // [KSPAddon(KSPAddon.Startup.Flight, false)] Where we're going, we don't *need* Startup.Flight.
    class DangerAlertGUI : MonoBehaviour
    {
        private Rect _windowPosition = new Rect();
        void Start()
        {
            GuiOn();
        }

        public void GuiOn()
        {
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }

        public void GuiOff()
        {
            RenderingManager.RemoveFromPostDrawQueue(0, OnDraw);
        }
        void OnDraw()
        {
            _windowPosition = GUILayout.Window(10, _windowPosition, OnWindow, "Danger Alerts");
        }
        public void Ping()
        {
            Debug.Log("DangerAlertGUI responded!");
        }

        void OnWindow(int windowId)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(250f));
            GUILayout.Label("This is a label");
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }
    }
}

using BepInEx;
using GorillaNetworking;
using HarmonyLib;
using System;
using System.Threading;
using UnityEngine;
using Valve.VR;

namespace ForeverHz
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public int HZ = 60;
        public bool gripOnly = false;
        void Start() =>
            instance = this;

        void OnGUI()
        {
            HZ = (int)GUI.HorizontalSlider(new Rect(4f, 4f, 400f, 40f), HZ, 0f, 150f);
            GUI.Label(new Rect(414f, 1f, 144f, 60f), "HZ: " + HZ.ToString());
            GUI.Label(new Rect(4f, 40f, 900f, 40f), "HZ slider by @goldentrophy");
            gripOnly = GUI.Toggle(new Rect(300f, 40f, 1080f, 40f), gripOnly, "Right Grip");
        }

        private static float lastTime;
        void Update()
        {
            if (gripOnly && !ControllerInputPoller.instance.rightGrab)
                return;

            float targetDelta = 1f / HZ;
            float elapsed = Time.realtimeSinceStartup - lastTime;

            if (elapsed < targetDelta)
            {
                int sleepMs = Mathf.FloorToInt((targetDelta - elapsed) * 1000);
                if (sleepMs > 0)
                    Thread.Sleep(sleepMs);
            }

            lastTime = Time.realtimeSinceStartup;
        }
    }
}

// DangerAlerts v1.1: A KSP mod. Public domain, do whatever you want, man.
// Author: SpaceIsBig42/Norpo (same person)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DangerAlerts
{
    [Serializable]
    class DangerAlertList
    {
        private static DangerAlertList instance = null;
        public static DangerAlertList Instance //Singleton interface

        {
            get
            {
                if (instance == null)
                {
                    instance = new DangerAlertList();
                }
                return instance;
            }
        }

        private string alertPath { get { return KSPUtil.ApplicationRootPath + "GameData/DangerAlerts/alerts.dat"; } } //alerts.dat path

        public List<AlertBase> AlertList = new List<AlertBase>();

        public List<CollisionAlert> CollisionAlertList = new List<CollisionAlert>();

        public List<ResourceAlert> ResourceAlertList = new List<ResourceAlert>();


        public void AddAlert(AlertBase alert)
        {
            AlertList.Add(alert);

            if (alert is CollisionAlert)
            {
                CollisionAlertList.Add((CollisionAlert) alert);
            }
            else if (alert is ResourceAlert)
            {
                ResourceAlertList.Add((ResourceAlert) alert);
            }
        }

        public void RemoveAlert(AlertBase alert)
        {
            AlertList.Remove(alert);

            if (alert is CollisionAlert)
            {
                CollisionAlertList.Remove((CollisionAlert) alert);
            }
            else if (alert is ResourceAlert)
            {
                ResourceAlertList.Remove((ResourceAlert) alert);
            }
        }

        public void UpdateAlertsFromDat()
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
            Stream stream = new FileStream(alertPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            instance = (DangerAlertList)formatter.Deserialize(stream);
            stream.Close();
            }
            catch //I don't know anymore!
            {
                SetToDefault();
                SaveAlertsDat();
            }
        }

        public void SaveAlertsDat()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(alertPath, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, instance);
            stream.Close();
        }

        private void SetToDefault()
        {
            DangerAlertUtils.Log("Alerts being set to default; probably due to first start-up");
            AddAlert(new CollisionAlert(7, 10, -2));
        }

    }
}

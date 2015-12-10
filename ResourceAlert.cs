using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DangerAlerts
{
    class ResourceAlert
    {

        public string ResourceString;
        public byte Percentage;

        public ResourceAlert(string resourceString, byte percentage)
        {
            ResourceString = resourceString;
            Percentage = percentage;
        }


        public override bool Triggered(Vessel currentVessel)
        {
            foreach (Vessel.ActiveResource res in currentVessel.GetActiveResources())
            {
                if (res.info.name.ToUpper() == ResourceString.ToUpper())
                {
                    if (res.amount < res.maxAmount * (Percentage * 0.01))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


    }
}

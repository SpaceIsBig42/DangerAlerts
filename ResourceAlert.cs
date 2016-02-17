// DangerAlerts v1.2: A KSP mod. Released under the MIT license; 
// see LICENSE.txt for details.
// Author: SpaceIsBig42/Norpo (same person)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DangerAlerts
{
    [Serializable]
    class ResourceAlert : AlertBase
    {

        public string ResourceString;
        public int Percentage;

        public ResourceAlert(string resourceString, byte percentage)
        {
            ResourceString = resourceString;
            Percentage = percentage;
            priority = AlertPriorities.HIGH;
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

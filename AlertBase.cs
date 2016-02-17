// DangerAlerts v1.2: A KSP mod. Released under the MIT license; 
// see LICENSE.txt for details.
// Author: SpaceIsBig42/Norpo (same person)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DangerAlerts
{
    enum AlertPriorities
    {
        CRITICAL,
        HIGH,
        MODERATE,
        MINOR
    }

    [Serializable]
    abstract class AlertBase
    {
        public AlertPriorities priority = AlertPriorities.CRITICAL;

        public bool Enabled = true;

        public abstract bool Triggered(Vessel currentVessel);
    }
}

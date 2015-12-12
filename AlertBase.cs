// DangerAlerts v1.1: A KSP mod. Public domain, do whatever you want, man.
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

        private bool enabled = true;
        public bool Enabled { get { return enabled; } }

        public abstract bool Triggered(Vessel currentVessel);
    }
}

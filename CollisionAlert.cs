// DangerAlerts v1.1: A KSP mod. Public domain, do whatever you want, man.
// Author: SpaceIsBig42/Norpo (same person)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DangerAlerts
{
    [Serializable]
    class CollisionAlert : AlertBase
    {

        public CollisionAlert(int tolerance, int minimumSpeed, int minimumVerticalSpeed)
        {
            Tolerance = tolerance;
            MinimumSpeed = minimumSpeed;
            MinimumVerticalSpeed = minimumVerticalSpeed;
        }

        public int Tolerance;
        public int MinimumSpeed;
        public int MinimumVerticalSpeed;

        public override bool Triggered(Vessel currentVessel)
        {
            if (currentVessel.heightFromTerrain > 0)
            //I'd like to talk a bit about the if statement above, because it's totally rad and bonkers.                  //
            //For _some_ reason, KSP decides that once you're past that magical threshold,                                //
            //usually, but not always 50x timewarp minimum height,                                                        //
            //that calculating surface altitude is pointless, so it defaults to *something* low, maybe it's zero,         //
            //maybe it's -1, I don't know. All I know is, this makes the plugin work. In stock, at least. I'm questioning //
            //my own sanity writing this, but hey, it works. What else can I say? :)                                      //
            {

                if (!currentVessel.Landed &&
                    !currentVessel.situation.Equals(Vessel.Situations.PRELAUNCH)
                    && !currentVessel.situation.Equals(Vessel.Situations.ORBITING)
                    ) //The ship probably isn't in danger of crashing if it's landed
                {
                    if ((Math.Abs(currentVessel.verticalSpeed) * Tolerance) > currentVessel.heightFromTerrain &&
                        Math.Abs(currentVessel.srfSpeed) > MinimumSpeed &&
                        currentVessel.verticalSpeed < MinimumVerticalSpeed) // Does fancy math, only "if ship is crashing"
                    {
                        return true; //...I'm in danger!
                    }
                    return false;
                }
            }
            return false; //I'm safe.
        }
    }
}

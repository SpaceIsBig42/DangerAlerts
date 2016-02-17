﻿// DangerAlerts v1.2: A KSP mod. Released under the MIT license; 
// see LICENSE.txt for details.
// Author: SpaceIsBig42/Norpo (same person)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DangerAlerts
{
    static class DangerAlertUtils
    {

        static public string Version = "1.1";

        static string tag = "[Danger Alerts] ";

        static public void Log(string message)
        {
            message = tag + message;
            Debug.Log(message);
        }

        static public void LogError(string message)
        {
            message = tag + message;
            Debug.LogError(message);
        }

        static public void LogWarning(string message)
        {
            message = tag + message;
            Debug.LogWarning(message);
        }
    }
}

using System;
using UnityEngine;

namespace UnityDemo
{
    public class DebugInfo
    {
        private static bool isDebug = true;

        public static void Log(object message)
        {
            if (isDebug)
                Debug.Log(message);
        }

        public static void LogError(object message)
        {
            if (isDebug)
                Debug.LogError(message);
        }

        public static void LogWarning(object message)
        {
            if (isDebug)
                Debug.LogWarning(message);
        }
    }
}


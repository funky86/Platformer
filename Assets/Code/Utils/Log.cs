using System;
using UnityEngine;

namespace Platformer.Utils {
    public class Log {
        public static void Debug(String msg, params object[] args) {
            String format = string.Format("f={0} t={1} {2}", Time.frameCount, Time.realtimeSinceStartup, msg);
            UnityEngine.Debug.LogFormat(format, args);
        }
    }
}

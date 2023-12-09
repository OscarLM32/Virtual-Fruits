using System.Collections.Generic;
using UnityEngine;

namespace EditorSystems.Logger
{
    //I would've liked to call this class simply "logger", but there is a class with that name in UnityEngine too
    public static class EditorLogger
    {
        private enum LoggingMode { DEFAULT, WARNING, ERROR }

        private static bool _dynamicDifficultySystem = true;
        private static bool _shootingEnemySystem = true;
        private static bool _player = true;

        private static Dictionary<LoggingSystem, bool> activeLoggers = new()
        {
            {LoggingSystem.DYNAMIC_DIFFICULTY_SYSTEM, _dynamicDifficultySystem },
            {LoggingSystem.SHOOTING_ENEMY, _shootingEnemySystem },
            {LoggingSystem.PLAYER, _player },
        };

        public static void Log(LoggingSystem loggingSystem, string msg)
        {
            Log(LoggingMode.DEFAULT, loggingSystem, msg);
        }

        public static void LogWarning(LoggingSystem loggingSystem, string msg)
        {
            Log(LoggingMode.WARNING, loggingSystem, msg);
        }

        public static void LogError(LoggingSystem loggingSystem, string msg)
        {
            Log(LoggingMode.ERROR, loggingSystem, msg);
        }

        private static void Log(LoggingMode mode, LoggingSystem loggingSystem, string msg)
        {
            //If the system is not active for logging just do nothing
            if (!activeLoggers[loggingSystem]) return;

            string finalMsg = BuildLogMessage(loggingSystem, msg);

            switch (mode)
            {
                case LoggingMode.DEFAULT:
                    Debug.Log(finalMsg);
                    break;
                case LoggingMode.WARNING:
                    Debug.LogWarning(finalMsg);
                    break;
                case LoggingMode.ERROR:
                    Debug.LogError(finalMsg);
                    break;
            }
        }

        private static string BuildLogMessage(LoggingSystem loggingSystem, string msg)
        {
            return $"[{loggingSystem}]: {msg}";
        }
    }
}

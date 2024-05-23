
using System;
using UnityEngine;

namespace DynamicDifficulty.DynamicParametersScriptables
{
    [Serializable]
    public struct FloatingPlatformDynamicParameters
    {
        public float scale;
        public float floatingTime;
        public float timeAfterMotorStopToFall;
    }

    [CreateAssetMenu(fileName = "SOFloatingPlatformDynamicParameters", menuName = "ScriptableObjects/DynamicParameters/Level/FloatingPlatform")]
    public class SOFloatingPlatformDynamicParameters : DynamicParameters<FloatingPlatformDynamicParameters>
    {

    }
}
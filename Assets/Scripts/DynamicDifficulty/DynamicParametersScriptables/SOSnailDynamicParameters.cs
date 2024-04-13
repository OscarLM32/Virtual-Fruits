

using System;
using UnityEngine;

namespace DynamicDifficulty.DynamicParametersScriptables
{
    [Serializable]
    public struct SnailDynamicParameters
    {
        public float speed;
    }

    [CreateAssetMenu(fileName = "SOSnailDynamicParameter", menuName = "ScriptableObjects/DynamicParameters/Spring/Snail")]
    public class SOSnailDynamicParameters : DynamicParameters<SnailDynamicParameters>
    {

    }
}
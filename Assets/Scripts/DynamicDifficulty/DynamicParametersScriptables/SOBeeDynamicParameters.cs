using System;
using UnityEngine;

namespace DynamicDifficulty.DynamicParametersScriptables
{
    [CreateAssetMenu(fileName = "SOBeeDynamicParameters", menuName = "ScriptableObjects/DynamicParameters/Spring/Bee")]
    public class SOBeeDynamicParameters : DynamicParameters<BeeDynamicParameters>
    {

    }

    [Serializable]
    public struct BeeDynamicParameters
    {
        public float attackSpeed;
        public float patrollingSpeed;
    }
}
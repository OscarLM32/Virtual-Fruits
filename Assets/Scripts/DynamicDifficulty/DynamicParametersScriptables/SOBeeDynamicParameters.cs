using System;
using UnityEngine;

namespace DynamicDifficulty.DynamicParametersScriptables
{
    [Serializable]
    public struct BeeDynamicParameters
    {
        public float attackSpeed;
        public float patrollingSpeed;
        public float projectileSpeed;
    }

    [CreateAssetMenu(fileName = "SOBeeDynamicParameters", menuName = "ScriptableObjects/DynamicParameters/Spring/Bee")]
    public class SOBeeDynamicParameters : DynamicParameters<BeeDynamicParameters>{}

}

using System;
using UnityEngine;

namespace DynamicDifficulty.DynamicParametersScriptables
{
    [Serializable]
    public struct BunnyDynamicParameters
    {
        public float patrollingSpeed;
        public float idleTimeFactor;
        public float attackChargeTime;
        public float maxAttackDistance;
        public float maxJumpTime;
    }

    [CreateAssetMenu(fileName = "SOBunnyDynamicParameters", menuName = "ScriptableObjects/DynamicParameters/Spring/Bunny")]
    public class SOBunnyDynamicPrameters : DynamicParameters<BunnyDynamicParameters> { }
}
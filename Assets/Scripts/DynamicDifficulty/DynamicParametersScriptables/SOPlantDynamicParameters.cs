
using System;
using UnityEngine;

namespace DynamicDifficulty.DynamicParametersScriptables
{
    [Serializable]
    public struct PlantDynamicParameters
    {
        public float attackSpeed;
        public float proyectileSpeed;
        public float fleetingSpeed;
        public float attackRange;
        public float fleetingRange;
    }

    [CreateAssetMenu(fileName = "SOPlantDynamicParameters", menuName = "ScriptableObjects/DynamicParameters/Spring/Plant")]
    public class SOPlantDynamicParameters : DynamicParameters<PlantDynamicParameters>
    {

    }
}
using System;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    [Serializable]
    public class DifficultyModifierSetting
    {
        public DifficultyModifierAction action;
        public GameObject target;
    }
}
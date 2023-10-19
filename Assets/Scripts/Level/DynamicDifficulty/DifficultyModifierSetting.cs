using Enemies;
using System;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    [Serializable]
    public class DifficultyModifierSetting
    {
        public DifficultyModifierActionType action;
        public GameObject target;

        public EnemyType enemyType;
        public Vector2 targetPosition;
    }
}
using Enemies;
using System;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    [Serializable]
    public class DifficultyModifier
    {
        public DifficultyModifierAction action;
        public GameObject target;

        public EnemyType enemyType;
        public Vector2 targetPosition;
    }
}
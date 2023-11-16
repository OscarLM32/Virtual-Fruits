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
        public Vector2 position;
        //TODO: i'm not a fan of this so, I should give it a whirl later on
        public Transform parentObject;
        //TODO: add patrolling values
        //TODO: add enemy transform parameters

    }
}
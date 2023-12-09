using Enemies;
using System;

namespace Level.DynamicDifficulty.Modifiers.Parameters
{
    [Serializable]
    public class AddEnemyParameters : ModifierParameters
    {
        public EnemyType enemyType;
        public bool test;
    }
}
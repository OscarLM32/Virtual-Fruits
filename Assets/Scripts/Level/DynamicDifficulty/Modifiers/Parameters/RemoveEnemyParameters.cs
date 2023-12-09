using Enemies;
using System;

namespace Level.DynamicDifficulty.Modifiers.Parameters
{
    [Serializable]
    public class RemoveEnemyParameters : ModifierParameters
    {
        public EnemyType type;
        public string test;
    }
}
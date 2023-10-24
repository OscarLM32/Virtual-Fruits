using System;

namespace Level.DynamicDifficulty
{
    [Serializable]
    public class DifficultySettings
    {
        public Difficulty difficulty;
        public DifficultyModifier[] difficultyModifiers;
    }
}
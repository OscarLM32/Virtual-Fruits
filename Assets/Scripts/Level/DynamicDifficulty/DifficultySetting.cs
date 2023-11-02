using System;

namespace Level.DynamicDifficulty
{
    [Serializable]
    public class DifficultySetting
    {
        public Difficulty difficulty;
        public DifficultyModifier[] difficultyModifiers;
    }
}
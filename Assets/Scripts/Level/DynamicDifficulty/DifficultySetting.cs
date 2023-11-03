using System;

namespace Level.DynamicDifficulty
{
    [Serializable]
    public class DifficultySetting
    {
        public Difficulty difficulty;
        public DifficultyModifier[] difficultyModifiers;

        public DifficultySetting(Difficulty difficulty, DifficultyModifier[] difficultyModifiers = null)
        {
            this.difficulty = difficulty;
            this.difficultyModifiers = difficultyModifiers;
        }
    }
}
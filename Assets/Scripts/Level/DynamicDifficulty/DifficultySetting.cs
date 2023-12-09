using Level.DynamicDifficulty.Modifiers;
using System;

namespace Level.DynamicDifficulty
{
    [Serializable]
    public class DifficultySetting
    {
        public Difficulty difficulty;
        public Modifier[] modifiers;

        public DifficultySetting(Difficulty difficulty, Modifier[] difficultyModifiers = null)
        {
            this.difficulty = difficulty;
            this.modifiers = difficultyModifiers;
        }
    }
}
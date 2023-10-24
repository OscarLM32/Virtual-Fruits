using System;
using System.IO;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    [RequireComponent(typeof(BoxCollider2D))]
    //TODO: Add ICallbackReciever so that I can ensure that there is only 1 DifficultySetting for each difficulty
    public class LevelSector : MonoBehaviour
    {
        public DifficultySettings[] difficultySettings;

        public void SetDifficultyChanges(Difficulty difficulty)
        {
            var settings = GetDifficultySettings(difficulty);
            foreach (var modifierSettings in settings.difficultyModifiers)
            {
                Action modifierAction = DifficultyModifierFactory.GetLevelModifier(modifierSettings);
                modifierAction?.Invoke();
            }
        }

        private DifficultySettings GetDifficultySettings(Difficulty difficulty)
        {
            foreach(var settings in difficultySettings)
            {
                if (difficulty == settings.difficulty)
                    return settings;
            }
            return null;
        }
    }
}
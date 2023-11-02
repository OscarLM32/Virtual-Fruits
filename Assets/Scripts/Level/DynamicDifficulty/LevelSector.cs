using System;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelSector : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private DifficultySetting[] _difficultySettings;

        public void SetDifficultyChanges(Difficulty difficulty)
        {
            var settings = GetDifficultySettings(difficulty);
            foreach (var modifierSettings in settings.difficultyModifiers)
            {
                Action modifierAction = DifficultyModifierFactory.GetLevelModifier(modifierSettings);
                modifierAction?.Invoke();
            }
        }

        private DifficultySetting GetDifficultySettings(Difficulty difficulty)
        {
            foreach (var settings in _difficultySettings)
            {
                if (difficulty == settings.difficulty)
                    return settings;
            }
            return null;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }

    }
}
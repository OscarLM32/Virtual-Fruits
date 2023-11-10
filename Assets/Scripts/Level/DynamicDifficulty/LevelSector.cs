using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelSector : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private DifficultySetting[] _difficultySettings = new DifficultySetting[Enum.GetValues(typeof(Difficulty)).Length - 1];
        private DifficultyModifierFactory _modifierFactory = new();


        #if UNITY_EDITOR
        private void Awake()
        {
            CheckSettingsIntegrity();
        }
        #endif

        public void SetDifficultyChanges(Difficulty difficulty)
        {
            var settings = GetDifficultySettings(difficulty);
            if(settings == null)
            {
                Debug.LogError("["+ gameObject.name + "]"+ "There are no difficulty settings specified for that difficulty");
                return;
            }

            foreach (var modifierSettings in settings.difficultyModifiers)
            {
                Action modifierAction = _modifierFactory.GetLevelModifier(modifierSettings);
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

        private void CheckSettingsIntegrity()
        {
            var difficulties = Enum.GetValues(typeof(Difficulty)) as Difficulty[];
            var currentSettings = _difficultySettings;

            currentSettings = currentSettings.Resize(difficulties.Length - 1);
            List<DifficultySetting> aux = new();

            int counter = 0;

            for (int i = 0; i < difficulties.Length; i++)
            {
                //TODO revisit this code since it does not look like null checking was done properly
                var difficulty = difficulties[i];
                if (difficulty != DynamicDifficultyConstants.baseDifficulty)
                {
                    if (currentSettings[counter] != null)
                    {
                        aux.Add(new DifficultySetting(difficulty, currentSettings[counter]?.difficultyModifiers));
                    }
                    else
                    {
                        aux.Add(new DifficultySetting(difficulty));
                    }

                    counter++;
                }
            }

            counter++;

            while (counter < difficulties.Length)
            {
                if (difficulties[counter] != DynamicDifficultyConstants.baseDifficulty)
                {
                    aux.Add(new DifficultySetting(difficulties[counter]));
                }
                counter++;
            }

            _difficultySettings = aux.ToArray();
        }

        public void OnAfterDeserialize()
        {
            CheckSettingsIntegrity();
        }

        public void OnBeforeSerialize()
        {

        }

    }
}
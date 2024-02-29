using EditorSystems.Logger;
using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    public class LevelSector : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private DifficultySetting[] _difficultySettings;


        private void Awake()
        {
            #if UNITY_EDITOR
            CheckSettingsIntegrity();
            #endif
        }

        public void SetDifficultyChanges(Difficulty difficulty)
        {
            var settings = GetDifficultySettings(difficulty);
            if (settings == null)
            {
                EditorLogger.LogWarning(LoggingSystem.DYNAMIC_DIFFICULTY_SYSTEM, "[" + gameObject.name + "]" + "There are no difficulty settings specified for that difficulty");
                return;
            }

            GameObject newLayout = settings.layoutReference.LoadAssetAsync<GameObject>().WaitForCompletion();
            Instantiate(newLayout, Vector3.zero, Quaternion.identity, transform);
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

            currentSettings = currentSettings.Resize(difficulties.Length);
            List<DifficultySetting> aux = new();

            for (int i = 0; i < difficulties.Length; i++)
            {
                var difficulty = difficulties[i];
                if (currentSettings[i] != null)
                {
                    aux.Add(new DifficultySetting(difficulty, currentSettings[i].layoutReference));
                }
                else
                {
                    aux.Add(new DifficultySetting(difficulty));
                }

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
using EditorSystems.Logger;
using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    [RequireComponent(typeof(BoxCollider2D))]
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

            GameObject currentLayout = transform.GetChild(0).gameObject;
            Destroy(currentLayout);

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

            currentSettings = currentSettings.Resize(difficulties.Length - 1);
            List<DifficultySetting> aux = new();

            int counter = 0;

            for (int i = 0; i < difficulties.Length; i++)
            {
                var difficulty = difficulties[i];
                if (difficulty != DynamicDifficultyConstants.baseDifficulty)
                {
                    if (currentSettings[counter] != null)
                    {
                        aux.Add(new DifficultySetting(difficulty, currentSettings[counter].layoutReference));
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
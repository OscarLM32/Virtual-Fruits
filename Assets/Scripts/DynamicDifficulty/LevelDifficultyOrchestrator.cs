using EditorSystems.Logger;
using System;
using UnityEngine;

namespace DynamicDifficulty
{
    public class LevelDifficultyOrchestrator : MonoBehaviour
    {
        public static Action OnLevelDifficultySet;

        [SerializeField] private LevelSector[] _levelSectors;

        private void Start()
        {
            SetLevelDifficulty(DynamicDifficultyManager.I.GenericDifficulty);
            OnLevelDifficultySet?.Invoke();
        }

        private void SetLevelDifficulty(Difficulty difficulty)
        {
            foreach (LevelSector levelSector in _levelSectors)
            {
                if (levelSector == null)
                {
                    EditorLogger.LogError(LoggingSystem.DYNAMIC_DIFFICULTY_SYSTEM, "{LevelDifficultyOrchestrator}: The sector was not found or set to null");
                    return;
                }
                levelSector.SetDifficultyChanges(difficulty);
            }
        }

        //TODO: implement custom editor to include this button in the inspector
        [ContextMenu("UpdateLevelSectorList")]
        private void UpdateLevelSectorList()
        {
            _levelSectors = FindObjectsOfType<LevelSector>();
        }
    }
}
using EditorSystems.Logger;
using UnityEngine;

namespace DynamicDifficulty
{
    public class LevelDifficultyOrchestrator : MonoBehaviour
    {
        [SerializeField] private LevelSector[] _levelSectors;

        public void SetLevelDifficulty(Difficulty difficulty)
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
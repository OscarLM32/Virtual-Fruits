using UnityEngine;

namespace Level.DynamicDifficulty
{
    public class LevelDifficultyOrchestrator : MonoBehaviour
    {
        [SerializeField] private LevelSector[] _levelSectors;

        public void SetLevelDifficulty(Difficulty difficulty)
        {
            foreach (LevelSector levelSector in _levelSectors)
            {
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
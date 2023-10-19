
using UnityEngine;

namespace Level.DynamicDifficulty
{
    public class LevelDifficultyOrchestrator
    {
        private LevelSector[] _levelSectors;

        void SetLevelDifficulty(Difficulty difficulty)
        {
            _levelSectors = Object.FindObjectsOfType<LevelSector>();

            foreach (LevelSector levelSector in _levelSectors)
            {
                levelSector.SetDifficultyChanges(difficulty);
            }
        }

        //TODO: I need to comment with tutor whether it is a better idea to make difficulties build on top of each other
        //meaning that "very hard" difficulty changes are built on top of the changes made by the "hard" diffculty or 
        //make both difficulties to have duplicated changes
    }
}
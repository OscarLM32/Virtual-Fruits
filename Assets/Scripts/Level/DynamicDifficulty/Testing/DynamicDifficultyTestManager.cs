using UnityEngine;

namespace Level.DynamicDifficulty.Testing
{
    public class DynamicDifficultyTestManager : MonoBehaviour
    {
        public Difficulty difficulty = Difficulty.VERY_EASY;
        public LevelDifficultyOrchestrator levelDifficultyOrchestrator;

        void Start()
        {
            Debug.Log("The difficulty is going to be set at: " + difficulty.ToString());
            levelDifficultyOrchestrator.SetLevelDifficulty(difficulty);
        }
    }
}
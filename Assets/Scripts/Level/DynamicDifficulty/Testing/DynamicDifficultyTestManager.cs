using UnityEngine;

#if UNITY_EDITOR

namespace Level.DynamicDifficulty.Testing
{
    public class DynamicDifficultyTestManager : MonoBehaviour
    {
        [Range(-5, 5)]
        public float currentSkillParameter = 0;
        public LevelDifficultyOrchestrator levelDifficultyOrchestrator;
        [HideInInspector] public Difficulty difficulty = Difficulty.VERY_EASY;

        private LogisticFunctionPlayerSkillCalculator _skillCalculator = new();

        [ContextMenu("Test")]
        private void Start()
        {
            _skillCalculator.PlayerSkillParameter = currentSkillParameter;
            difficulty = _skillCalculator.CalculatePlayerLevelDifficulty();

            Debug.Log("The difficulty is going to be set at: " + difficulty.ToString());
            levelDifficultyOrchestrator.SetLevelDifficulty(difficulty);
        }
    }
}

#endif
using CoreSystems.SaveSystem;
using DynamicDifficulty.Skillcalculator;
using EditorSystems.Logger;
using Enemies;
using GameSystems.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicDifficulty
{
    public class DynamicDifficultyManager : Singleton<DynamicDifficultyManager>
    {

        public Difficulty genericDifficulty { get; private set;}

        //Only for testing purposes
#if UNITY_EDITOR
        [Range(-5, 5)]
        [SerializeField]
#endif
        private float _playerSkillParameter = 0;
        private Dictionary<EnemyType, float> _enemyDifficultyParameters;
        private Dictionary<EnemyType, Difficulty> enemyDifficulties = new();

        //Select the type of calculator wanted
        private ISkillCalculator calculator = new LogisticFunctionCalculator();

        protected override void OnAwake()
        {
            //Load data from the SaveManager
            _playerSkillParameter = SaveManager.I.GetPlayerSkillParameter();
            _enemyDifficultyParameters = SaveManager.I.GetEnemyDifficultyParameters();
            genericDifficulty = calculator.GetPlayerSkillLevel(_playerSkillParameter);   
        }

        private void Start()
        {
            SetUpLevelDifficulty();
        }

        public Difficulty GetEnemyDifficulty(EnemyType enemy)
        {
            if (enemyDifficulties.ContainsKey(enemy)) return enemyDifficulties[enemy];

            enemyDifficulties.Add(enemy, calculator.CalculateEnemyDifficulty(_enemyDifficultyParameters[enemy]));
            return enemyDifficulties[enemy];
        }

        public void SetUpLevelDifficulty()
        {
            LevelDifficultyOrchestrator orchestrator = FindObjectOfType<LevelDifficultyOrchestrator>();
            if (orchestrator == null)
            {
                EditorLogger.LogError(LoggingSystem.DYNAMIC_DIFFICULTY_SYSTEM, "{DynamicDifficultyManager}: Orchestrator not found");
                return;
            }
            orchestrator.SetLevelDifficulty(genericDifficulty);
        }
    }
}
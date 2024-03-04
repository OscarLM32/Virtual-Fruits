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

        //Select the type of calculator wanted
        private ISkillCalculator calculator = new LogisticFunctionCalculator();

#if UNITY_EDITOR
        [Range(-5, 5)] //Only for testing purposes
        [SerializeField]
#endif
        private float _playerSkillParameter = 0;

        private Dictionary<EnemyType, float> enemyDifficultyParameters;
        private Dictionary<EnemyType, Difficulty> enemyDifficulties;


        protected override void OnAwake()
        {
            //Load data from the SaveManager
            //_playerSkillParameter = SavaManager.GetPlayerSkillParameter();
            //enemyDifficultyParameters = SaveManager.GetEnemyDifficultyParameters();
            genericDifficulty = calculator.GetPlayerSkillLevel(_playerSkillParameter);   
        }

        public Difficulty GetEnemyDifficulty(EnemyType enemy)
        {
            if (enemyDifficulties.ContainsKey(enemy)) return enemyDifficulties[enemy];

            enemyDifficulties.Add(enemy, calculator.CalculateEnemyDifficulty(enemyDifficultyParameters[enemy]));
            return enemyDifficulties[enemy];
        }

        public void SetUpLevelDifficulty()
        {
            LevelDifficultyOrchestrator orchestrator = FindObjectOfType<LevelDifficultyOrchestrator>();
            if (orchestrator == null) EditorLogger.LogError(LoggingSystem.DYNAMIC_DIFFICULTY_SYSTEM, "{DynamicDifficultyManager}: Orchestrator not found");

            orchestrator.SetLevelDifficulty(genericDifficulty);
        }
    }
}
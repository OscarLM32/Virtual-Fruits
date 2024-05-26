using CoreSystems.SaveSystem;
using DynamicDifficulty.Skillcalculator;
using EditorSystems.Logger;
using Enemies;
using DevSystems.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicDifficulty
{
    public class DynamicDifficultyManager : Singleton<DynamicDifficultyManager>
    {
        public Difficulty GenericDifficulty { get; private set;}

        private const int _maxSkillParameter = 3;
        private const int _minSkillParameter = -3;

        private const float _improvementFactor = 0.1f;

        //Only for testing purposes
#if UNITY_EDITOR
        [Range(-5, 5)]
        [SerializeField]
#endif
        private float _playerSkillParameter = 0;
        private Dictionary<EnemyType, float> _enemyDifficultyParameters;
        private Dictionary<EnemyType, Difficulty> _enemyDifficulties = new();

        //Select the type of calculator wanted
        private ISkillCalculator _calculator = new LogisticFunctionCalculator();

        protected override void OnAwake()
        {
            _playerSkillParameter = SaveManager.I.GetPlayerSkillParameter();
            _enemyDifficultyParameters = SaveManager.I.GetEnemyDifficultyParameters();
            GenericDifficulty = _calculator.GetPlayerSkillLevel(_playerSkillParameter);
        }

        private void OnEnable()
        {
            GameActions.OnPlayerDeath += OnPlayerDeath;
            GameActions.OnEnemyKilled += OnEnemyKilled;
        }

        public Difficulty GetEnemyDifficulty(EnemyType enemy)
        {
            if (_enemyDifficulties.ContainsKey(enemy)) return _enemyDifficulties[enemy];

            _enemyDifficulties.Add(enemy, _calculator.CalculateEnemyDifficulty(_enemyDifficultyParameters[enemy]));
            return _enemyDifficulties[enemy];
        }


        public void SaveData()
        {
            SaveManager.I.SaveDynamicDifficultyData(_playerSkillParameter, _enemyDifficultyParameters);
        }

        #region SkillUpdate
        private void UpdatePlayerSkill(float value)
        {
            _playerSkillParameter += value;
            _playerSkillParameter = LimitSkillParameter(_playerSkillParameter);
        }

        private void UpdateEnemyDifficultyParameter(float value, EnemyType type)
        {
            _enemyDifficultyParameters[type] += value;
            _enemyDifficultyParameters[type] = LimitSkillParameter(_enemyDifficultyParameters[type]);
        }

        private float LimitSkillParameter(float value)
        {
            if (value > _maxSkillParameter)
            {
                value = _maxSkillParameter;
            }
            else if (value < _minSkillParameter)
            {
                value = _minSkillParameter;
            }
            return value;
        }

        private void OnPlayerDeath(EnemyType? type)
        {
            EditorLogger.Log(LoggingSystem.DYNAMIC_DIFFICULTY_SYSTEM, $"Player died to: {type.ToString()}");
            //TODO: check if using a VOID enum is a better solution
            if(type != null)
            {
                UpdateEnemyDifficultyParameter(_improvementFactor, (EnemyType)type);
            }
            UpdatePlayerSkill(-_improvementFactor);
        }

        private void OnEnemyKilled(EnemyType type)
        {
            EditorLogger.Log(LoggingSystem.DYNAMIC_DIFFICULTY_SYSTEM, $"Enemy {type} was killed");

            UpdateEnemyDifficultyParameter(-_improvementFactor, type);
            UpdatePlayerSkill(_improvementFactor/2);
        }

        private void OnLevelCompleted(float time, float averageTime)
        {
            //Calculate improvement factor based on the time it has taken the player to complete the level
            //It could be loaded from an addressable 
        }

        private void OnSpecialCoinPickup()
        {
            UpdatePlayerSkill(_improvementFactor / 2);
        }
        #endregion
    }
}
using Enemies;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreSystems.SaveSystem.Test
{
#if UNITY_EDITOR
    public class SaveManagerTest : MonoBehaviour
    {
        [SerializeField]
        [Range(-3, 3)]
        private float _playerSkillParameter = 0;
        private Dictionary<EnemyType, float> _enemyDifficultyParameters = new();

        [ContextMenu("LoadDynamicDifficultyData")]
        private void LoadDynamicDifficultyData()
        {
            _enemyDifficultyParameters = SaveManager.I.GetEnemyDifficultyParameters();
            _playerSkillParameter = SaveManager.I.GetPlayerSkillParameter();

            Debug.Log($"[SaveManagerTest]: Info loaded -> playerSkillParameter = {_playerSkillParameter} \n" +
                $"Enemy data retrieved:");
            foreach( var parameter in _enemyDifficultyParameters)
            {
                Debug.Log($"Enemy: {parameter.Key} | Parameter: {parameter.Value}");
            }
        }

        [ContextMenu("SaveDynamicDifficultyData")]
        private void SaveDynamicDifficultyData()
        {
            _enemyDifficultyParameters[EnemyType.BUNNY] = 1.2f;
            _enemyDifficultyParameters[EnemyType.BEE] = -0.8f;

            SaveManager.I.SaveDynamicDifficultyData(_playerSkillParameter, _enemyDifficultyParameters);
        }

        [ContextMenu("SaveDynamicDifficultyWrongData")]
        private void SaveDynamidDifficultyWrongData()
        {
            _enemyDifficultyParameters = null;
            _playerSkillParameter = 327;

            SaveManager.I.SaveDynamicDifficultyData(_playerSkillParameter, null);
        }
    }
#endif
}
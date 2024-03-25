
using Enemies;
using UnityEngine;

namespace DynamicDifficulty.Test
{
    public class DynamicDifficultyManagerTest : MonoBehaviour
    {
        public EnemyType enemy;

        [ContextMenu("UpdateData")]
        public void UpdateData()
        {
            DynamicDifficultyManager.I.SaveData();
        }

        [ContextMenu("SimulateEnemyKilled")]
        public void SimulateEnemyKilled()
        {
            GameActions.OnEnemyKilled?.Invoke(enemy);
        }

        [ContextMenu("SimulatePlayerDeath")]
        public void SimulatePlayerDeath()
        {
            GameActions.OnPlayerDeath?.Invoke(null);
        }

        [ContextMenu("SimulatePLayerDeathByEnemy")]
        public void SimulatePlayerDeathByEnemy()
        {
            GameActions.OnPlayerDeath?.Invoke(enemy);
        }
    }
}
using Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level.DynamicDifficulty
{
    internal class DifficultyModifierFactory
    {
        private Dictionary<EnemyType, GameObject> _loadedEnemies = new();

        public Action GetLevelModifier(DifficultyModifier settings)
        {
            var actionType = settings.action;
            Action modifier = null;

            switch (actionType)
            {
                case DifficultyModifierAction.CUSTOM:
                    break;
                case DifficultyModifierAction.CHANGE_TERRAIN:
                    break;
                case DifficultyModifierAction.ADD_ENEMY:
                    modifier = () => AddEnemyAction(settings.enemyType, settings.targetPosition, settings.parentObject);
                    break;
                case DifficultyModifierAction.REMOVE_ENEMY:
                    modifier = () => RemoveEnemyAction(settings.target);
                    break;
            }

            return modifier;
        }

        #region ACTIONS
        private void AddEnemyAction(EnemyType type, Vector3 position, Transform parent)
        {
            var enemy = _loadedEnemies[type];
            if (enemy == null)
            {
                enemy = LoadEnemyPrefab(type);
                _loadedEnemies.Add(type, enemy);
            }

            UnityEngine.Object.Instantiate(enemy, position, Quaternion.identity, parent);
        }

        //This is currently quite overengenired but it may be possible that we add exra logic to it in a future
        private void RemoveEnemyAction(GameObject target)
        {
            UnityEngine.Object.Destroy(target);
        }
        #endregion

        private GameObject LoadEnemyPrefab(EnemyType type)
        {
            return null;
        }
    }
}
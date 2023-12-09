using Enemies;
using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level.DynamicDifficulty.Modifiers
{
    internal class ModifierFactory
    {
        private static Dictionary<EnemyType, GameObject> _loadedEnemies = new();

        public Action GetLevelModifier(Modifier settings)
        {
            Action modifier = null;

            switch (settings.action)
            {
                case ModifierAction.CUSTOM:
                    break;
                case ModifierAction.CHANGE_TERRAIN:
                    break;
                case ModifierAction.ADD_ENEMY:
                    //modifier = () => AddEnemyAction(settings.enemyType, settings.position, settings.parentObject);
                    break;
                case ModifierAction.REMOVE_ENEMY:
                    //modifier = () => RemoveEnemyAction(settings.target);
                    break;
            }

            return modifier;
        }

        #region ACTIONS
        private void AddEnemyAction(EnemyType type, Vector3 position, Transform parent)
        {
            GameObject enemy;

            if (!_loadedEnemies.TryGetValue(type, out enemy))
            {
                enemy = LoadEnemyPrefab(type);
                _loadedEnemies.Add(type, enemy);
            }

            var obj = UnityEngine.Object.Instantiate(enemy, position, Quaternion.identity, parent);
        }

        //This is currently quite overengenired but it may be possible that we add exra logic to it in a future
        private void RemoveEnemyAction(GameObject target)
        {
            UnityEngine.Object.Destroy(target);
        }
        #endregion

        private GameObject LoadEnemyPrefab(EnemyType type)
        {
            string address = type.GetAddressableKey();
            //Debug.Log("Loading enemy from address: " + address);
            return Addressables.LoadAssetAsync<GameObject>(address).WaitForCompletion();
        }
    }
}
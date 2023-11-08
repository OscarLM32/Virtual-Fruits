using Enemies;
using System;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    public static class DifficultyModifierFactory
    {
        public static Action GetLevelModifier(DifficultyModifier settings)
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
                    modifier = () => AddEnemyAction(settings.enemyType, settings.targetPosition);
                    break;
                case DifficultyModifierAction.REMOVE_ENEMY:
                    modifier = () => RemoveEnemyAction(settings.target);
                    break;
            }

            return modifier;
        }

        #region ACTIONS
        private static void AddEnemyAction(EnemyType type, Vector2 position)
        {
            //get enemy prefab from the type specified in settings (probably from addressables)
            //get the data 
            //instantiate the enemy
        }

        //This is currently quite overengenired but it may be possible that we add exra logic to it in a future
        private static void RemoveEnemyAction(GameObject target)
        {
            UnityEngine.Object.Destroy(target);
        }
        #endregion
    }
}
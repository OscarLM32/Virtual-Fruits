using System;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    public static class DifficultyModifierFactory
    {
        public static Action GetLevelModifier(DifficultyModifier settings)
        {
            var actionType = settings.action;
            Action difficultyModifier = null;

            switch (actionType)
            {
                case DifficultyModifierAction.CUSTOM:
                    break;
                case DifficultyModifierAction.CHANGE_TERRAIN:
                    break;
                case DifficultyModifierAction.ADD_ENEMY:
                    difficultyModifier = RemoveEnemyAction(settings.target);
                    break;
                case DifficultyModifierAction.REMOVE_ENEMY:
                    break;
            }
            return null;
        }

        private static Action RemoveEnemyAction(GameObject target)
        {
            //get enemy prefab from the type specified in settings (probably from addressables)
            //get the data 
            //instantiate the enemy
            return null;
        }
    }
}
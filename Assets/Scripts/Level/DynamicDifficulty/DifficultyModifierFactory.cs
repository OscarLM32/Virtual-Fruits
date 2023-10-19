using UnityEngine;
using System;

namespace Level.DynamicDifficulty
{
    public static class DifficultyModifierFactory
    {
        public static Action GetLevelModifier(DifficultyModifierSetting setting)
        {
            var actionType = setting.action;
            Action difficultyModifier = null;

            switch (actionType)
            {
                case DifficultyModifierActionType.CUSTOM:
                    break;
                case DifficultyModifierActionType.CHANGE_TERRAIN:
                    break;
                case DifficultyModifierActionType.ADD_ENEMY:
                    difficultyModifier = () =>
                    {
                        //get enemy prefab from the type specified in settings (probably from addressables)
                        //get the data 
                        //instantiate the enemy
                    };
                    break;
                case DifficultyModifierActionType.REMOVE_ENEMY:
                    break;
            }
            return null;
        }
    }
}
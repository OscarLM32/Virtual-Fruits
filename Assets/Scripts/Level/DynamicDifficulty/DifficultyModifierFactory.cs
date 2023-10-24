using UnityEngine;
using System;

namespace Level.DynamicDifficulty
{
    public static class DifficultyModifierFactory
    {
        public static Action GetLevelModifier(DifficultyModifier setting)
        {
            var actionType = setting.action;
            Action difficultyModifier = null;

            switch (actionType)
            {
                case DifficultyModifierAction.CUSTOM:
                    break;
                case DifficultyModifierAction.CHANGE_TERRAIN:
                    break;
                case DifficultyModifierAction.ADD_ENEMY:
                    difficultyModifier = () =>
                    {
                        //get enemy prefab from the type specified in settings (probably from addressables)
                        //get the data 
                        //instantiate the enemy
                    };
                    break;
                case DifficultyModifierAction.REMOVE_ENEMY:
                    break;
            }
            return null;
        }
    }
}
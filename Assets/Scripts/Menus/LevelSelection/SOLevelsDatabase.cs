using DynamicDifficulty;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Menus.LevelSelection
{
    [CreateAssetMenu(fileName = "SOLevelsDatabase", menuName = "ScriptableObjects/LevelsDatabase")]
    public class SOLevelsDatabase : ScriptableObject, ISerializationCallbackReceiver
    {

        public LevelData this[string id]
        {
            get
            {
                foreach (var item in database)
                {
                    if (item.id == id) return item;
                }
                return null;
            }
        } 

        public List<LevelData> database;

        public void OnBeforeSerialize()
        {
            Difficulty[] difficulties = Enum.GetValues(typeof(Difficulty)) as Difficulty[];
            foreach(LevelData levelData in database)
            {
                foreach (Difficulty difficulty in difficulties)
                {
                    levelData.completionTimes.TryAdd(difficulty, 0);
                }
            }
        }

        public void OnAfterDeserialize()
        {

        }
    }
}
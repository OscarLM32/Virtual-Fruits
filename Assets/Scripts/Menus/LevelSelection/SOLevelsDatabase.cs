using Extensions.Serializables;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Menus.LevelSelection
{
    [CreateAssetMenu(fileName = "SOLevelsDatabase", menuName = "ScriptableObjects/LevelsDatabase")]
    public class SOLevelsDatabase : ScriptableObject
    {
        [Serializable]
        public class LevelData
        {
            public string id = "";
            public AssetReference reference = null;
            public bool unlocked;
        }

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
    }
}
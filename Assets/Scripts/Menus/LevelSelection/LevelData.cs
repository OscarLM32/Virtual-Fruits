

using DynamicDifficulty;
using Extensions.Serializables;
using System;
using UnityEngine.AddressableAssets;

namespace Menus.LevelSelection
{
    [Serializable]
    public class LevelData
    {
        public string id = "";
        public AssetReference reference = null;
        public bool unlocked;
        public SerializableDictionary<Difficulty, float> completionTimes = new();
    }
}
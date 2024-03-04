using System;
using UnityEngine.AddressableAssets;

namespace DynamicDifficulty
{
    [Serializable]
    public class DifficultySetting
    {
        public Difficulty difficulty;
        public AssetReference layoutReference;

        public DifficultySetting(Difficulty difficulty, AssetReference layoutReference = null)
        {
            this.difficulty = difficulty;
            this.layoutReference = layoutReference;
        }
    }
}
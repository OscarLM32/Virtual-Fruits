
using UnityEngine.AddressableAssets;

namespace Extensions
{
    public static class AddressablesExtensions
    {
        public static T LoadAssetSync<T>(this AssetReference assetRef)
        {
            var action = Addressables.LoadAssetAsync<T>(assetRef);
            action.WaitForCompletion();
            return action.Result;
        }
    }
}
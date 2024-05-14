using UnityEditor;
using UnityEngine.AddressableAssets;

//Singleton so that I can ensure that the info is unique
public class LevelDataSO : ScriptableSingleton<LevelDataSO>
{
    public string id;
    public AssetReference levelRef;
}

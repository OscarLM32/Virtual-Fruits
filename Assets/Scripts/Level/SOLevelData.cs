using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

//Singleton so that I can ensure that the info is unique
[CreateAssetMenu(fileName = "SOLevelData", menuName = "ScriptableObjects/LevelData")]
public class SOLevelData : ScriptableSingleton<SOLevelData>
{
    public string id;
    public AssetReference levelRef;
}

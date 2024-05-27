using DynamicDifficulty;
using Menus.LevelSelection;
using System;
using UnityEngine;

//Singleton so that I can ensure that the info is unique
[CreateAssetMenu(fileName = "SOLevelData", menuName = "ScriptableObjects/LevelData")]
public class SOSelectedLevelData : ScriptableObject, ISerializationCallbackReceiver
{
    public LevelData levelData;

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        var difficulties = Enum.GetValues(typeof(Difficulty)) as Difficulty[];
        foreach (var difficulty in difficulties)
        {
            levelData.completionTimes.TryAdd(difficulty, 0);
        }
    }
}

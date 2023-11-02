using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveLoadSystem : MonoBehaviour
{
    public string currentLevel = "1-1";
    public bool resetItemBitMap = false;
    public bool resetCollectedItems = false;

    private static SaveLoadSystem _instance;
    private GameData _gameDataSave;

    public static SaveLoadSystem I
    {
        get
        {
            if (_instance == null)
            {
                Init();
                //Debug.Log(Application.persistentDataPath);
            }
            return _instance;
        }
    }

    private static void Init()
    {
        GameObject obj = new GameObject("SaveLoadManager");
        _instance = obj.AddComponent<SaveLoadSystem>();
        _instance.Load();

        if (_instance.resetCollectedItems)
            _instance.ResetCollectedItems();

        if (_instance.resetItemBitMap)
            _instance.ResetItemBitMap();

        DontDestroyOnLoad(obj);
    }

    public string GetMaxLevelReached()
    {
        return _gameDataSave.maxLevelReached;
    }

    public List<bool> GetLevelBitMap()
    {
        if (_gameDataSave.levelItemBitMap.ContainsKey(currentLevel))
        {
            return _gameDataSave.levelItemBitMap[currentLevel];
        }
        _gameDataSave.levelItemBitMap.Add(currentLevel, new List<bool>());
        return _gameDataSave.levelItemBitMap[currentLevel];
    }

    public int GetTotalPickedFruits()
    {
        int sum = 0;
        if (_gameDataSave.collectedItems.Count == 0)
            return sum;

        foreach (var pickedItem in _gameDataSave.collectedItems)
        {
            sum += pickedItem.Value;
        }
        return sum;
    }

    private void CheckPointSave()
    {
        string path = Application.persistentDataPath + "/gameData.save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        formatter.Serialize(stream, _gameDataSave);
        stream.Close();
    }

    private void LevelEndSave()
    {
        string path = Application.persistentDataPath + "/gameData.save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        UpdateMaxLevelReached();

        formatter.Serialize(stream, _gameDataSave);
        stream.Close();
    }

    private void Load()
    {
        string path = Application.persistentDataPath + "/gameData.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            _gameDataSave = formatter.Deserialize(stream) as GameData;
            if (_gameDataSave.maxLevelReached == null)
            {
                _gameDataSave.maxLevelReached = "0-0";
            }
            stream.Close();
        }
        else
        {
            _gameDataSave = new GameData();
            Debug.Log("The save file was not found in path: " + path);
        }
    }

    private void ItemPicked(int itemType, int bitMapPos)
    {
        //Change the bit-map bit to 0 so the item does not spawn again
        _gameDataSave.levelItemBitMap[currentLevel][bitMapPos] = false;
        //Update the CollectedItemDictionary by one if the key has not been added I add it
        if (_gameDataSave.collectedItems.ContainsKey(itemType))
            _gameDataSave.collectedItems[itemType] += 1;
        else
            _gameDataSave.collectedItems.Add(itemType, 1);
    }

    //Inner reset of the bit map with the same number of objects as before
    private void ResetItemBitMap()
    {
        int itemNum = _gameDataSave.levelItemBitMap[currentLevel].Count;
        _gameDataSave.levelItemBitMap[currentLevel].Clear();
        _gameDataSave.levelItemBitMap[currentLevel] = new List<bool>(itemNum);
        for (int i = 0; i < itemNum; i++)
            _gameDataSave.levelItemBitMap[currentLevel].Add(true);
    }

    //Called by the item manager specifying a new number of items in the level 
    //items: the number of items that are in the level
    public void ResetItemBitMap(int items)
    {
        _gameDataSave.levelItemBitMap[currentLevel].Clear();
        _gameDataSave.levelItemBitMap[currentLevel] = new List<bool>(items);
        for (int i = 0; i < items; i++)
            _gameDataSave.levelItemBitMap[currentLevel].Add(true);
    }

    private void ResetCollectedItems()
    {
        foreach (var collectedItem in _gameDataSave.collectedItems)
        {
            _gameDataSave.collectedItems[collectedItem.Key] = 0;
        }
    }

    private void UpdateMaxLevelReached()
    {
        var currentLevelValues = currentLevel.Split("-");
        var maxLevelReachedValues = _gameDataSave.maxLevelReached.Split("-");

        if (Int32.Parse(currentLevelValues[0]) > Int32.Parse(maxLevelReachedValues[0]))
        {
            _gameDataSave.maxLevelReached = currentLevel;
        }
        else if (Int32.Parse(currentLevelValues[1]) > Int32.Parse(maxLevelReachedValues[1]))
        {
            _gameDataSave.maxLevelReached = currentLevel;
        }
    }

    private void OnEnable()
    {
        GameActions.ItemPicked += ItemPicked;
        GameActions.CheckpointReached += CheckPointSave;
        GameActions.LevelEndReached += LevelEndSave;
    }

    private void OnDisable()
    {
        GameActions.ItemPicked -= ItemPicked;
        GameActions.CheckpointReached -= CheckPointSave;
        GameActions.LevelEndReached -= LevelEndSave;
    }

    [Serializable]
    private class GameData
    {
        public string maxLevelReached = "0-0";
        public Dictionary<int, int> collectedItems = new Dictionary<int, int>();
        public Dictionary<string, List<bool>> levelItemBitMap = new Dictionary<string, List<bool>>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Tooltip("Resets the item collection bitmap when true")]
    public bool debug = false;
    
    private const String ITEM_TAG = "Item";
    
    [SerializeField]private List<bool> _pickedItems = new List<bool>();
    [SerializeField]private List<GameObject> _items = new List<GameObject>();

    private void Start()
    {
        GetItems();
        GetPickedItemsList();
        SetUpItems();
    }

    private void GetItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(ITEM_TAG);
        foreach (var item in items)
        {
            _items.Add(item);
        }
    }

    private void GetPickedItemsList()
    {
        _pickedItems = SaveLoadSystem.I.GetLevelBitMap();
        //This logic inside the "if" only makes sense in development, may be cut out on launch
        if (_pickedItems.Count != _items.Count || debug)
        {
            //if the size does not match to _items size I need to tell the save system to create a new one
            SaveLoadSystem.I.ResetItemBitMap(_items.Count);
            //Calling the function recursively is an overkill
            _pickedItems = SaveLoadSystem.I.GetLevelBitMap();
        }
    }

    private void SetUpItems()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].GetComponent<Item>().id = i;
            _items[i].SetActive(_pickedItems[i]);
        }
    }
}

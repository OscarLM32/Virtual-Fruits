using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public Sprite itemSprite;
}

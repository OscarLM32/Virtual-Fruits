using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class ItemSO : ScriptableObject
{
    public int itemType;
    public Sprite itemSprite;
}

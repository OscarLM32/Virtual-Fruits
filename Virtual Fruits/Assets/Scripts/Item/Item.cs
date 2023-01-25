using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public ItemSO itemSO;
    private const string COLLECTION_ANIMATION = "ItemCollectionAnimation";
    private int _itemType;
    private Animator _animator;


    private void Start()
    {
        _itemType = itemSO.itemType;
        _animator = GetComponent<Animator>();
        //TODO: change this non-working method to the one I use in the player animator
        _animator.SetInteger("Type", _itemType);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _animator.Play(COLLECTION_ANIMATION);
        //Tell the SaveLoadSystem that I have been picked
        GameActions.ItemPicked(_itemType, id);
        //Destroy the gameObject after playing the animation;
        Destroy(gameObject);
    }
    
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemSO.itemSprite;
    }
}
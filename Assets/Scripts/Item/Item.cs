using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class Item : MonoBehaviour
{
    private const string COLLECTION_ANIMATION = "ItemCollectionAnimation";
    private const float COLLECTION_ANIMATION_TIME = 0.5f;
    
    public int id;
    public ItemSO itemSO;
    

    [SerializeField]private int _itemType;
    private Animator _animator;
    private AudioManager _audioManager;

    private void Awake()
    {
        _itemType = (int)itemSO.itemType;
        _animator = GetComponent<Animator>();
        _animator.SetInteger("Type", _itemType);
        GetComponent<SpriteRenderer>().sprite = itemSO.itemSprite;
        _audioManager = GetComponent<AudioManager>();
    }
    
    private IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        GetComponent<Collider2D>().enabled = false;
        _audioManager.Play("ItemPickUp");
        _animator.Play(COLLECTION_ANIMATION);
        //Tell the SaveLoadSystem that I have been picked
        GameActions.ItemPicked(_itemType, id);
        //Destroy the gameObject after playing the animation;
        yield return new WaitForSeconds(COLLECTION_ANIMATION_TIME);
        Destroy(gameObject);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private const int THROW_SPEED = 15;
    private const int RETURN_SPEED = 25;
    
    [SerializeField]private Transform _player;
    private Collider2D _coll;
    private float _speed = THROW_SPEED;
    private Vector2 _currentTarget;
    private bool _throw = false;
    
    void OnEnable()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (_throw)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, _speed * Time.deltaTime);
            if ((Vector2) transform.position == _currentTarget)
            {
                _throw = !_throw;
                _coll.enabled = false;
                _speed = RETURN_SPEED;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);
            if ((Vector2) transform.position == (Vector2) _player.position)
            {
                GameActions.RetrieveWeapon();
                _speed = THROW_SPEED;
                gameObject.SetActive(false);
            }
        }
        transform.Rotate(Vector3.forward, 50);
    }

    public void Throw(Vector2 target)
    {
        gameObject.SetActive(true);
        transform.position = _player.position;
        _currentTarget = target;
        _throw = true;
        _coll.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _throw = false;
        _coll.enabled = false;
        _speed = RETURN_SPEED;
    }
}

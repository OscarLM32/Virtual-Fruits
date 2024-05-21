using System.Collections;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    private bool _platformFalling;
    private float _floatingTime = 0.75f;
    private float _timeAfterMotorStopToFall = 0.5f;

    private float _respawnTime = 3f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Set up dyamic difficulty settings
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_platformFalling) return;

        _platformFalling = true;
        StartCoroutine(Fall());
    }


    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(_floatingTime);
        //Stop animation
        yield return new WaitForSeconds(_timeAfterMotorStopToFall);
        //Unfreeze position Y
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTime);
        //Move platform to original position
        //Lock Y position and set gravity
        //Play floating animation
    }

}

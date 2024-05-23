using DynamicDifficulty;
using DynamicDifficulty.DynamicParametersScriptables;
using System.Collections;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public SOFloatingPlatformDynamicParameters dynamicParameters;

    private Vector2 _originalPos;

    private bool _platformFalling;
    private float _floatingTime = 0.75f;
    private float _timeAfterMotorStopToFall = 0.75f;

    private bool _respawning = false;
    private float _respawnTime = 3f;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _originalPos = transform.position;
    }

    void Start()
    {
        SetUpDynamicParameters();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_platformFalling) return;

        _platformFalling = true;
        StartCoroutine(Fall());
    }

    private void OnBecameInvisible()
    {
        StartCoroutine(Respawn());
    }


    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(_floatingTime);
        _animator.Play("Idle");
        yield return new WaitForSeconds(_timeAfterMotorStopToFall);
        _rb.gravityScale = 1;
    }

    private IEnumerator Respawn()
    {
        if (_respawning) yield break;

        _respawning = true;
        yield return new WaitForSeconds(_respawnTime);

        transform.position = _originalPos;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0;
        _animator.Play("Floating");

        _platformFalling = false;
        _respawning = false;
    }

}

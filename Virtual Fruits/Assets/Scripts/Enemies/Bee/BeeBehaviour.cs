using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class BeeBehaviour : MonoBehaviour
{
    private static class BeeAnimations
    {
        public static readonly string IDLE = "BeeIdle";
        public static readonly string ATTACK = "BeeAttack";
        public static readonly string HIT = "BeeHit";
    }

    public PhysicsMaterial2D ragdollMaterial;
    public ProjectileType projectileType;

    private Animator _animator;
    private string _patrolId;
    private AudioManager _audioManager;

    private float _attackCycleTime = 3f;
    private float _timeElapsed = 0f;
    private bool _stopAttacking = false;
    private Collider2D _collider;
    private Rigidbody2D _rb;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _patrolId = GetComponent<EnemyBasicPatrolling>().patrolId;
        _audioManager = GetComponent<AudioManager>();

        //I get all the possible colliders that the enemy has
        _collider = GetComponent<Collider2D>();

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_stopAttacking)
            return;
        if (_timeElapsed > _attackCycleTime)
        {
            StartCoroutine(AttackBehaviour());
            _timeElapsed = 0f;
        }

        _timeElapsed += Time.deltaTime;
    }

    private IEnumerator AttackBehaviour()
    {
        _animator.Play(BeeAnimations.ATTACK);
        yield return new WaitForSeconds(0.5f);
        //In case the bee gets hit while mid animation, we need to cancel this logic
        if (!_stopAttacking)
        {
            GameObject projectile = EnemyProjectilePool.I.GetProjectile(projectileType);
            projectile.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
            yield return new WaitForSeconds(0.16f);
            _animator.Play(BeeAnimations.IDLE);
            _audioManager.Play("Shoot");
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == (int)LayerValues.Weapon)
        {
            _stopAttacking = true;
            StartCoroutine(OnPlayerWeaponCollision(col.gameObject));
        }
    }

    private IEnumerator OnPlayerWeaponCollision(GameObject other)
    {
        //Stop patrolling
        DOTween.Pause(_patrolId);

        //Play the proper animation
        _animator.Play(BeeAnimations.HIT);

        _collider.enabled = false;

        //Launch the enemy
        LaunchEnemy(other);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void LaunchEnemy(GameObject other)
    {
        //It must have a collider because for the time being it should only be collided by the player 
        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
        int launchDirection = otherRb.velocity.x > 0 ? 1 : -1;
        _rb.sharedMaterial = ragdollMaterial;

        //TODO: add a little bit of randomness to the launch
        _rb.AddForce(new Vector2(800 * launchDirection, 550));
    }
}
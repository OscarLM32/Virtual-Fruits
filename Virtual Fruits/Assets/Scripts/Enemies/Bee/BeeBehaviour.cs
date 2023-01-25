using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;


public class BeeBehaviour : MonoBehaviour
{
    private static class BeeAnimations
    {
        public static readonly string IDLE = "BeeIdle";
        public static readonly string ATTACK = "BeeAttack";
        public static readonly string HIT  = "BeeHit";
    }
    
    public PhysicsMaterial2D ragdollMaterial;
    public GameObject stinger;

    private Animator _animator;
    private string _patrolId;
    
    private float _attackCycleTime = 3f;
    private float _timeElapsed = 0f;
    private bool _stopAttacking = false;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _patrolId = GetComponent<EnemyPatrolling>().patrolId;

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
            Vector2 stingerSpawnPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
            Instantiate(stinger, stingerSpawnPosition, Quaternion.identity);
            _animator.Play(BeeAnimations.IDLE);
        }
    }


    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        //Stop patrolling
        DOTween.Pause(_patrolId);
        //Play the proper animation
        _animator.Play(BeeAnimations.HIT);
        
        //Logic for launching the enemy like a ragdoll
        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        int launchDirection = otherRb.velocity.x > 0 ? 1 : -1;
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.sharedMaterial = ragdollMaterial;
        rb.AddForce(new Vector2(800 * launchDirection, 550));
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

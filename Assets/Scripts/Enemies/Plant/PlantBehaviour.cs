using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    private static class PlantAnimations
    {
        public static readonly string IDLE = "PlantIdle";
        public static readonly string ATTACK = "PlantAttack";
        public static readonly string HIT = "PlantHit";
    }

    private const float ATTACK_ANIMATION_TIME = 0.5f;
    
    public ProjectileType projectileType;

    private Animator _animator;
    private AudioManager _audioManager;
    [SerializeField]private float _attackCycleTime = 3f;
    private float _timeElapsed = 0f;
    private bool _hit = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioManager = GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (_hit)
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
        _animator.Play(PlantAnimations.ATTACK);
        yield return new WaitForSeconds(ATTACK_ANIMATION_TIME);

        if (_hit) yield break;
        
        GameObject projectile = EnemyProjectilePool.I.GetProjectile(projectileType);
        Bean beanScript = projectile.GetComponent<Bean>();
        //The direction of the sprite is inverted (looking to the left originally) so I have to invert the direction 
        beanScript.direction = (int)(Math.Abs(transform.localScale.x) / transform.localScale.x) * -1;
        projectile.transform.position = new Vector2(transform.position.x+0.5f, transform.position.y+0.1f);
        
        _animator.Play(PlantAnimations.IDLE);
        _audioManager.Play("Shoot");
    }

    private IEnumerator OnTriggerEnter2D(Collider2D coll)
    {
        _hit = true;
        GetComponent<Collider2D>().enabled = false;
        _animator.Play(PlantAnimations.HIT);
        GetComponent<SpriteRenderer>().DOFade(0, 2f);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

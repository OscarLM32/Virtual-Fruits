using DG.Tweening;
using Enemies.ShootingEnemyLogic;
using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Plant : ShootingEnemy
    {
        private static class PlantAnimations
        {
            public static readonly string IDLE = "PlantIdle";
            public static readonly string ATTACK = "PlantAttack";
            public static readonly string HIT = "PlantHit";
        }

        private Animator _animator;
        private AudioManager _audioManager;

        private const float _beforeAttackAnimationSyncTime = 0.45f;

        #region OVERRIDDEN METHODS

        protected override void OnStart()
        {
            _animator = GetComponent<Animator>();
            _audioManager = GetComponent<AudioManager>();
        }

        protected override void SetUpEnemy()
        {
            shootingDirection = new Vector2(-transform.localScale.x, 0).normalized;
            shootingPosition = transform.position;
        }

        protected override IEnumerator Attack()
        {
            _animator.Play(PlantAnimations.ATTACK);

            yield return StartCoroutine(Shoot(_beforeAttackAnimationSyncTime));

            _animator.Play(PlantAnimations.IDLE);
            _audioManager.Play("Shoot");
        }

        #endregion

        private IEnumerator OnTriggerEnter2D(Collider2D coll)
        {
            stopShooting = true;
            GetComponent<Collider2D>().enabled = false;
            _animator.Play(PlantAnimations.HIT);
            GetComponent<SpriteRenderer>().DOFade(0, 2f);
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }


    }
}
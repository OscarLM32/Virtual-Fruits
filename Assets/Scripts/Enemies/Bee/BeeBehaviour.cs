using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyBasicPatrolling))]
    public class BeeBehaviour : ShootingEnemy
    {
        private static class BeeAnimations
        {
            public static readonly string IDLE = "BeeIdle";
            public static readonly string ATTACK = "BeeAttack";
            public static readonly string HIT = "BeeHit";
        }

        public PhysicsMaterial2D ragdollMaterial;

        private Animator _animator;
        private Collider2D _collider;
        private Rigidbody2D _rb;
        private AudioManager _audioManager;

        private EnemyBasicPatrolling _patrolBehaviour;

        #region OVERRIDDEN METHODS

        protected override void OnStart()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _audioManager = GetComponent<AudioManager>();

            _patrolBehaviour = GetComponent<EnemyBasicPatrolling>();
        }

        protected override void SetUpEnemy()
        {
            shootingPosition = (Vector2)transform.position - new Vector2(0, 0.5f);
        }

        protected override IEnumerator Attack()
        {
            _animator.Play(BeeAnimations.ATTACK);
            if (stopBehaviour) yield break;

            yield return Shoot(0.5f, 0.16f);

            _animator.Play(BeeAnimations.IDLE);
            _audioManager.Play("Shoot");
        }

        #endregion

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer == (int)LayerValues.Weapon)
            {
                stopBehaviour = true;
                StartCoroutine(OnPlayerWeaponCollision(col.gameObject));
            }
        }

        private IEnumerator OnPlayerWeaponCollision(GameObject other)
        {
            //Stop patrolling
            _patrolBehaviour.PausePatrol();
            _collider.enabled = false;

            //Play the proper animation
            _animator.Play(BeeAnimations.HIT);

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
}
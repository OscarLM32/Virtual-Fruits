using DevSystems.CombatSystem;
using DynamicDifficulty;
using DynamicDifficulty.DynamicParametersScriptables;
using Enemies.ShootingEnemyLogic;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyBasicPatrolling))]
    public class Bee : ShootingEnemy, IKillable
    {
        private static class BeeAnimations
        {
            public static readonly string IDLE = "BeeIdle";
            public static readonly string ATTACK = "BeeAttack";
            public static readonly string HIT = "BeeHit";
        }

        [Header("Bee parameters")]
        [SerializeField] private SOBeeDynamicParameters _dynamicParameters;
        //TODO: most likely deletable
        public PhysicsMaterial2D ragdollMaterial;

        private Animator _animator;
        private Collider2D _collider;
        private Rigidbody2D _rb;
        private AudioManager _audioManager;

        private EnemyBasicPatrolling _patrolBehaviour;
        [SerializeField] private float _patrollingSpeed;

        private const int _protectionPower = 0;


        protected void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _audioManager = GetComponent<AudioManager>();

            _patrolBehaviour = GetComponent<EnemyBasicPatrolling>();
        }

        protected override void OnStart()
        {
            SetUpDynamicValues();
            _patrolBehaviour.SetUpPatrol(_patrollingSpeed);
            _patrolBehaviour.StartPatrolling();
        }

        private void SetUpDynamicValues()
        {
            var difficulty = DynamicDifficultyManager.I.GetEnemyDifficulty(EnemyType.BEE);
            var parameters = _dynamicParameters.parameters[difficulty];

            attackSpeed = parameters.attackSpeed;
            _patrollingSpeed = parameters.patrollingSpeed;
            projectileSpeed = parameters.projectileSpeed;
        }

        protected override IEnumerator Attack()
        {
            _animator.Play(BeeAnimations.ATTACK);

            yield return Shoot(0.5f,
                () => { UpdateShootingPosition(); },
                () => { _audioManager.Play("Shoot"); },
                0.16f);

            _animator.Play(BeeAnimations.IDLE);
        }

        private void UpdateShootingPosition()
        {
            shootingPosition = (Vector2)transform.position - new Vector2(0, 0.5f);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            IKillable killable = col.gameObject.GetComponent<IKillable>();
            killable?.Kill(new KillContext(1, EnemyType.BEE));
        }

        private IEnumerator OnPlayerWeaponCollision(GameObject other)
        {
            //Stop patrolling
            _patrolBehaviour.StopPatrolling();
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

        public void Kill(KillContext killContext)
        {
            if (killContext.attackPower <= _protectionPower) return;
            GameActions.OnEnemyKilled?.Invoke(EnemyType.BEE);
            StartCoroutine(OnKillBehavior());
        }

        public IEnumerator OnKillBehavior()
        {
            _animator.Play("BeeHit");
            _patrolBehaviour.StopPatrolling();
            stopShooting = true;
            StopCoroutine(Shoot());

            var colliders = GetComponents<BoxCollider2D>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}
using DevSystems.StateMachine;
using DynamicDifficulty;
using DynamicDifficulty.DynamicParametersScriptables;
using EditorSystems.Logger;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Enemies.Plant
{
    public class PlantStateMachine : BaseStateMachine<PlantStateMachine>
    {
        [SerializeField] private SOPlantDynamicParameters _dynamicParameters;

        #region States
        public PlantIdleState idleState { get; private set; }
        public PlantRunState runState { get; private set; }
        public PlantAttackState attackState { get; private set; }
        #endregion

        #region Components
        public Animator animator { get; private set; }
        public Rigidbody2D rb { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }
        #endregion

        #region Attack Variables
        public bool isPlayerInAttackRange { get; private set; }
        private const float _verticalPlayerDetectionRange = 4f;
        #endregion

        #region Run Variables
        public bool isPlayerInSafeZone { get; private set; }
        public bool canRun { get; private set; }
        [SerializeField]private Transform _groundChecker;
        [SerializeField]private Transform _wallChecker;
        [SerializeField]private float _safeZoneRange = 4f;
        #endregion

        public int playerDirection { get; set; }

        #region Unity Functions

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (!CheckComponentsIntegrity())
            {
                EditorLogger.LogError(LoggingSystem.ENEMY, $"{{{EnemyType.PLANT}}}: one or more components are not properly set");
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            var dynamicParameters = _dynamicParameters[DynamicDifficultyManager.I.genericDifficulty];
            SetUpStates(dynamicParameters);
            SetUpAttackCollider(dynamicParameters.attackRange);
            _safeZoneRange = dynamicParameters.fleetingRange;

            CurrentState = idleState;
            CurrentState.OnEnter();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsPlayer(collision.gameObject)) return;

            GetPlayerDirection(collision.gameObject);
            HandleDirection();
            CheckCanRun();
            isPlayerInAttackRange = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var other = collision.gameObject;
            if (!IsPlayer(other)) return;

            GetPlayerDirection(other);
            HandleDirection();
            CheckCanRun();
            CheckPlayerInSafeZone(other);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!IsPlayer(collision.gameObject)) return;

            isPlayerInAttackRange = false;
        }

        #endregion

        private bool CheckComponentsIntegrity()
        {
            bool exit = true;
            if(animator == null || rb == null || _groundChecker == null || _wallChecker == null)
            {
                exit = false;
            }
            return exit;
        }

        private void SetUpStates(PlantDynamicParameters dynamicParameters)
        {
            idleState = new PlantIdleState(this);
            runState = new PlantRunState(this, dynamicParameters.fleetingSpeed);
            attackState = new PlantAttackState(this, dynamicParameters.attackSpeed, dynamicParameters.proyectileSpeed);
        }

        private void SetUpAttackCollider(float attackRange)
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(attackRange * 2, _verticalPlayerDetectionRange);
            collider.offset = new Vector2(0, _verticalPlayerDetectionRange / 2 - 0.5f);
        }

        private bool IsPlayer(GameObject other)
        {
            return other.layer == (int)LayerValues.Player;
        }

        private void GetPlayerDirection(GameObject player)
        {
            playerDirection = 1;

            var playerPosX = player.transform.position.x;
            if(playerPosX < transform.position.x)
            {
                playerDirection = -1;
            }
        }

        private void HandleDirection()
        {
            transform.localScale = new Vector3(playerDirection, 1, 1);
        }

        private void CheckCanRun()
        {
            canRun = true;
            if(!Physics2D.OverlapBox(_groundChecker.position, new Vector2(1, 0.1f), 0, LayerMask.GetMask("Ground")) ||
               Physics2D.OverlapBox(_wallChecker.position, new Vector2(0.1f, 1), 0, LayerMask.GetMask("Ground")))
            {
                canRun = false;
            }
        }

        private void CheckPlayerInSafeZone(GameObject player)
        {
            var distance = Vector2.Distance(player.transform.position, transform.position);
            if (distance < _safeZoneRange)
            {
                isPlayerInSafeZone = true;
            }
            else
            {
                isPlayerInSafeZone = false;
            }
        }


    }
}
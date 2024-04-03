using DevSystems.StateMachine;
using EditorSystems.Logger;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Enemies.Plant
{
    public class PlantStateMachine : BaseStateMachine<PlantStateMachine>
    {
        public PlantIdleState idleState { get; private set; }
        public PlantRunState runState { get; private set; }
        public PlantAttackState attackState { get; private set; }


        public Animator animator { get; private set; }

        public bool isPlayerInAttackRange { get; private set; }
        public bool isPlayerInSafeZone { get; private set; }

        [SerializeField]private float _attackRange = 8f;
        private const float _verticalPlayerDetectionRange = 4f;
        [SerializeField]private float _safeZoneRange = 4f;
        public int playerDirection { get; set; }

        #region Unity Functions

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SetUpStates();
            SetUpAttackCollider();

            CurrentState = new PlantIdleState(this);
            CurrentState.OnEnter();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsPlayer(collision.gameObject)) return;

            GetPlayerDirection(collision.gameObject);
            isPlayerInAttackRange = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var other = collision.gameObject;
            if (!IsPlayer(other)) return;
            //maybe overkill
            if (IsBelow(other)) return;

            GetPlayerDirection(other);

            var distance = Vector2.Distance(other.transform.position, transform.position);
            if(distance < _safeZoneRange)
            {
                isPlayerInSafeZone = true;
            }
            else
            {
                isPlayerInSafeZone = false;
            }
        }

        #endregion

        private void SetUpStates()
        {
            idleState = new PlantIdleState(this);
            runState = new PlantRunState(this);
            attackState = new PlantAttackState(this);
        }

        private void SetUpAttackCollider()
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(_attackRange, _verticalPlayerDetectionRange);
            collider.offset = new Vector2(0, _verticalPlayerDetectionRange / 2);
        }

        private bool IsPlayer(GameObject other)
        {
            return other.layer != (int)LayerValues.Player;
        }

        private bool IsBelow(GameObject other)
        {
            return other.transform.position.y < transform.position.y;
        }

        private void GetPlayerDirection(GameObject player)
        {

        }


    }
}
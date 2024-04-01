
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

namespace Enemies.Bunny
{
    [RequireComponent(typeof(BunnyIdleState), typeof(BunnyJumpState), typeof(BunnyRunState))]
    [RequireComponent(typeof(BunnyAttackState))]
    public class BunnyStateMachine : MonoBehaviour
    {
        private BunnyIdleState _idleState;
        private BunnyRunState _runState;
        private BunnyJumpState _jumpState;
        private BunnyAttackState _attackState;
        private MonoBehaviour _currentState;

        [Header("Checkers")]
        [SerializeField] private Transform _groundChecker;
        //Attack variables
        [SerializeField] private Collider2D _attackRangeChecker;
        private float _attackChargeTime = 1f;
        private bool _isAttacking = false;


        [Header("Initial patrol point")]
        [SerializeField] private BunnyPatrolPoint _initialPatrolPoint;
        private BunnyPatrolPoint _currentPatrolPoint;
        private Vector2 _attackTo;

        private Rigidbody2D _rb;

        private float _lastPosition;
        private int _lastFacingDirection = -1;

        #region Unity Functions

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _idleState = GetComponent<BunnyIdleState>();
            _runState = GetComponent<BunnyRunState>();
            _jumpState = GetComponent<BunnyJumpState>();
            _attackState = GetComponent<BunnyAttackState>();

            _attackState.SetDynamicValues(5, 1);

            _currentState = _idleState;
            _currentState.enabled = true;

            _currentPatrolPoint = _initialPatrolPoint;

            transform.position = _initialPatrolPoint.transform.position;
            _lastPosition = transform.position.x;
        }

        private void OnEnable()
        {
            _attackState.OnAttackFinished += OnAttackFinished;
        }

        private void OnDisable()
        {
            _attackState.OnAttackFinished -= OnAttackFinished;
        }

        private void Start()
        {
            StartCoroutine(HandlePatrolAction(_initialPatrolPoint.GetFirstAction()));
        }

        private void Update()
        {
            HandleFacingDirection();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == (int)LayerValues.Player)
            {
                _isAttacking = true;
                _attackTo = collision.transform.position;
                StartCoroutine(HandleAttack());
            }
        }

        #endregion

        public void PatrolPointEntered(BunnyPatrolPoint patrolPoint)
        {
            if (patrolPoint == _currentPatrolPoint) return;

            StartCoroutine(HandlePatrolAction(patrolPoint.GetNextAction(_currentPatrolPoint)));
            _currentPatrolPoint = patrolPoint;
        }

        private void OnAttackFinished()
        {

        }

        private void SwitchState(MonoBehaviour newState)
        {
            _currentState.enabled = false;
            _currentState = newState;
            _currentState.enabled = true;
        }

        private void HandleFacingDirection()
        {
            var currentPosition = transform.position.x;
            var xVelocity = (currentPosition - _lastPosition)/Time.deltaTime;
            if(xVelocity == 0)
            {
                SetFacingDirection(_lastFacingDirection);
            }
            else if(xVelocity > 0)
            {
                SetFacingDirection(-1);
                _lastFacingDirection = -1;
            }
            else
            {
                SetFacingDirection(1);
                _lastFacingDirection = 1;
            }
            _lastPosition = currentPosition;
        }

        private void SetFacingDirection(int direction)
        {
            var localScale = transform.localScale;
            transform.localScale = new Vector3(direction, localScale.y, localScale.z);
        }

        private IEnumerator HandleAttack()
        {
            SwitchState(_idleState);
            yield return new WaitForSeconds(_attackChargeTime);

            _attackState.SetUpAttack(_attackTo);
            SwitchState(_attackState);
        }


        private IEnumerator HandlePatrolAction(BunnyPatrolAction patrolAction)
        {
            if(patrolAction.idleTime > 0)
            {
                SwitchState(_idleState);
                yield return new WaitForSeconds(patrolAction.idleTime);
            }

            var nextPatrolPointPos = patrolAction.nextPatrolPoint.position;
            switch (patrolAction.action)
            {
                case BunnyPatrolActionType.RUN:
                    HandleRunAction(nextPatrolPointPos);
                    break;
                case BunnyPatrolActionType.JUMP:
                    HandleJumpAction(nextPatrolPointPos);
                    break;
            }
        }

        private void HandleRunAction(Vector2 moveTo)
        {
            _runState.SetUpMove(transform.position, moveTo);
            SwitchState(_runState);
        }

        private void HandleJumpAction(Vector2 jumpTo)
        {
            _jumpState.SetUpJump(jumpTo);
            SwitchState(_jumpState);
        }
    }
}
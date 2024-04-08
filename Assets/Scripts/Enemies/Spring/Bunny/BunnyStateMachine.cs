
using DynamicDifficulty;
using DynamicDifficulty.DynamicParametersScriptables;
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
        public SOBunnyDynamicPrameters dynamicParameters;

        private BunnyIdleState _idleState;
        private BunnyRunState _runState;
        private BunnyJumpState _jumpState;
        private BunnyAttackState _attackState;
        private MonoBehaviour _currentState;

        private float _attackChargeTime = 1f;
        private bool _isAttacking = false;

        [SerializeField] private BunnyPatrolPoint _initialPatrolPoint;
        [SerializeField]private BunnyPatrolPoint _currentPatrolPoint;
        private BunnyPatrolPoint _cachedPoint = null;
        private Vector2 _attackTo;
        [SerializeField] private float _idleTimeFactor = 1;

        private float _lastPosition;
        private int _lastFacingDirection = -1;

        #region Unity Functions

        private void Awake()
        {
            SetUpComponents();

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
            SetUpDynamicValues();
            StartCoroutine(HandlePatrolAction(_initialPatrolPoint.GetFirstAction()));
        }

        private void Update()
        {
            HandleFacingDirection();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var other = collision.gameObject;
            if (other.layer == (int)LayerValues.Player && IsOtherGrounded(other))
            {
                _isAttacking = true;
                _attackTo = collision.transform.position;
                StopCoroutine("HandlePatrolAction");
                StartCoroutine(HandleAttack());
            }
        }

        #endregion

        private void SetUpComponents()
        {
            _idleState = GetComponent<BunnyIdleState>();
            _runState = GetComponent<BunnyRunState>();
            _jumpState = GetComponent<BunnyJumpState>();
            _attackState = GetComponent<BunnyAttackState>();
        }

        private void SetUpDynamicValues()
        {
            var ddValues = dynamicParameters[DynamicDifficultyManager.I.GetEnemyDifficulty(EnemyType.BUNNY)];

            _attackChargeTime = ddValues.attackChargeTime;
            _idleTimeFactor = ddValues.idleTimeFactor;
            _attackState.SetDynamicValues(ddValues.maxJumpTime, ddValues.maxAttackDistance);
            _runState.SetDynamicValues(ddValues.patrollingSpeed);
        }

        public void PatrolPointEntered(BunnyPatrolPoint patrolPoint)
        {
            if (patrolPoint == _currentPatrolPoint) return;

            if (_isAttacking)
            {
                Debug.Log("cached action");
                _cachedPoint = patrolPoint;
                return;
            }

            StartCoroutine(HandlePatrolPoint(patrolPoint));
        }

        private void OnAttackFinished()
        {
            _isAttacking = false;

            if(_cachedPoint != null)
            {
                StartCoroutine(HandlePatrolPoint(_cachedPoint));
                _cachedPoint = null;
            }
            else
            {
                _runState.SetUpMove(transform.position);
                SwitchState(_runState);
            }
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

        private bool IsOtherGrounded(GameObject other)
        {
            return Physics2D.OverlapBox(other.transform.position, new Vector2(0.1f, 1.1f), 0, LayerMask.GetMask("Ground"));
        }

        private IEnumerator HandleAttack()
        {
            SwitchState(_idleState);
            yield return new WaitForSeconds(_attackChargeTime);

            _attackState.SetUpAttack(_attackTo);
            SwitchState(_attackState);
        }


        private IEnumerator HandlePatrolPoint(BunnyPatrolPoint patrolPoint)
        {
            var patrolAction = patrolPoint.GetNextAction(_currentPatrolPoint);

            yield return StartCoroutine(HandlePatrolAction(patrolAction, patrolPoint));
        }

        private IEnumerator HandlePatrolAction(BunnyPatrolAction patrolAction, BunnyPatrolPoint patrolPoint = null)
        {
            if (patrolAction.idleTime > 0)
            {
                SwitchState(_idleState);
                var totalIdle = patrolAction.idleTime * _idleTimeFactor;
                yield return new WaitForSeconds(totalIdle);
            }

            if (_isAttacking) yield break;
            if (patrolPoint != null) _currentPatrolPoint = patrolPoint;

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
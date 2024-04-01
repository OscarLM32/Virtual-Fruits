
using JetBrains.Annotations;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Enemies.Bunny
{
    [RequireComponent(typeof(BunnyIdleState), typeof(BunnyJumpState), typeof(BunnyRunState))]
    public class BunnyStateMachine : MonoBehaviour
    {
        private BunnyIdleState _idleState;
        private BunnyRunState _runState;
        private BunnyJumpState _jumpState;

        private MonoBehaviour _currentState;

        [SerializeField] private Transform _groundChecker;
        private bool _isGrounded = true;

        [Space]
        [SerializeField] private BunnyPatrolPoint _initialPatrolPoint;
        private BunnyPatrolPoint _currentPatrolPoint;

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

            _currentState = _idleState;
            _currentState.enabled = true;

            _currentPatrolPoint = _initialPatrolPoint;

            transform.position = _initialPatrolPoint.transform.position;
            _lastPosition = transform.position.x;
        }

        private void Start()
        {
            StartCoroutine(HandlePatrolAction(_initialPatrolPoint.GetFirstAction()));
        }

        private void Update()
        {
            HandleGrounded();
            HandleFacingDirection();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var patrolPoint = collision.GetComponent<BunnyPatrolPoint>();
            if(patrolPoint != null && patrolPoint != _currentPatrolPoint)
            {
                StartCoroutine(HandlePatrolAction(patrolPoint.GetNextAction(_currentPatrolPoint)));
                _currentPatrolPoint = patrolPoint;
            }
        }

        #endregion

        private void SwitchState(MonoBehaviour newState)
        {
            _currentState.enabled = false;
            _currentState = newState;
            _currentState.enabled = true;
        }

        private void HandleGrounded()
        {

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

using System.Collections;
using UnityEngine;

namespace Enemies.Bunny
{
    [RequireComponent(typeof(BunnyIdleState), typeof(BunnyJumpState), typeof(BunnyRunState))]
    public class BunnyStateMachine : MonoBehaviour
    {
        private static class BunnyAnimations
        {
            public static readonly string IDLE = "BunnyIdle";
            public static readonly string RUN = "BunnyRun";
            public static readonly string JUMP = "BunnyJump";
            public static readonly string FALL = "BunnyFall";
            public static readonly string HIT = "BunnyHit";
        }

        private BunnyIdleState _idleState;
        private BunnyRunState _runState;
        private BunnyJumpState _jumpState;

        private MonoBehaviour _currentState;

        [SerializeField] private Transform _groundChecker;
        private bool _isGrounded = true;

        [Space]
        [SerializeField] private BunnyPatrolPoint _initialPatrolPoint;
        private BunnyPatrolPoint _currentPatrolPoint;

        #region Unity Functions

        private void Awake()
        {
            _idleState = GetComponent<BunnyIdleState>();
            _runState = GetComponent<BunnyRunState>();
            _jumpState = GetComponent<BunnyJumpState>();

            _currentState = _idleState;
            _currentState.enabled = true;

            _currentPatrolPoint = _initialPatrolPoint;

            transform.position = _initialPatrolPoint.transform.position;
        }

        private void Start()
        {
            StartCoroutine(HandlePatrolAction(_initialPatrolPoint.GetFirstAction()));
        }

        private void Update()
        {
            HandleGrounded();
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
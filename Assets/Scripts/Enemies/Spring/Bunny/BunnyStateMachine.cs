
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

        [SerializeField]private Transform _groundChecker; 


        private BunnyIdleState _idleState;
        private BunnyRunState _runState;
        private BunnyJumpState _jumpState;

        private MonoBehaviour _currentState;

        #region Unity Functions

        private void Awake()
        {
            _idleState = GetComponent<BunnyIdleState>();
            _runState = GetComponent<BunnyRunState>();
            _jumpState = GetComponent<BunnyJumpState>();

            _currentState = _idleState;
            _currentState.enabled = true;
        }

        private void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
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

    }
}
using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerJumpDownPlatformState : PlayerBaseState
    {
        private float _minIgnoreTime = 0.2f;
        private float _timeElapsed = 0f;
        public PlayerJumpDownPlatformState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            _timeElapsed = 0;
            Context.IsJumpingDownPlatform = true;
            Physics2D.IgnoreLayerCollision(Context.PlatformLayerID, Context.PlayerLayerID, true);
        }

        public override void UpdateState()
        {
            if (!Context.IsJumpDownPlatformPressed)
                _timeElapsed += Time.deltaTime;
            else
                _timeElapsed = 0;

            if (_timeElapsed > _minIgnoreTime || Context.IsGrounded)
            {
                CheckSwitchStates();
            }
        }

        public override void ExitState()
        {
            Physics2D.IgnoreLayerCollision(Context.PlatformLayerID, Context.PlayerLayerID, false);
            Context.IsJumpingDownPlatform = false;
        }

        public override void InitializeSubState() { }

        public override void CheckSwitchStates()
        {
            if (Context.CurrentMovement.x == 0)
            {
                SwitchState(PlayerState.IDLE);
                return;
            }
            SwitchState(PlayerState.MOVEMENT);

        }
    }
}
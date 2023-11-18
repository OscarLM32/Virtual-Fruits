using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerWallJumpState : PlayerBaseState
    {
        private float _halfTimeToApex;
        private float _timeElapsed = 0;

        public PlayerWallJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) 
        {
            
        }

        public override void EnterState()
        {
            _halfTimeToApex = Context.CurrentWallJumpTimeToApex * 0.5f;
            _timeElapsed = 0;
        }

        public override void UpdateState()
        {
            //Maybe I should encapsulate this logic inside teh CheckSwitchState
            if (_timeElapsed > _halfTimeToApex)
            {
                CheckSwitchStates();
            }
            _timeElapsed += Time.deltaTime;
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {

        }

        public override void CheckSwitchStates()
        {
            SwitchState(PlayerState.MOVEMENT);
        }
    }
}
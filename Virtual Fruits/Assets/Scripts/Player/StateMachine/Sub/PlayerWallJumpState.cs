using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerBaseState
{
    private float _timeToHalfApex;
    private float _timeElapsed = 0;
    
    public PlayerWallJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        _timeToHalfApex = Context.WallJumpsData[Context.WallJumpsCount].TimeToApex * 0.5f;
        _timeElapsed = 0;
    }

    public override void UpdateState()
    {
        if (_timeElapsed > _timeToHalfApex)
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
        SwitchState(Factory.Movement());
    }
}

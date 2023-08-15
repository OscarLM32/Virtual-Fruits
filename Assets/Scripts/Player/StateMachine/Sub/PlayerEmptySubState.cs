using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is probably not the best approach for the problem.
public class PlayerEmptySubState : PlayerBaseState
{
    public PlayerEmptySubState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {

    }
}
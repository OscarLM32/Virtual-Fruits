using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState, IRootState
{
    private enum Sounds
    {
        Attack
    }
    
    private const string ATTACK_ANIMATION = "PlayerAttack";
    private Vector2 _finalAttackPosition;

    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context.debugText.text = "STATE: ATTACKING";
        InitializeSubState();
        CalculateFinalAttackPosition();
        Context.PlayerAudioManager.Play(Sounds.Attack.ToString());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Weapon.Throw(_finalAttackPosition);
        Context.RequireNewAttackPress = true;
        Context.IsWeaponReady = false;
    }

    public override void InitializeSubState()
    {
        SetSubState(Context.CurrentMovementInput.x == 0 ? Factory.Idle() : Factory.Movement());
    }

    public override void CheckSwitchStates()
    {
        SwitchState(Factory.Grounded());
    }

    public void HandleGravity()
    {
        Context.Rb2D.gravityScale = 0; // No gravity;
    }

    public void HandleAnimation()
    {
        Context.PlayerAnimator.Play(ATTACK_ANIMATION);
    }

    private void CalculateFinalAttackPosition()
    {
        _finalAttackPosition.x = (float)Math.Cos(Context.AttackAngle);
        _finalAttackPosition.y = (float)Math.Sin(Context.AttackAngle);

        _finalAttackPosition *= Context.MaxAttackDistance;
        _finalAttackPosition += (Vector2)Context.gameObject.transform.position;
    }
    
}

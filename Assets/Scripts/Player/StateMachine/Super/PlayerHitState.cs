using UnityEngine;

//TODO: This logic should be handled in a different script
public class PlayerHitState : PlayerBaseState, IRootState
{
    private static class HitAnimations
    {
        public static readonly string BOUNCE_BACK = "PlayerBounceBack";
        public static readonly string DEATH = "PlayerDeath";
    }

    private enum Sounds
    {
        Death,
        BounceBack
    }

    private const int BOUNCE_BACK_FORCE = 400;
    private const float BOUNCE_BACK_TIME = 0.7f;
    private const float DEATH_TIME = 1.5f;
    private float _exitTime = 0f;
    private float _timeElapsed = 0;

    public PlayerHitState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        //Context.debugText.SetText("State: HIT"); 

        _timeElapsed = 0;
        InitializeSubState();
        HandleGravity();
        HandleAnimation();

        IgnoreThreatCollisions(true);
        SetTimer();
        HandleBounceBack();
        HandleSound();
    }

    public override void UpdateState()
    {
        _timeElapsed += Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        if (Context.PlayerDead)
            GameActions.PlayerDeath();

        IgnoreThreatCollisions(false);

        Context.PlayerHit = false;
        Context.PlayerDead = false;
        Context.PlayerBounceBack = false;
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Empty());
    }

    public override void CheckSwitchStates()
    {
        if (_timeElapsed < _exitTime)
            return;

        if (Context.IsGrounded)
        {
            SwitchState(Factory.Grounded());
            return;
        }

        if (Context.IsGrapplingWall)
        {
            SwitchState(Factory.GrapplingWall());
            return;
        }

        SwitchState(Factory.Falling());
    }

    private void SetTimer()
    {
        if (Context.PlayerBounceBack)
            _exitTime = BOUNCE_BACK_TIME;
        else
            _exitTime = DEATH_TIME;
    }

    public void HandleGravity()
    {
        Context.Rb2D.gravityScale = Context.JumpingGravityFactor;
    }

    public void HandleAnimation()
    {
        Context.PlayerAnimator.Play(Context.PlayerBounceBack ? HitAnimations.BOUNCE_BACK : HitAnimations.DEATH);
    }

    private void HandleBounceBack()
    {
        //TODO: create a bouncy material (not the ragdoll one) and apply it while in the state
        Context.Rb2D.AddForce(new Vector2(-1 * Context.LastFacingDirection * BOUNCE_BACK_FORCE, 0));
    }

    private void IgnoreThreatCollisions(bool active)
    {
        Physics2D.IgnoreLayerCollision(Context.EnemyLayerID, Context.PlayerLayerID, active);
        Physics2D.IgnoreLayerCollision(Context.ProjectileLayerID, Context.PlayerLayerID, active);
    }

    private void HandleSound()
    {
        if (Context.PlayerDead)
            Context.PlayerAudioManager.Play(Sounds.Death.ToString());
        else
            Context.PlayerAudioManager.Play(Sounds.BounceBack.ToString());
    }
}

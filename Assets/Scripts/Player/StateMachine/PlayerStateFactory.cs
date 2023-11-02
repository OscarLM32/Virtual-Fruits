using System.Collections.Generic;

public class PlayerStateFactory
{
    private PlayerStateMachine _context;
    private Dictionary<States, PlayerBaseState> _cache = new Dictionary<States, PlayerBaseState>();

    private enum States
    {
        empty,
        idle,
        movement,
        jumpDownPlatform,
        wallJump,
        grounded,
        jumping,
        falling,
        dashing,
        gliding,
        grapplingWall,
        hit,
        attack
    }

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _cache[States.empty] = new PlayerEmptySubState(_context, this);
        _cache[States.idle] = new PlayerIdleState(_context, this);
        _cache[States.movement] = new PlayerMovementState(_context, this);
        _cache[States.jumpDownPlatform] = new PlayerJumpDownPlatformState(_context, this);
        _cache[States.wallJump] = new PlayerWallJumpState(_context, this);
        _cache[States.grounded] = new PlayerGroundState(_context, this);
        _cache[States.jumping] = new PlayerJumpingState(_context, this);
        _cache[States.falling] = new PlayerFallState(_context, this);
        _cache[States.dashing] = new PlayerDashingState(_context, this);
        _cache[States.gliding] = new PlayerGlidingState(_context, this);
        _cache[States.grapplingWall] = new PlayerGrapplingWallState(_context, this);
        _cache[States.hit] = new PlayerHitState(_context, this);
        _cache[States.attack] = new PlayerAttackState(_context, this);
    }

    public PlayerBaseState Empty()
    {
        return _cache[States.empty];
    }

    public PlayerBaseState Idle()
    {
        return _cache[States.idle];
    }

    public PlayerBaseState Movement()
    {
        return _cache[States.movement];
    }

    public PlayerBaseState JumpDownPlatform()
    {
        return _cache[States.jumpDownPlatform];
    }

    public PlayerBaseState WallJump()
    {
        return _cache[States.wallJump];
    }

    public PlayerBaseState Grounded()
    {
        return _cache[States.grounded];
    }
    public PlayerBaseState Jumping()
    {
        return _cache[States.jumping];
    }

    public PlayerBaseState Falling()
    {
        return _cache[States.falling];
    }

    public PlayerBaseState Dashing()
    {
        return _cache[States.dashing];
    }

    public PlayerBaseState Gliding()
    {
        return _cache[States.gliding];
    }

    public PlayerBaseState GrapplingWall()
    {
        return _cache[States.grapplingWall];
    }

    public PlayerBaseState Hit()
    {
        return _cache[States.hit];
    }

    public PlayerBaseState Attack()
    {
        return _cache[States.attack];
    }

}

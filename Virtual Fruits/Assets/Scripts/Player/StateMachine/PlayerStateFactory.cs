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
        grounded,
        jumping,
        falling,
        dashing,
        gliding,
        grapplingWall
    }
   
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _cache[States.empty] = new PlayerEmptySubState(_context, this);
        _cache[States.idle] = new PlayerIdleState(_context, this);
        _cache[States.movement] = new PlayerMovementState(_context, this);
        _cache[States.grounded] = new PlayerGroundState(_context, this);
        _cache[States.jumping] = new PlayerJumpingState(_context, this);
        _cache[States.falling] = new PlayerFallState(_context, this);
        _cache[States.dashing] = new PlayerDashingState(_context, this);
        _cache[States.gliding] = new PlayerGlidingState(_context, this);
        _cache[States.grapplingWall] = new PlayerGrapplingWallState(_context, this);
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

}

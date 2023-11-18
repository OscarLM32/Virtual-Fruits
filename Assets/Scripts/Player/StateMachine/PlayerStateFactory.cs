using System.Collections.Generic;

namespace Player.StateMachine
{
    public partial class PlayerStateFactory
    {
        private PlayerStateMachine _context;
        private Dictionary<PlayerState, PlayerBaseState> _cache = new Dictionary<PlayerState, PlayerBaseState>();

        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext;
            _cache[PlayerState.EMPTY] = new PlayerEmptySubState(_context, this);
            _cache[PlayerState.IDLE] = new PlayerIdleState(_context, this);
            _cache[PlayerState.MOVEMENT] = new PlayerMovementState(_context, this);
            _cache[PlayerState.JUMP_DOWN_PLATFORM] = new PlayerJumpDownPlatformState(_context, this);
            _cache[PlayerState.WALL_JUMP] = new PlayerWallJumpState(_context, this);
            _cache[PlayerState.GROUNDED] = new PlayerGroundState(_context, this);
            _cache[PlayerState.JUMPING] = new PlayerJumpingState(_context, this);
            _cache[PlayerState.FALLING] = new PlayerFallState(_context, this);
            _cache[PlayerState.DASHING] = new PlayerDashingState(_context, this);
            _cache[PlayerState.GLIDING] = new PlayerGlidingState(_context, this);
            _cache[PlayerState.GRAPPLING_WALL] = new PlayerGrapplingWallState(_context, this);
            _cache[PlayerState.HIT] = new PlayerHitState(_context, this);
            _cache[PlayerState.ATTACK] = new PlayerAttackState(_context, this);
        }

        public PlayerBaseState GetState(PlayerState state)
        {
            return _cache[state];
        }

    }
}
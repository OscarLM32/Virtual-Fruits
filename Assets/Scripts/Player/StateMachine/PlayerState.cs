namespace Player.StateMachine
{
    public enum PlayerState
    {
        IDLE,
        MOVEMENT,
        JUMP_DOWN_PLATFORM,
        WALL_JUMP,
        GROUNDED,
        JUMPING,
        FALLING,
        DASHING,
        GLIDING,
        GRAPPLING_WALL,
        HIT,
        ATTACK
    }
}
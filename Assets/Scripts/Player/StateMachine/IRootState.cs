namespace Player.StateMachine
{
    public interface IRootState
    {
        public void HandleGravity();
        public void HandleAnimation();
    }
}

using DevSystems.StateMachine;
using UnityEngine;

namespace Enemies.Plant
{
    public class PlantRunState : BaseState<PlantStateMachine>
    {
        //TODO: implement runAnimation
        private float _speed = 2f;

        public PlantRunState(PlantStateMachine context) : base(context)
        {
        }

        public override void OnEnter()
        {
            HandleVelocity();
        }

        public override void OnUpdate()
        {
            HandleVelocity();
            HandleSpriteDirection();
            CheckSwitchState();
        }

        protected override void CheckSwitchState()
        {
            if(context.isPlayerInAttackRange && !context.canRun)
            {
                SwitchState(context.attackState);
                return;
            }

            if (!context.isPlayerInAttackRange)
            {
                SwitchState(context.idleState);
                return;
            }
        }

        protected override void OnExit()
        {
            context.rb.velocity = Vector2.zero;
        }

        private void HandleVelocity()
        {
            var velocity = new Vector2(_speed * -context.playerDirection, 0);
            context.rb.velocity = velocity;
        }

        private void HandleSpriteDirection()
        {
            context.transform.localScale = new Vector3(context.playerDirection, 1, 1);
        }
    }
}
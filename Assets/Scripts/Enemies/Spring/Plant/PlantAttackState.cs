using UnityEngine;
using DevSystems.StateMachine.SpecificStates;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

namespace Enemies.Plant
{
    public class PlantAttackState : ShootingState<PlantStateMachine>
    {
        private readonly Vector2 _shootingOffset;
        private const float _animationSyncTime = 0.55f;
        private float _animationSyncElapsedTime = 0;

        public PlantAttackState(PlantStateMachine context, float attackSpeed, float projectileSpeed) : base(context)
        {
            projectileType = ShootingEnemyLogic.ProjectileType.Bean;

            this.attackSpeed = attackSpeed;
            this.projectileSpeed = projectileSpeed;

            //TODO: set up a proper offset
            _shootingOffset = Vector2.zero;
        }

        public override void OnEnter()
        {
            HandleSpriteDirection();
            SetUpShot();
        }

        public override void OnUpdate()
        {
            HandleShot();
            CheckSwitchState();
        }

        protected override void CheckSwitchState()
        {
            if (context.isPlayerInSafeZone && context.canRun)
            {
                SwitchState(context.runState);
                return;
            }

            if (!context.isPlayerInAttackRange)
            {
                SwitchState(context.idleState);
            }
        }

        protected override void OnExit()
        {
            elapsedTime = 0;
            context.spriteRenderer.flipX = false;
        }

        private void HandleSpriteDirection()
        {
            if(context.playerDirection == context.transform.localScale.x)
            {
                context.spriteRenderer.flipX = true;
            }
        }

        private void SetUpShot()
        {
            shootingDirection = new Vector2(context.playerDirection, 0);
            shootingPosition = context.transform.position + (Vector3)_shootingOffset;
        }


        //Very bade code. My non-monobehaviour approach makes handling time hard. Since unity behaviour and tasks do not work well
        private void HandleShot()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < attackSpeed) return;

            _animationSyncElapsedTime += Time.deltaTime;
            context.animator.Play("PlantAttack");
            if(_animationSyncElapsedTime > _animationSyncTime)
            {
                Shoot();
                _animationSyncElapsedTime = 0;
                elapsedTime = 0;
                context.animator.Play("PlantIdle");
            }
        }



    }
}
using Enemies.ShootingEnemyLogic;
using System;
using UnityEngine;

namespace DevSystems.StateMachine.SpecificStates
{
    public abstract class ShootingState<T> : BaseState<T> where T : BaseStateMachine<T>
    {
        protected ProjectileType projectileType;
        protected Vector2 shootingPosition;
        protected Vector2 shootingDirection;

        protected Action beforeShotAction;
        protected Action afterShotAction;

        protected float projectileSpeed;
        protected float attackSpeed;
        protected float elapsedTime;

        protected ShootingState(T context) : base(context)
        {
        }

        protected void SetUpShot(Vector2 shootingPosition, Vector2 shootingDirection = default)
        {
            this.shootingPosition = shootingPosition;
            if(shootingDirection != default)
            {
                this.shootingDirection = shootingDirection;
            }
        }

        protected void Shoot()
        {
            beforeShotAction?.Invoke();

            if (EnemyProjectilePool.I == null)
            {
                Debug.LogWarning("There is no projectile pool in the scene");
                return;
            }

            GameObject obj = EnemyProjectilePool.I.GetProjectile(projectileType);
            EnemyProjectile projectile = obj.GetComponent<EnemyProjectile>();

            obj.SetActive(true);
            projectile.SetUpProjectile(shootingPosition, shootingDirection, projectileSpeed);

            afterShotAction?.Invoke();
        }
    }
}
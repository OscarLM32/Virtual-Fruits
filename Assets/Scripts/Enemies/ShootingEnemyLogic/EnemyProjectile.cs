using DevSystems.CombatSystem;
using UnityEngine;

namespace Enemies.ShootingEnemyLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField]private EnemyType enemyType;
        [SerializeField]private ProjectileType type;
        [SerializeField]private GameObject _onCollisionParticles;
        [SerializeField]private Rigidbody2D _rb;

        private int _attackPower;

        public void SetUpProjectile(Vector2 position, Vector2 direction, float speed, int attackPower = 1)
        {
            transform.position = position;
            _rb.velocity = direction * speed;
            _attackPower = attackPower;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(_onCollisionParticles != null)
            {
                Instantiate(_onCollisionParticles, gameObject.transform.position, Quaternion.identity);
            }

            var killable = collision.gameObject.GetComponent<IKillable>();
            if(killable != null) killable.Kill(new KillContext(_attackPower, enemyType));

            EnemyProjectilePool.I.DeleteProjectile(type, gameObject);
        }
    }
}